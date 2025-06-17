using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ScanSQL
{
    public class ConnectionService
    {
        public SqlConnection SqlConnection { get; private set; }

        public bool TryConnect(string connectionString)
        {
            try
            {
                SqlConnection = new SqlConnection(connectionString);
                SqlConnection.Open();
            }

            catch (Exception)
            {
                MessageBox.Show("Failed to connect. \r\n" +
                                "Please check your credentials and try again.",
                                "Connection Failed",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        public void CloseConnection()
        {
            if (SqlConnection != null && SqlConnection.State == ConnectionState.Open)
            {
                SqlConnection.Close();
            }
        }
    }
}
