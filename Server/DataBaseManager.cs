using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;

namespace Server
{
    class DataBaseManager
    {

        private SqlConnection connection;
        private string connectionString;

        public DataBaseManager() 
        {
            connectionString = ConfigurationManager.ConnectionStrings["UserInfo.Properties.Settings.UserInfoConnectionString"].ConnectionString;
        }

        public bool getUserInfoReg(string user, string pass)
        {

            string query = "INSERT INTO UserInfo (username, password) VALUES (@username, @password)";

            using (connection = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", user.ToLower());
                    cmd.Parameters.AddWithValue("@password", pass);
                    //registreren
                    cmd.ExecuteScalar();
                    
                    return true;
                }
            }

        }

        public bool getUserInfoLogIn(string user, string pass)
        {

            string query = "SELECT COUNT(*) FROM UserInfo WHERE username=@username AND password=@password";

            using (connection = new SqlConnection(connectionString))
            {

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@username", user.ToLower());
                    cmd.Parameters.AddWithValue("@password", pass);
                    
                    int result = (int)cmd.ExecuteScalar();

                    if (result > 0 )
                    {
                        //gebruiker gevonden
                        return true;
                    }
                    else
                    {
                        //gebruiker niet gevonden
                        return false;
                    }

                }


            }

        }

    }
}
