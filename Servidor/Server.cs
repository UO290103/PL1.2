using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ServidorUDP
{
    class Server
    {
        public void Run()
        {
            UdpClient udpServer = new UdpClient(50000);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

            Console.WriteLine("Servidor UDP esperando conexión en el puerto 50000...");

            // Aquí se debe desarrollar la recepción y guardado de los mensajes.

        }

        static void Main()
        {
            Server server = new Server();
            server.Run();
        }
    }
}
