using System;
using System.Net.Sockets;

namespace Cliente
{
    class Cliente
    {
        public void Run()
        {
            TcpClient client = null;
            NetworkStream netStream = null;

            try
            {
                client = new TcpClient("localhost", 50000);
                netStream = client.GetStream();
                Console.WriteLine("Conectado al servidor.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
            }
            finally
            {
                if (netStream != null)
                    netStream.Close();

                if (client != null)
                    client.Close();
            }
        }

        // Método Main como punto de entrada
        static void Main()
        {
            Cliente cliente = new Cliente();
            cliente.Run();
        }
    }
}
