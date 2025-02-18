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
        private const bool _test = true;

        private static void Run()
        {
            int seq = 0;

            // Crear un bloque try-catch para manejar excepciones
            try
            {
                // Bucle infinito para recibir mensajes del cliente
                while (true)
                {
                    // Recibir el mensaje del cliente
                    byte[] receivedBytes = _client.Receive(ref _ip);

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

                        // Comandos para comprobar el correcto funcionamiento
                        Console.WriteLine($"Secuencia recibida: {msg.Seq} " +
                            $"Mensaje recibido: {msg.Number}");
                        seq++;

                        // Si la secuencia es correcta -> Incrementar la secuencia
                    }
                    else if (_test)
                    {
                        /*
                         * Si la secuencia es incorrecta -> No incrementar la secuencia.
                         * Mostrar el mensaje duplicado y la secuencia esperada.
                         */

                        Console.WriteLine($"Mensaje duplicado: {msg.Number} " +
                            $"Secuencia Recibida: {msg.Seq} Secuencia Esperada: {seq}");
                    }

                    // Crear un mensaje de respuesta para el cliente con la secuencia
                    Response(msg.Seq);

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

        public static void Response(int seq)
        {
            /*
             * Este método se emplea para el envio del ACK de manera
             * que exista la posibilidad de que falle.
             */
            ACK res = new ACK(seq);
            byte[] ack = res.Encode();
            var rand = new Random();
            if (rand.Next(100) > _probFallo)
            {
                _client.Send(ack, ack.Length, _ip);
            }
            else if (_test)
            {
                Console.WriteLine("Se ha perdido el ACK.");
            }
        }
    }
}
