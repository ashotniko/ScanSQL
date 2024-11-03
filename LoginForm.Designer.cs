namespace ScanSQL
{
    partial class LoginForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Login = new System.Windows.Forms.Label();
            this.Password = new System.Windows.Forms.Label();
            this.txtLogin = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.labelDBName = new System.Windows.Forms.Label();
            this.txtServerName = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.rememberMeChkBox = new System.Windows.Forms.CheckBox();
            this.showPasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.ServerName = new System.Windows.Forms.Label();
            this.labelDBNamee = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Login
            // 
            this.Login.AutoSize = true;
            this.Login.Location = new System.Drawing.Point(178, 198);
            this.Login.Name = "Login";
            this.Login.Size = new System.Drawing.Size(33, 13);
            this.Login.TabIndex = 0;
            this.Login.Text = "Login";
            this.Login.Click += new System.EventHandler(this.labelUserName_Click);
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Location = new System.Drawing.Point(178, 229);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(53, 13);
            this.Password.TabIndex = 1;
            this.Password.Text = "Password";
            this.Password.Click += new System.EventHandler(this.labelPassword_Click);
            // 
            // txtLogin
            // 
            this.txtLogin.Location = new System.Drawing.Point(253, 195);
            this.txtLogin.Name = "txtLogin";
            this.txtLogin.Size = new System.Drawing.Size(194, 20);
            this.txtLogin.TabIndex = 2;
            this.txtLogin.TextChanged += new System.EventHandler(this.txtLogin_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(253, 226);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(194, 20);
            this.txtPassword.TabIndex = 3;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // labelDBName
            // 
            this.labelDBName.AutoSize = true;
            this.labelDBName.BackColor = System.Drawing.SystemColors.Control;
            this.labelDBName.Location = new System.Drawing.Point(178, 137);
            this.labelDBName.Name = "labelDBName";
            this.labelDBName.Size = new System.Drawing.Size(0, 13);
            this.labelDBName.TabIndex = 5;
            this.labelDBName.Click += new System.EventHandler(this.labelDBName_Click);
            // 
            // txtServerName
            // 
            this.txtServerName.Location = new System.Drawing.Point(253, 134);
            this.txtServerName.Name = "txtServerName";
            this.txtServerName.Size = new System.Drawing.Size(194, 20);
            this.txtServerName.TabIndex = 7;
            this.txtServerName.TextChanged += new System.EventHandler(this.txtServerName_TextChanged);
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(369, 268);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(78, 25);
            this.loginButton.TabIndex = 8;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // rememberMeChkBox
            // 
            this.rememberMeChkBox.AutoSize = true;
            this.rememberMeChkBox.Location = new System.Drawing.Point(369, 311);
            this.rememberMeChkBox.Name = "rememberMeChkBox";
            this.rememberMeChkBox.Size = new System.Drawing.Size(94, 17);
            this.rememberMeChkBox.TabIndex = 9;
            this.rememberMeChkBox.Text = "Remember me";
            this.rememberMeChkBox.UseVisualStyleBackColor = true;
            this.rememberMeChkBox.CheckedChanged += new System.EventHandler(this.rememberPasswordCheckBox_CheckedChanged);
            // 
            // showPasswordCheckBox
            // 
            this.showPasswordCheckBox.AutoSize = true;
            this.showPasswordCheckBox.Location = new System.Drawing.Point(475, 229);
            this.showPasswordCheckBox.Name = "showPasswordCheckBox";
            this.showPasswordCheckBox.Size = new System.Drawing.Size(101, 17);
            this.showPasswordCheckBox.TabIndex = 10;
            this.showPasswordCheckBox.Text = "Show password";
            this.showPasswordCheckBox.UseVisualStyleBackColor = true;
            this.showPasswordCheckBox.CheckedChanged += new System.EventHandler(this.chkShowPassword_CheckedChanged);
            // 
            // ServerName
            // 
            this.ServerName.AutoSize = true;
            this.ServerName.Location = new System.Drawing.Point(178, 137);
            this.ServerName.Name = "ServerName";
            this.ServerName.Size = new System.Drawing.Size(69, 13);
            this.ServerName.TabIndex = 11;
            this.ServerName.Text = "Server Name";
            this.ServerName.Click += new System.EventHandler(this.ServerName_Click);
            // 
            // labelDBNamee
            // 
            this.labelDBNamee.AutoSize = true;
            this.labelDBNamee.Location = new System.Drawing.Point(178, 168);
            this.labelDBNamee.Name = "labelDBNamee";
            this.labelDBNamee.Size = new System.Drawing.Size(50, 13);
            this.labelDBNamee.TabIndex = 12;
            this.labelDBNamee.Text = "DBName";
            this.labelDBNamee.Click += new System.EventHandler(this.labelDBNamee_Click);
            // 
            // txtDBName
            // 
            this.txtDBName.Location = new System.Drawing.Point(253, 165);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(194, 20);
            this.txtDBName.TabIndex = 13;
            this.txtDBName.TextChanged += new System.EventHandler(this.txtDBName_TextChanged);
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.labelDBNamee);
            this.Controls.Add(this.ServerName);
            this.Controls.Add(this.showPasswordCheckBox);
            this.Controls.Add(this.rememberMeChkBox);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.txtServerName);
            this.Controls.Add(this.labelDBName);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtLogin);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.Login);
            this.Name = "LoginForm";
            this.Text = "LoginForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.LoginForm_FormClosing);
            this.Load += new System.EventHandler(this.LoginForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Login;
        private System.Windows.Forms.Label Password;
        private System.Windows.Forms.TextBox txtLogin;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label labelDBName;
        private System.Windows.Forms.TextBox txtServerName;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.CheckBox rememberMeChkBox;
        private System.Windows.Forms.CheckBox showPasswordCheckBox;
        private System.Windows.Forms.Label ServerName;
        private System.Windows.Forms.Label labelDBNamee;
        private System.Windows.Forms.TextBox txtDBName;
    }
}