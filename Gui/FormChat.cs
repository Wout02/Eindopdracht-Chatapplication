using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Gui
{
    public partial class FormChat : Form
    {
        private string user;
        private List<string> users;

        public FormChat(string user, List<string> users)
        {
            this.user = user;
            this.users = users;
            InitializeComponent();

            OnlineUsers.DataSource = users;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = textBox2.Text;
            string temp = user + ": " + message + "\r\n";
            if (message.Length > 0)
            { 
                textBox1.Text += temp;
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
    }
}
