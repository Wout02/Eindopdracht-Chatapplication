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
using MySql.Data.MySqlClient.Memcached;
using System.IO;

namespace Gui
{
    public partial class FormChat : Form
    {
        private string user;
        private List<string> users;
        private TcpClient client;
        private static NetworkStream stream;
        private static byte[] buffer = new byte[1024];
        private static string totalBuffer;
        private static bool connected = true;


        public FormChat(string user, List<string> users, TcpClient client)
        {
            InitializeComponent();

            this.client = client;
            this.user = user;
            this.users = users;

            OnConnect();
        }

        private void OnConnect()
        {
            
            textBox1.Text += "Connected!\r\n";
            connected = true;
            Console.WriteLine("ASDBASDBIASBDAS");
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);

            foreach (string user in users)
            {
                write($"{user} is connected\r\n");
                Console.WriteLine(user);
            }

            //write($"{user} is connected\r\n");
            Console.WriteLine("DONEEEEEEEEEEEEEEE");


        }

        private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);
            string receivedText = Encoding.ASCII.GetString(buffer, 0, receivedBytes);

            if (receivedText.Contains(" is connected\r\n"))
            {
                string temp = receivedText.Substring(0, receivedText.IndexOf(" "));
                if (temp != "")
                {
                    OnlineUsers.Items.Add(temp);
                }
            }

            totalBuffer += receivedText;
            textBox1.Text = totalBuffer;

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();
            
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
        }

        private void write(string data)
        {
            totalBuffer += data;

            textBox1.SelectionStart = textBox1.Text.Length;
            textBox1.ScrollToCaret();

            var dataAsBytes = Encoding.ASCII.GetBytes(data);
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();

            writeToFile(data);


        }

        private void writeToFile(string data)
        {
            using (StreamWriter file = File.AppendText(Directory.GetCurrentDirectory() + "\\" + user + ".txt"))
            {
                file.WriteLine(data);
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            Console.WriteLine("GETTING LOG");
           
            String filePath = Directory.GetCurrentDirectory() + "\\" + OnlineUsers.SelectedItem.ToString() + ".txt";
            Console.WriteLine(filePath);
            if (File.Exists(filePath))
            {
                System.Diagnostics.Process.Start("notepad.exe" ,filePath);
            }
        }
    }
}
