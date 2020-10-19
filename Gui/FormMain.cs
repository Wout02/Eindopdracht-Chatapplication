using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;

namespace Gui
{
    public partial class FormMain : Form
    {
        private string connectionString;
        private SqlConnection connection;

        public FormMain()
        {
            InitializeComponent();


            connectionString = ConfigurationManager.ConnectionStrings["UserInfo.Properties.Settings.UserInfoConnectionString"].ConnectionString;
            //MessageBox.Show(connectionString);

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string user = TboxUsername.Text.ToLower();
            string pass = TboxPassword.Text;

            string query = $"INSERT INTO UserInfo (username, password) VALUES(@username, @password);";

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", user);
                    cmd.Parameters.AddWithValue("@password", pass);
                    cmd.ExecuteNonQuery();
                }

            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = TboxUsername.Text.ToLower();
            string pass = TboxPassword.Text;

            string query = $"SELECT COUNT(*) FROM UserInfo WHERE username=@username AND password=@password";
            Console.WriteLine(query);

            using (connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", user);
                    cmd.Parameters.AddWithValue("@password", pass);
                    int result = (int)cmd.ExecuteScalar();

                    if (result > 0)
                    {
                        MessageBox.Show($"Welcome {user}");
                    }

                    if (result <= 0)
                    {
                        MessageBox.Show("This account does not exist");
                    }
                }

            }
        }

        private void populateUsers()
        {




        }

    }
}
