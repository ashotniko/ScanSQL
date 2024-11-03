using System;
using System.Windows.Forms;

namespace ScanSQL
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ConnectionService connectionService = new ConnectionService();

            using (LoginForm login = new LoginForm(connectionService))
            {
                DialogResult result = login.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Application.Run(new MainForm(login, connectionService));
                }
            }
        }
    }
}
