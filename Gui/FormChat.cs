using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
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
            }
        }
    }
}
