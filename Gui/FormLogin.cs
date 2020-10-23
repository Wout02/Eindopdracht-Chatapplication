using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gui
{
    public partial class FormLogin : Form
    {
        private TcpClient client;
        private NetworkStream stream;
        private static string totalBuffer;
        private static byte[] buffer = new byte[1024];
        private static bool connected = false;
        private List<string> users = new List<string>();

        private DataBaseManager manager;

        public FormLogin()
        {
            manager = new DataBaseManager();

            InitializeComponent();

        }

        private void btnReg_Click(object sender, EventArgs e)
        {
            string user = textBoxUserName.Text;
            string pass = textBoxPass.Text;

            if (manager.getUserInfoReg(user, pass))
            {
                MessageBox.Show($"{user} has been created!");
            }


        }

        async private void btnLogIn_Click(object sender, EventArgs e)
        {
            string user = textBoxUserName.Text;
            string pass = textBoxPass.Text;


            if (manager.getUserInfoLogIn(user, pass))
            {
                MessageBox.Show($"Welcome {user}");
                client = new TcpClient();
                await client.ConnectAsync("localhost", 15243);
                OnConnect();


                users.Add(user);

                FormChat chat = new FormChat(user, users, client);
                chat.Show();
                this.Hide();
                chat.AddOnlineUser(sender, e);
            }
            else
            {
                MessageBox.Show("This account does not exist");
            }
            //server.DataBaseManager.getUserInfoLogIn(user, pass)
        }

        private void OnConnect()
        {
            string user = textBoxUserName.Text;

            Console.WriteLine("Connected!");
            connected = true;
            stream = client.GetStream();
            stream.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(OnRead), null);
            //write($"{user} is connected\r\n");
            Console.WriteLine("ENDENDED");

        }

        private void OnRead(IAsyncResult ar)
        {
            int receivedBytes = stream.EndRead(ar);
            string receivedText = System.Text.Encoding.ASCII.GetString(buffer, 0, receivedBytes);
            totalBuffer += receivedText;


        }
        private void write(string data)
        {
            var dataAsBytes = System.Text.Encoding.ASCII.GetBytes(data);
            stream.Write(dataAsBytes, 0, dataAsBytes.Length);
            stream.Flush();
        }

    }
}
