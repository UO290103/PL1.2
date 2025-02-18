﻿using System;
using System.Net;
using System.Net.Sockets;
using Vocabulario;
using System.IO;
using System.Collections.Generic;

namespace Servidor
{
    public class Server
    {
        private const int _port = 50000;
        private const int _probFallo = 20;
        private const bool _test = true;
        static UdpClient client = new UdpClient(_port);
        static IPEndPoint ip = new IPEndPoint(IPAddress.Any, _port);

        private static void Run()
        {
            // Inicializar el puerto UDP y la dirección IP
            
            string path = "C:\\Secuencias\\Recibidas\\data.txt";
            FileWriter writer = new FileWriter();
            


            // Crear un bloque try-catch para manejar excepciones
            try
            {
                while (true)  // Bucle principal para el servidor
                {
                    Console.WriteLine("Esperando conexión del cliente...");

                    List<sbyte> ints = new List<sbyte>();  // Nueva lista para cada cliente
                    bool isConnected = false; 
                    int seq = 0;

                    // Esperamos el primer mensaje.
                    byte[] receivedBytes = client.Receive(ref ip);
                    Data msg = new Data();
                    msg.Decode(receivedBytes);

                    Console.WriteLine("Conexión establecida con el cliente.");
                    isConnected = true; // Activamos para entrar en el bucle de transferencia de datos.

                    seq++; // Aumentamos la secuenci para comenzar a recibir los datos

                    while (isConnected)
                    {
                        receivedBytes = client.Receive(ref ip);
                        msg.Decode(receivedBytes);

                        if (msg.Seq == -1)
                        {
                            Console.WriteLine("Cliente desconectado. Guardando datos...");
                            Response(msg.Seq);

                            writer.Writer(ints, path);
                            isConnected = false;
                            break;  // Salir de este cliente, pero el servidor sigue activo
                        }

                        else if (msg.Seq == seq)
                        {
                            if (_test)
                            {
                                Console.WriteLine($"Seq: {seq} Num: {msg.Number}");
                            }

                            // Agregamos el número a la lista
                            ints.Add((sbyte)msg.Number);
                            seq++;
                        }


                        // Ahora hacemos el envio del ACK
                        Response(msg.Seq);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Servidor apagado.");
                client.Close();
            }
        }

        public static void Main()
        {
            Run();
        }

        public static void Response(int seq)
        {
            /*
             * Este método se emplea para el envio del ACK de manera
             * que exista la posibilidad de que falle.
             */
            ACK res = new ACK(seq);
            byte[] ack = res.Encode();

            var rand = new Random();
            if (rand.Next(100) > _probFallo)
            {
                client.Send(ack, ack.Length, ip);
            }
            else if (_test)
            {
                Console.WriteLine("Se ha perdido el ACK.");
            }
        }
    }
}
