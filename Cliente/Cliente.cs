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
        private UdpClient _cliente = new UdpClient();  // Cliente UDP
        private IPEndPoint _ip = new IPEndPoint(IPAddress.Loopback, 50000);
        private string _path = "C:\\Secuencias\\Secuencia.txt"; //Path al fichero del que se obtienen los números a transmitir
        private bool _conexion = true;  // Booleano que indica cuando la conexión está activa o no
        private const int _probFallo = 20;  // Porcentaje de fallo en el envío de mensajes (Entre 0 y 100)
        private int _seq = 0;  // Número de secuencia del mensaje
        private sbyte[] _numeros;  // Array de enteros donde se guardan los números a transmitit
        private byte[] _data;  // Array de bytes donde se codifica y decodifica la información
        private bool _test = true;
        private FileReader numReader = new FileReader(); // Invocamos el método para leer los números
        
        public void Send(int seq, sbyte num)
        {
            // Creamos el mensaje con seq correspondiente y número correspondiente
            Data msg = new Data(seq, num);
            _data = msg.Encode();
            // Mandamos array de bytes por la conexión
            _cliente.Send(_data, _data.Length, _ip);
            //Console.WriteLine("Se ha enviado el número");
        }

        public void Run()
        {
            _numeros = numReader.Reader(_path);

            _cliente.Client.ReceiveTimeout = 2000;
            var rand = new Random();

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
                            //Mandamos el primer mensaje con _seq = 0 y un número sin importancia
                            Send(_seq, 0);
                        }

                        else // Else, resto de casos que no sean el primer mensaje de la conexión
                        {
                            // Se crea un número entre 0 y 99, si este número es menor a Prob_Fallo no se envía el mensaje.
                            if (rand.Next(100) > _probFallo)
                            {
                                //Se envía el mensaje con seq y num correspondiente
                                Send(_seq, _numeros[_seq-1]);
                            }
                            /*
                            else
                            {
                                Console.WriteLine("Se ha fallado en el envío");
                            }
                            */
                        }

                        ACK ack = new ACK(-1);
                        while (ack.SequenceNumber != _seq)
                        {
                            // Esperamos a recibir la ACK que envía el servidor
                            _data = _cliente.Receive(ref _ip);
                            Console.WriteLine("Se recibe ACK");
                            // Creamos la ACK vacía y decodificamos lo recibido en ella
                            ack.Decode(_data);
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

        public static void Main()
        {
            Cliente _c = new Cliente();
            _c.Run();
        }
    }
}
