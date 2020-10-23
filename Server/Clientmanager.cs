using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;

namespace Server
{
    public class Clientmanager
    {
        
        private TcpClient tcpClient;
        private NetworkStream stream;
        private byte[] buffer = new byte[1024];
        private string totalBuffer = "";

        public string UserName { get; set; }


        public Clientmanager(TcpClient tcpClient)
        {
            this.tcpClient = tcpClient;

            this.stream = this.tcpClient.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
       
        public void OnRead(IAsyncResult ar)
        {
            try
            {
                int receivedBytes = stream.EndRead(ar);
                string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
                totalBuffer += receivedText;
                Program.SendMessage(this, receivedText);
            }
            catch (IOException)
            {
                Program.Disconnect(this);
                return;
            }

            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
      

        public void Write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }
    }
}