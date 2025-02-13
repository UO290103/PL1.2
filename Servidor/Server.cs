using System;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace Server
{
    public class Server
    {
        private const int Port = 50000;
        private const int _probFallo = 0;

        private static void Run()
        {
            // Inicializar el puerto UDP y la dirección IP
            UdpClient client = new UdpClient(Port);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, Port);
            bool isConnected = false;

            byte[] ack;
            int seq = 0;

            // Crear un bloque try-catch para manejar excepciones
            try
            {
                // Bucle infinito para recibir mensajes del cliente
                while (true)
                {
                    // Recibir el mensaje del cliente
                    byte[] receivedBytes = client.Receive(ref ip);

                    // Verificar conexión
                    if (!isConnected)
                    {
                        Console.WriteLine("Conexión establecida con el cliente.");
                        isConnected = true;
                    }

                    // Convertir los datos recibidos a una cadena decodificándolos
                    Data msg = new Data();
                    msg.Decode(receivedBytes);

                    // Aquí almacenaríamos los datos recibidos en un archivo de texto -> No implementado

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


                    if (msg.Seq != -1)
                    {
                        // Crear un mensaje de respuesta para el cliente con la secuencia
                        ACK response = new ACK(msg.Seq);
                        ack = response.Encode();

                        // Hacemos que exista la posibilidad de que un ACK no llegue al cliente.

                        var _rand = new Random();
                        if (_rand.Next(100) > _probFallo)
                        {
                            // Enviar el mensaje de reconocimiento al cliente
                            client.Send(ack, ack.Length, ip);
                        }
                        else
                        {
                            Console.WriteLine("El ACK se ha perdido.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("La comunicación ha finalizado.");
                        seq = 0;
                        ACK final = new ACK(-1);
                        ack = final.Encode();
                        client.Send(ack, ack.Length, ip);
                        isConnected = false;
                    }
                    

                    // El reconocimiento se envía de vuelta al cliente para confirmar la recepción del mensaje -> No implementado
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                Console.WriteLine("Conexión terminada.");
                client.Close();
            }
        }

        public static void Main()
        {
            Run();
        }
    }
}
