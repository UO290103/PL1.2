using System;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace Server
{
    public class Server
    {
        // Como solo empleamos una conexión clinete-servidor, hacemos "static"
        private const int Port = 50000;
        private static UdpClient _client = new UdpClient(Port);
        private static IPEndPoint _ip = new IPEndPoint(IPAddress.Any, Port);
        private const int _probFallo = 20;


        // Métodos
        private static void Response(int s)
        {
            /*
             * El método Response, recibe la secuencia que quiere emitir
             * el servidor, crea un ACK, la codifica y finalmente la envia
             * al emisor como confirmación de recepción.
             */
            byte[] ack;
            ACK msg = new ACK(s);
            ack= msg.Encode();
            _client.Send(ack, ack.Length, _ip);
        }

        private static void Run()
        {
            bool isConnected = false;
            int seq = 0;

            // Crear un bloque try-catch para manejar excepciones
            try
            {
                // Bucle infinito para recibir mensajes del cliente
                while (true)
                {
                    // Recibir el mensaje del cliente
                    byte[] receivedBytes = _client.Receive(ref _ip);

                    // Verificar conexión
                    if (!isConnected)
                    {
                        Console.WriteLine("Conexión establecida con el cliente.");
                        isConnected = true;
                    }

                    // Convertir los datos recibidos a una cadena decodificándolos
                    Data msg = new Data();
                    msg.Decode(receivedBytes);

                    // Verificar si la secuencia recibida coincide con la secuencia esperada
                    if (msg.Seq == seq)
                    {
                        if (msg.Seq == 0)
                        {
                            Console.WriteLine("El cliente comienza a transmitir.");
                        }

                        Console.WriteLine("Secuencia recibida: {0} Mensaje recibido: {1}",
                            msg.Seq, msg.Number);
                        seq++;

                        // Si la secuencia es correcta -> Incrementar la secuencia
                    }
                    else
                    {
                        /*
                         * Si la secuencia es incorrecta -> No incrementar la secuencia.
                         * Mostrar el mensaje duplicado y la secuencia esperada.
                         */

                        Console.WriteLine("Mensaje duplicado: {0} Secuencia Recibida: {1} Secuencia Esprada: {2}",
                            msg.Number, msg.Seq, seq);
                    }

                    // Crear un mensaje de respuesta para el cliente con la secuencia
                    Response(msg.Seq);

                    // Hacemos que exista la posibilidad de que un ACK no llegue al cliente.
                    var _rand = new Random();
                    if (_rand.Next(100) > _probFallo)
                    {
                        // Enviar el mensaje de reconocimiento al cliente
                        Response(msg.Seq);
                    }
                    else
                    {
                        Console.WriteLine("El ACK se ha perdido.");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Conexión terminada.");
                _client.Close();
            }
        }

        public static void Main()
        {
            Run();
        }
    }
}
