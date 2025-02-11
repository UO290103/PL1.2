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

            byte[] ack;
            int seq = 0;

            // Creamos una instancia try-catch para manejar excepciones

            try
            {
                // Bucle infinito para realizar la recepción de mensajes por parte del cliente.
                while (true)
                {
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

                    if (msg.seq == 0)
                    {
                        Console.WriteLine("Conexión establecida con el cliente.");
                    }

                    // Guardaremos los datos recibidos en un archivo de texto. -> No implementado.

                    // Deberemos de crear un comprobador de correspondencia de la secuencia con el mensaje recibido.
                    if (msg.seq == seq)
                    {
                        Console.WriteLine("Secuencia recibida: " + msg.seq + " Mensaje recibido: " + msg.num);
                        
                        // En caso de seq correcta -> Incrementamos la secuencia.
                        seq++;
                    }

                    // Esta variable se usa durante la comprobación de funcionamiento! El mensaje se descartaría.
                    else
                    {
                        /* 
                         * En caso de seq incorrecta -> No incrementamos la secuencia.
                         * Además mostramos por consola el mensaje duplicado y la secuencia esperada.
                         */

                        Console.WriteLine("Mensaje duplicado: " + msg.num);
                        Console.WriteLine("Secuencia esperada: " + seq);
                        Console.WriteLine("Secuencia recibida: " + msg.seq);
                        
                        
                    }

                    // Creamos un mensaje de confirmación para el cliente con la secuencia deferente.
                    ACK res = new ACK(msg.seq);
                    ack = res.Code();

                    // Enviamos el mensaje de confirmación al cliente.
                    cliente.Send(ack, ack.Length, ip);


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
