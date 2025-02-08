using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Vocabulario;

namespace Servidor
{
    public class server
    {
        private const int port = 50000;

        private static void Run()
        {
            // Inicializamos el puerto UDP y la dirección IP
            UdpClient cliente = new UdpClient(port);
            IPEndPoint ip = new IPEndPoint(IPAddress.Any, port);
            bool conexion = false;

            int seq = 0;

            // Creamos una instancia try-catch para manejar excepciones

            try
            {
                // Bucle infinito para realizar la recepción de mensajes por parte del cliente.
                while (true)
                {

                    if (seq == 0)
                    {
                        Console.WriteLine("Esperando conexión con el cliente...");
                        seq++;
                        Console.WriteLine(seq); //Mensaje de comprobación
                    }

                    // Debemos recibir el mensaje por parte del cliente.
                    byte[] bytes = cliente.Receive(ref ip);

                    // Implementamos verificación de conexión.
                    if (!conexion)
                    {
                        Console.WriteLine("Conexión establecida con el cliente.");
                        conexion = true;
                    }

                    // Debemos de convertir los datos recibidos a un string decodificandolos.
                    Datos msg = new Datos();
                    msg.Decode(bytes);

                    // Guardaremos los datos recibidos en un archivo de texto. -> No implementado.

                    // Deberemos de crear un comprobador de correspondencia de la secuencia con el mensaje recibido.
                    if (msg.seq == seq)
                    {
                        Console.WriteLine("Mensaje recibido: " + msg.num);
                        seq++;
                    }

                    // Esta variable se usa durante la comprobación de funcionamiento! El mensaje se descartaría.
                    else
                    {
                        Console.WriteLine("Mensaje duplicado: " + msg.num);
                        Console.WriteLine("Secuencia esperada: " + seq);
                        Console.WriteLine("Secuencia recibida: " + msg.seq);
                    }

                    // Creamos un mensaje de confirmación para el cliente con la secuencia deferente.
                    ACK ack = new ACK(seq);
                    ack.Code();

                    // Ahora enviamos este mensaje ACK al cliente para confirmar la recepción del mensaje. -> No implementado.
                }
            }

            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            finally
            {
                Console.WriteLine("Conexión finalizada.");
                cliente.Close();
            }
        }

        public static void Main()
        {
            Run();
        }
    }
}
