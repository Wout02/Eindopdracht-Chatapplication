using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Gui;

namespace Server
{
   public static class Program
    {
        private static TcpListener listener;
        private static List<Clientmanager> clients = new List<Clientmanager>();
        private static string totalBuffer = "";


        static void Main(string[] args)
        {
            Console.WriteLine("Server");

            listener = new TcpListener(IPAddress.Any, 15243);
            listener.Start(); 
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
            

            Console.ReadLine();
            
        }

        private static void OnConnect(IAsyncResult ar)
        {
            
            var tcpClient = listener.EndAcceptTcpClient(ar);
                        
            Console.WriteLine($"Client connected from {tcpClient.Client.RemoteEndPoint}");

            Clientmanager man = new Clientmanager(tcpClient);


            foreach (Clientmanager c in clients)
            {
                man.Write($"{c.UserName} is connected" );
                Console.WriteLine($"{c.UserName} is connected");
            }
            clients.Add(man);

            Thread notifyClients = new Thread(() =>
            {
                NotifyClients(clients, $"Client connected from {tcpClient.Client.RemoteEndPoint}\r\n");
            });

            notifyClients.Start();
            
            
            listener.BeginAcceptTcpClient(new AsyncCallback(OnConnect), null);
        }

       

        internal static void NotifyClients(List<Clientmanager> clients, string data)
        {
            foreach(Clientmanager client in clients)
            {
                client.Write(data);
            }
        }

        internal static void Disconnect(Clientmanager clientManager)
        {
            clients.Remove(clientManager);
            Console.WriteLine("Client disconnected");
            NotifyClients(clients, "Client disconnected");
            
        }

        public static void SendMessage(Clientmanager manager, string message)
        {
            foreach (Clientmanager client in clients)
            {
                if (manager != client)
                {
                    client.Write(message);
                }
            }
        }

    }
}
