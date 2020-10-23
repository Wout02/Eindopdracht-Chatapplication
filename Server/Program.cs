using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
            clients.Add(new Clientmanager(tcpClient));           
            NotifyClients(clients, $"Client connected from {tcpClient.Client.RemoteEndPoint}\r\n");
            
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
    }
}
