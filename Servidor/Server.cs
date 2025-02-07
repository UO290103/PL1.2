using System;
using System.Net;
using System.Net.Sockets;

namespace Servidor
{
    class Server
    {
        public void Run()
        {
            TcpListener listener = null;

            try
            {
                listener = new TcpListener(IPAddress.Any, 50000);
                listener.Start();
                Console.WriteLine("Servidor escuchando en el puerto 50000...");
            }
            catch (SocketException se)
            {
                Console.WriteLine("Error al iniciar el servidor: {0}", se.Message);
                return;
            }

            while (true)
            {
                TcpClient client = null;
                NetworkStream netStream = null;

                try
                {
                    Console.WriteLine("Esperando conexión de un cliente...");
                    client = listener.AcceptTcpClient();
                    Console.WriteLine("Cliente conectado.");

                    netStream = client.GetStream();

                    // Responder al cliente (opcional)
                    byte[] message = System.Text.Encoding.UTF8.GetBytes("¡Conexión establecida!");
                    netStream.Write(message, 0, message.Length);

                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: {0}", e.Message);
                }
                finally
                {
                    if (netStream != null) netStream.Close();
                    if (client != null) client.Close();
                }
            }
        }

        static void Main()
        {
            Server server = new Server();
            server.Run();
        }
    }
}
