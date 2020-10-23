using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using Server;

namespace Gui
{
    public partial class FormChat : Form
    {
        private  string user;
        private List<string> users;
        private static TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static bool connected = false;


        public FormChat(string user, List<string> users)
        {
          
            client = new TcpClient();
            client.BeginConnect("localhost", 15243, new AsyncCallback(OnConnect), null);
            this.user = user;
            this.users = users;

            InitializeComponent();

}

        private void OnConnect(IAsyncResult ar)
        {
            if (connected)
            {
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            }
            else
            {
                client.EndConnect(ar);
                textBox1.Text += "Connected!\r\n";
                connected = true;
                stream = client.GetStream();
                stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
                write($"{user} is connected\r\n");
            }
            


        }

        private void OnRead(IAsyncResult ar)
        {
            

            int receivedBytes = stream.EndRead(ar);
            string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            
            textBox1.Text += receivedText;

            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }
        private static void write(string data)
        {
            var dataAsBytes = Encoding.ASCII.GetBytes(data);
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox2.Text;
            string temp = user + ": " + message + "\r\n";
            if (message.Length > 0)
            { 
                textBox1.Text += temp;
                write(temp);
                textBox2.Text = "";
            }
        }

        private void CheckEnterKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return) 
            {
                string message = textBox2.Text;
                string temp = user + ": " + message + "\r\n";
                if (message.Length > 0)
                {
                    textBox1.Text += temp;                  
                    write(temp);
                    textBox2.Text = "";
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "New Chat message";
            notifyIcon1.BalloonTipText = "There is a new message in the chat";
            if (this.WindowState == FormWindowState.Minimized)
            { 
                if (notifyIcon1.BalloonTipText != null)
                {
                    notifyIcon1.ShowBalloonTip(1000);
                }
            }
            
        }

        public void AddOnlineUser(object sender, EventArgs e)
        {

            OnlineUsers.Items.Add(user);
           
        }
    }
}
