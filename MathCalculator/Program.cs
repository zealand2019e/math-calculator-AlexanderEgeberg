using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

namespace MathCalculator
{ 
    class Program
    {
        static void Main(string[] args)
        {
            Server.Start();
        }
    } 
    class Server
        {
            static int _clientNr = 0;

            public static void Start()
            {
                int port = 3002;
                TcpListener listener = new TcpListener(IPAddress.Loopback, port);
                listener.Start();
                Console.WriteLine("Server started...");

                while (true)
                {
                    TcpClient socket = listener.AcceptTcpClient();
                    _clientNr++;
                    Console.WriteLine("User connected");
                    Console.WriteLine($"Number of users online {_clientNr}");

                    Task.Run(() =>
                    {
                       // TcpClient tempSocket = socket;
                        DoClient(socket);
                    }
                    );
                }

            }

            public static void DoClient(TcpClient socket)
            {
                NetworkStream ns = socket.GetStream();
                StreamReader reader = new StreamReader(ns);
                StreamWriter writer = new StreamWriter(ns) { AutoFlush = true };

                writer.WriteLine("Connected to server...");

                string[] wordsArray;
                List<int> intList = new List<int>();
                try
                {

                    while (true) //inputLine != null && inputLine != " "
                    {
                        int result = 0;
                        string inputLine = "";
                        inputLine = reader.ReadLine();

                        if (inputLine == null)
                        {
                            ns.Close();
                            break;
                        }

                        wordsArray = inputLine.Split("+");

                            foreach (string s in wordsArray)
                            {
                                intList.Add(Int32.Parse(s));
                            }

                            foreach (var number in intList)
                            {
                                result = result + number;
                            }
                        

                        Console.WriteLine($"Result: {result}");
                        writer.WriteLine(result);
                        intList.Clear();
                    }
                }
                catch (Exception e)
                {
                    
                    if (e.Message == "Unable to read data from the transport connection: En eksisterende forbindelse blev tvangsafbrudt af en ekstern vært..")
                    {
                        ns.Close();
                    }
                    else
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }

                ns.Close();
                _clientNr--;
                Console.WriteLine($"User disconnected... current number of users: {_clientNr}");
            }
        }
    }
