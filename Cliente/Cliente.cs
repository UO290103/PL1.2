using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace ClienteUDP
{
    public class Cliente
    {
        UdpClient cliente = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, 50000);

        // Número de secuencia
        int seq = 0;
        int num;
        string[] numeros;
        List<string> list = new List<string>();
        string linea;
        byte[] data;
        public void Run()
        {
            try //Bloque Try-catch unico para la lectura del archivo de texto
            {
                //Inicializamos stream de lectura con el path del archivo a leer
                StreamReader Secuencia = new StreamReader("C:\\Secuencias\\Secuencia.txt");
                //Leemos la primera linea antes de entrar al bucle
                linea = Secuencia.ReadLine();
                //Bucle para leer todas las lineas que haya en el archivo
                while (linea != null)
                {
                    //Las lineas deben ser de numeros y que esten separados por comas Ej 9,1,3,4
                    //Separamos la linea por las comas en un array
                    string[] nums_linea = linea.Split(",");
                    for (int i = 0; i < nums_linea.Length; i++)
                    {
                        //Recorremos toda la longitud del array y lo añadimos a una lista
                        list.Add(nums_linea[i]);
                    }
                    //Leemos la siguiente linea del archivo
                    linea = Secuencia.ReadLine();
                }
                //Pasamos la lista completa a un array
                numeros = list.ToArray();
            }
            catch (Exception ex) {
                //Si hay un error durante este proceso se indica en consola, se cierra la conexión y se retorna
                Console.WriteLine("Error durante la lectura del archivo: "+ex.Message);
                cliente.Close();
                return;
            }
            try //Bloue Try-catch para el envio de datos y recibo de ACKs
            {
                //Bucle para el envio y recepción no para hasta que el numero de secuencia sea mayor a la cantidad de numeros que tengamos que mandar
                while (seq <= numeros.Length) {
                    //Comprobamos si seq es 0, en ese caso se manda el mensaje para iniciar la conexión
                    if (seq == 0)
                    {
                        //Creamos el mensaje con seq = 0 y un numero que no importa y lo codificamos a un array de bytes
                        Datos msg = new Datos(seq, 0);
                        data = msg.Code();
                        //Mandamos dicho array de bytes por la conexión
                        cliente.Send(data,data.Length,ip);
                    }
                    else //Else, resto de casos que no sean el primer mensaje de la conexión
                    {
                        //Obtenemos como entero el numero correspondiente del array
                        num = Int32.Parse(numeros[seq - 1]);
                        //Creamos el mensaje con seq correspondiente y numero correspondiente
                        Datos msg = new Datos(seq, num);
                        data = msg.Code();
                        //Mandamos array de bytes por la conexión
                        cliente.Send(data, data.Length, ip);
                    }
                    //Esperamos a recibir la ACK que envía el servidor
                    data = cliente.Receive(ref ip);
                    //Creamos la ACK vacia y decodificamos lo recibido en ella
                    ACK Ack = new ACK();
                    Ack.Decode(data);
                    //Comprobamos si el numero de seq de la ACK corresponde con el actual
                    if (seq == Ack.seq) {
                        //En el caso de que si sea se aumenta el numero de secuencia
                        seq++;
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            cliente.Close();
        }

        public static void Main(string[] args)
        {
            Cliente c = new Cliente();
            c.Run();
        }
    }
}
