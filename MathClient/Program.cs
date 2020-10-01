using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;

namespace MathClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Client.Start();
        }
    }

    class Client
    {
        public static void Start()
        {
            int port = 3002;

            TcpClient client = new TcpClient("localhost", port);

            using (client)
            {
                ConnectServer(client);
            }
        }
        public static void ConnectServer(TcpClient client)
        {
            Stream ns = client.GetStream();
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true;

 
            Console.WriteLine(sr.ReadLine());
            while (true)
            {
                string mathExpression = Console.ReadLine();
                sw.WriteLine(mathExpression);
                var result = sr.ReadLine();
                Console.WriteLine(result);
            }

        }

    }
}
