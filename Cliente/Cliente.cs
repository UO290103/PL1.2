using System;
using System.Net;
using System.Net.Sockets;
using Vocabulario;

namespace ClienteUDP
{
    public class Cliente
    {
        UdpClient cliente = new UdpClient();
        IPEndPoint ip = new IPEndPoint(IPAddress.Loopback, 50000);
        
        // Número de secuencia
        int seq = 1;
        

        public void Run()
        {
            try
            {
                Datos msg = new Datos(seq, 0); // Mensaje para establecer comunicación.
                byte[] bytes = msg.Code();

                // Enviamos un "0" para que el servidor saque por consola que se ha establecido conexión.
                if (seq == 1)
                {
                    cliente.Send(bytes, bytes.Length, ip);
                    seq++;
                }

                // Enviamos un número
                int num = 9;
                msg = new Datos(seq, num);
                bytes = msg.Code();
                cliente.Send(bytes, bytes.Length, ip);
                seq++;
            }

            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static void Main(string[] args)
        {
            Cliente c = new Cliente();
            c.Run();
        }
    }
}
