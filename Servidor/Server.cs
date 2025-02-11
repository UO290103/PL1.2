using System;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace Server
{
    public class Server
    {
        private const int Port = 50000;
        private int failureProbability = 50;

        private static void Run()
        {
            // Inicializar el puerto UDP y la dirección IP
            UdpClient client = new UdpClient(Port);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, Port);
            bool isConnected = false;

            byte[] acknowledgment;
            int sequenceNumber = 0;

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
                    if (msg.Seq == sequenceNumber)
                    {
                        if (msg.Seq == 0)
                        {
                            Console.WriteLine("El cliente comienza a transmitir.");
                        }

                        Console.WriteLine("Secuencia recibida: {0} Mensaje recibido: {1}",
                            msg.Seq, msg.Number);
                        sequenceNumber++;

                        // Si la secuencia es correcta -> Incrementar la secuencia
                    }
                    else
                    {
                        /*
                         * Si la secuencia es incorrecta -> No incrementar la secuencia.
                         * Mostrar el mensaje duplicado y la secuencia esperada.
                         */

                        Console.WriteLine("Mensaje duplicado: " + msg.Number);
                        Console.WriteLine("Secuencia esperada: " + sequenceNumber);
                        Console.WriteLine("Secuencia recibida: " + msg.Seq);
                    }

                    // Crear un mensaje de respuesta para el cliente con la secuencia
                    ACK response = new ACK(msg.Seq);
                    acknowledgment = response.Encode();

                    // Enviar el mensaje de reconocimiento al cliente
                    client.Send(acknowledgment, acknowledgment.Length, ip);

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
