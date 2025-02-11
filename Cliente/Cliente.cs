using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace Cliente
{
    public class Cliente
    {
        private UdpClient _cliente = new UdpClient();  // privado
        private IPEndPoint _ip = new IPEndPoint(IPAddress.Loopback, 50000);  // privado
        private bool _conexion = true;  // privado
        private const int _probFallo = 20;  // privado
        private int _seq = 0;  // privado
        private int _num;  // privado
        private string[] _numeros;  // privado
        private byte[] _data;  // privado

        public void Run()
        {
            try // Bloque Try-catch único para la lectura del archivo de texto
            {
                // Lista para ir guardando los números antes de en el array y string para las líneas del archivo.
                List<string> _list = new List<string>();
                string _linea;

                // Inicializamos stream de lectura con el path del archivo a leer
                StreamReader _secuencia = new StreamReader("C:\\Secuencias\\Secuencia.txt");

                // Leemos la primera línea antes de entrar al bucle
                _linea = _secuencia.ReadLine();

                // Bucle para leer todas las líneas que haya en el archivo
                while (_linea != null)
                {
                    // Las líneas deben ser de números y que estén separados por comas Ej 9,1,3,4
                    // Separamos la línea por las comas en un array
                    string[] _numsLinea = _linea.Split(",");
                    for (int i = 0; i < _numsLinea.Length; i++)
                    {
                        // Recorremos toda la longitud del array y lo añadimos a una lista
                        _list.Add(_numsLinea[i]);
                    }
                    // Leemos la siguiente línea del archivo
                    _linea = _secuencia.ReadLine();
                }
                // Pasamos la lista completa a un array
                _numeros = _list.ToArray();
                _secuencia.Close();
            }
            catch (Exception ex)
            {
                // Si hay un error durante este proceso se indica en consola, se cierra la conexión y se retorna
                Console.WriteLine("Error durante la lectura del archivo: " + ex.Message);
                _conexion = false;
            }

            _cliente.Client.ReceiveTimeout = 2000;
            var _rand = new Random();

            while (_conexion)
            {
                try // Bloque Try-catch para el envío de datos y recibo de ACKs
                {
                    // Bucle para el envío y recepción, no para hasta que el número de secuencia sea mayor a la cantidad de números que tengamos que mandar
                    while (_seq <= _numeros.Length)
                    {
                        // Comprobamos si seq es 0, en ese caso se manda el mensaje para iniciar la conexión
                        if (_seq == 0)
                        {
                            // Creamos el mensaje con seq = 0 y un número que no importa y lo codificamos a un array de bytes
                            Data _msg = new Data(_seq, 0);
                            _data = _msg.Encode();
                            // Mandamos dicho array de bytes por la conexión
                            _cliente.Send(_data, _data.Length, _ip);
                        }

                        else // Else, resto de casos que no sean el primer mensaje de la conexión
                        {
                            // Se crea un número entre 0 y 99, si este número es menor a Prob_Fallo no se envía el mensaje.
                            if (_rand.Next(100) > _probFallo)
                            {
                                // Obtenemos como entero el número correspondiente del array
                                _num = int.Parse(_numeros[_seq - 1]);
                                // Creamos el mensaje con seq correspondiente y número correspondiente
                                Data _msg = new Data(_seq, _num);
                                _data = _msg.Encode();
                                // Mandamos array de bytes por la conexión
                                _cliente.Send(_data, _data.Length, _ip);
                                Console.WriteLine("Se ha enviado el número");
                            }

                            else
                            {
                                Console.WriteLine("Se ha fallado en el envío");
                            }
                        }

                        ACK _ack = new ACK(-1);
                        while (_ack.SequenceNumber != _seq)
                        {
                            // Esperamos a recibir la ACK que envía el servidor
                            _data = _cliente.Receive(ref _ip);
                            Console.WriteLine("Se recibe ACK");
                            // Creamos la ACK vacía y decodificamos lo recibido en ella
                            _ack.Decode(_data);
                        }
                        Console.WriteLine("Seq: " + _seq);
                        _seq++;
                    }
                    _conexion = false;
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
                        // Otros errores: Mostramos por pantalla y cerramos la conexión
                        Console.WriteLine(se.ErrorCode + ": " + se.Message);
                        _conexion = false;
                    }
                }
            }
            _cliente.Close();
        }

        public static void Main(string[] args)
        {
            Cliente _c = new Cliente();
            _c.Run();
        }
    }
}
