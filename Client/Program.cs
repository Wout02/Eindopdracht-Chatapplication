using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace  Client
{
    class Program
    {
        private static string password;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static string username;
        private static bool connected = false;
        private static bool userConnected = false;


        static void Main(string[] args)
        {
           
            Console.WriteLine("Username? ");
            username = Console.ReadLine();
            Console.WriteLine("Password? ");
            password = Console.ReadLine();

            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);

            while (true)
            {
                if (connected)
                {
                    Console.WriteLine("Chat:");
                    string newChatMessage = Console.ReadLine();
                    write($"{username}:{newChatMessage}");
                }
                else
                {
                    Console.WriteLine("Connecting...");
                }
            }
        }

        private static void OnConnect(IAsyncResult ar)
        {
            client.EndConnect(ar);
            Console.WriteLine("Connected!");
            connected = true;
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);          
            write($"{username} is connected\r\n");
            
            
        }

        private static void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);
            string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            totalBuffer += receivedText;

           
        }
        private static void write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
    }
}