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

            // Inicializamos la variable para el contador.
            int cont = 0;

            // Creamos una instancia try-catch para manejar excepciones

            try
            {
                // Bucle infinito para realizar la recepción de mensajes por parte del cliente.
                while (true)
                {

                    if (cont == 0)
                    {
                        Console.WriteLine("Esperando conexión con el cliente...");
                        cont++;
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

                    // Guardaremos los datos recibidos en un archivo de texto.
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter("datos.txt", true))
                    {
                        file.WriteLine("Secuencia: " + msg.seq + " Número: " + msg.num);
                    }
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
