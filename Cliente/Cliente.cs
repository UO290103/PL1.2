using System;
using System.Net;
using System.Net.Sockets;

namespace ClienteUDP
{
    class Cliente
    {
        static void Main()
        {
            UdpClient udpClient = new UdpClient();
            IPEndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 50000);

            try
            {
                // Aquí se debe desarrollar el envio del mensaje
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
            }
            finally
            {
                udpClient.Close();
            }
        }
    }
}
