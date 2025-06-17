using ScanSQL.Enums;
using System;
using System.Windows.Forms;

namespace ScanSQL
{
    public partial class LoginForm : Form
    {
        bool _firstTime = true;
        bool _clickOnLogin = false;
        public Login _login;
        public ConnectionService _connectionService;

        public string ConnectionString { get; private set; }
        public LoginForm(ConnectionService connectionService)
        {
            _connectionService = connectionService;
            _login = new Login(this, connectionService);
            InitializeComponent();
            UpdateLoginButtonState();
        }

        private void labelUserName_Click(object sender, EventArgs e)
        {

        }

        private void labelServerName_Click(object sender, EventArgs e)
        {

        }

        private void labelDBName_Click(object sender, EventArgs e)
        {

        }

        private void chkShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = !showPasswordCheckBox.Checked;
        }

        private void labelPassword_Click(object sender, EventArgs e)
        {

        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            //If remeber me clicks save creditionals
            if (rememberMeChkBox.Checked)
            {
                Properties.Settings.Default.ServerName = txtServerName.Text;
                Properties.Settings.Default.DBName = txtDBName.Text;
                Properties.Settings.Default.Login = txtLogin.Text;
                Properties.Settings.Default.Password = txtPassword.Text;
                Properties.Settings.Default.RememberMe = true;
                Properties.Settings.Default.ProductMode = cmbProductMode.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.ServerName = string.Empty;
                Properties.Settings.Default.DBName = string.Empty;
                Properties.Settings.Default.Login = string.Empty;
                Properties.Settings.Default.Password = string.Empty;
                Properties.Settings.Default.RememberMe = false;
                Properties.Settings.Default.ProductMode = ProducteMode.Enterprise.ToString();
                Properties.Settings.Default.Save();
            }

            //If connected close login form and go to main form
            ConnectionString = _login.CreateConnectionString(txtServerName.Text, txtDBName.Text, txtLogin.Text, txtPassword.Text);
            if (_login.TryConnectToDatabase(ConnectionString))
            {
                DialogResult = DialogResult.OK;
                _clickOnLogin = true;
                Close();
            }
            ;
        }

        private void txtServerName_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void txtLogin_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (_firstTime && !showPasswordCheckBox.Checked)
            {
                txtPassword.UseSystemPasswordChar = true;
                _firstTime = false;
            }
            UpdateLoginButtonState();
        }

        private void rememberPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ShowConsoleOutput(object sender, EventArgs e)
        {

        }

        private void ServerName_Click(object sender, EventArgs e)
        {

        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            cmbProductMode.DataSource = Enum.GetValues(typeof(ProducteMode));
            cmbProductMode.SelectedItem = ProducteMode.Enterprise; // Default selection
            if (Properties.Settings.Default.RememberMe)
            {
                txtServerName.Text = Properties.Settings.Default.ServerName;
                txtDBName.Text = Properties.Settings.Default.DBName;
                txtLogin.Text = Properties.Settings.Default.Login;
                txtPassword.Text = Properties.Settings.Default.Password;
                rememberMeChkBox.Checked = Properties.Settings.Default.RememberMe;

                if (Enum.TryParse(Properties.Settings.Default.ProductMode, out ProducteMode savedMode))
                {
                    cmbProductMode.SelectedItem = savedMode;
                }
            }

        }

        private void txtDBName_TextChanged(object sender, EventArgs e)
        {
            UpdateLoginButtonState();
        }

        private void labelDBNamee_Click(object sender, EventArgs e)
        {

        }
        private void UpdateLoginButtonState()
        {
            // Check if all fields are filled out
            loginButton.Enabled = !string.IsNullOrEmpty(txtServerName.Text) &&
                                  !string.IsNullOrEmpty(txtDBName.Text) &&
                                  !string.IsNullOrEmpty(txtLogin.Text) &&
                                  !string.IsNullOrEmpty(txtPassword.Text);
        }

        private void LoginForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_clickOnLogin)
            {
                // Display a confirmation dialog before closing
                var result = MessageBox.Show("Are you sure you want to close the application?",
                                             "Confirm Exit",
                                             MessageBoxButtons.YesNo,
                                             MessageBoxIcon.Question);

                // If the user selects 'No', cancel the close action
                if (result == DialogResult.No)
                {
                    e.Cancel = true;  // Prevents the form from closing
                }
            }

        }
    }
}
