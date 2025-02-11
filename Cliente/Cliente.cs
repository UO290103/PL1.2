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
        bool conexion = true; //Booleano que indica cuando la conexión está funcionando
        int Prob_Fallo = 50; //Porcentaje de fallo en el envio del sistema (Entre 0 y 100)
        int seq = 0; //Variable donde se guarda el numero de secuencia 
        int num; //Variable donde se guarda el numero a enviar
        string[] numeros; //Array donde se guardara todos los numeros a enviar
        byte[] data;
        public void Run()
        {  
            try //Bloque Try-catch unico para la lectura del archivo de texto
            {
                //Lista para ir guardando los numeros antes de en el array y string para las lineas del archivo.
                List<string> list = new List<string>();
                string linea;
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
                Secuencia.Close();
            }
            catch (Exception ex) {
                //Si hay un error durante este proceso se indica en consola, se cierra la conexión y se retorna
                Console.WriteLine("Error durante la lectura del archivo: "+ex.Message);
                conexion = false;
            }
            cliente.Client.ReceiveTimeout = 2000;
            var Rand = new Random();
            while (conexion)
            {
                try //Bloque Try-catch para el envio de datos y recibo de ACKs
                {
                    //Bucle para el envio y recepción no para hasta que el numero de secuencia sea mayor a la cantidad de numeros que tengamos que mandar
                    while (seq <= numeros.Length)
                    {
                        //Comprobamos si seq es 0, en ese caso se manda el mensaje para iniciar la conexión
                        if (seq == 0)
                        {
                            //Creamos el mensaje con seq = 0 y un numero que no importa y lo codificamos a un array de bytes
                            Datos msg = new Datos(seq, 0);
                            data = msg.Code();
                            //Mandamos dicho array de bytes por la conexión
                            cliente.Send(data, data.Length, ip);
                        }
                        else //Else, resto de casos que no sean el primer mensaje de la conexión
                        {
                            //Se crea un numero entre 0 y 99, si este numero es menor a Prob_Fallo no se envía el mensaje.
                            if (Rand.Next(100) > Prob_Fallo)
                            {
                                //Obtenemos como entero el numero correspondiente del array
                                num = Int32.Parse(numeros[seq - 1]);
                                //Creamos el mensaje con seq correspondiente y numero correspondiente
                                Datos msg = new Datos(seq, num);
                                data = msg.Code();
                                //Mandamos array de bytes por la conexión
                                cliente.Send(data, data.Length, ip);
                                Console.WriteLine("Se ha enviado el numero");
                            }
                            else 
                            {
                                Console.WriteLine("Se ha fallado en el envío");
                            }
                        }
                        ACK ack = new ACK(-1);
                        while (ack.seq != seq)
                        {
                            //Esperamos a recibir la ACK que envía el servidor
                            data = cliente.Receive(ref ip);
                            Console.WriteLine("Se recibe ACK");
                            //Creamos la ACK vacia y decodificamos lo recibido en ella
                            ack.Decode(data);
                        }
                        Console.WriteLine("Seq: " + seq);
                        seq++;
                                          
                        /*
                        //Esperamos a recibir la ACK que envía el servidor
                        data = cliente.Receive(ref ip);
                        Console.WriteLine("Se ha recibido ACK");
                        //Creamos la ACK vacia y decodificamos lo recibido en ella
                        ACK Ack = new ACK();
                        Ack.Decode(data);
                        if(Ack.seq == seq)
                        {
                            Console.WriteLine("Seq correcto");
                            seq++;
                        }
                        else
                        {
                            Console.WriteLine("Seq incorrecto");
                        }
                        */
                    }
                    conexion = false;
                }

                catch (SocketException se)
                {
                    if (se.SocketErrorCode == SocketError.TimedOut)
                    {
                        // Ha habido un timeout: Se indica por consola que se va a reenviar el dato
                        Console.WriteLine("Ha habido un timeout. Se procede a reenviar la información");
                    }

                    else
                    {
                        //Otros errores: Mostramos por pantalla y cerramos la conexión
                        Console.WriteLine(se.ErrorCode + ": " + se.Message);
                        conexion = false;
                    }
                }
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
