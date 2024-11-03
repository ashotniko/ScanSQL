namespace ScanSQL
{
    partial class MainForm
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
            this.Start = new System.Windows.Forms.Button();
            this.Output = new System.Windows.Forms.TextBox();
            this.Scan = new System.Windows.Forms.Button();
            this.Reset = new System.Windows.Forms.Button();
            this.previousButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.ClearOutputHistory = new System.Windows.Forms.Button();
            this.EmptyAuditLog = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Start
            // 
            this.Start.Location = new System.Drawing.Point(54, 43);
            this.Start.Name = "Start";
            this.Start.Size = new System.Drawing.Size(89, 55);
            this.Start.TabIndex = 0;
            this.Start.Text = "Start";
            this.Start.UseVisualStyleBackColor = true;
            this.Start.Click += new System.EventHandler(this.Start_Click);
            // 
            // Output
            // 
            this.Output.Location = new System.Drawing.Point(54, 380);
            this.Output.MaxLength = 1000000;
            this.Output.Multiline = true;
            this.Output.Name = "Output";
            this.Output.ReadOnly = true;
            this.Output.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.Output.Size = new System.Drawing.Size(987, 610);
            this.Output.TabIndex = 1;
            this.Output.TextChanged += new System.EventHandler(this.Output_TextChanged);
            // 
            // Scan
            // 
            this.Scan.Location = new System.Drawing.Point(54, 216);
            this.Scan.Name = "Scan";
            this.Scan.Size = new System.Drawing.Size(89, 55);
            this.Scan.TabIndex = 2;
            this.Scan.Text = "Scan";
            this.Scan.UseVisualStyleBackColor = true;
            this.Scan.Click += new System.EventHandler(this.Scan_Click);
            // 
            // Reset
            // 
            this.Reset.Location = new System.Drawing.Point(195, 43);
            this.Reset.Name = "Reset";
            this.Reset.Size = new System.Drawing.Size(89, 55);
            this.Reset.TabIndex = 3;
            this.Reset.Text = "Reset";
            this.Reset.UseVisualStyleBackColor = true;
            this.Reset.Click += new System.EventHandler(this.Reset_Click);
            // 
            // previousButton
            // 
            this.previousButton.Location = new System.Drawing.Point(195, 216);
            this.previousButton.Name = "previousButton";
            this.previousButton.Size = new System.Drawing.Size(89, 55);
            this.previousButton.TabIndex = 4;
            this.previousButton.Text = "Previous";
            this.previousButton.UseVisualStyleBackColor = true;
            this.previousButton.Click += new System.EventHandler(this.PreviousButton_Click);
            // 
            // nextButton
            // 
            this.nextButton.Location = new System.Drawing.Point(334, 216);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(89, 55);
            this.nextButton.TabIndex = 5;
            this.nextButton.Text = "Next";
            this.nextButton.UseVisualStyleBackColor = true;
            this.nextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // ClearOutputHistory
            // 
            this.ClearOutputHistory.Location = new System.Drawing.Point(489, 216);
            this.ClearOutputHistory.Name = "ClearOutputHistory";
            this.ClearOutputHistory.Size = new System.Drawing.Size(89, 55);
            this.ClearOutputHistory.TabIndex = 6;
            this.ClearOutputHistory.Text = "Clear History";
            this.ClearOutputHistory.UseVisualStyleBackColor = true;
            this.ClearOutputHistory.Click += new System.EventHandler(this.ClearOutputHistory_Click);
            // 
            // EmptyAuditLog
            // 
            this.EmptyAuditLog.Location = new System.Drawing.Point(334, 43);
            this.EmptyAuditLog.Name = "EmptyAuditLog";
            this.EmptyAuditLog.Size = new System.Drawing.Size(89, 55);
            this.EmptyAuditLog.TabIndex = 7;
            this.EmptyAuditLog.Text = "Empty AuditLog";
            this.EmptyAuditLog.UseVisualStyleBackColor = true;
            this.EmptyAuditLog.Click += new System.EventHandler(this.EmptyAuditLog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 749);
            this.Controls.Add(this.EmptyAuditLog);
            this.Controls.Add(this.ClearOutputHistory);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.previousButton);
            this.Controls.Add(this.Reset);
            this.Controls.Add(this.Scan);
            this.Controls.Add(this.Output);
            this.Controls.Add(this.Start);
            this.Name = "MainForm";
            this.Text = "Sql Change Scanner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Start;
        private System.Windows.Forms.TextBox Output;
        private System.Windows.Forms.Button Scan;
        private System.Windows.Forms.Button Reset;
        private System.Windows.Forms.Button previousButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button ClearOutputHistory;
        private System.Windows.Forms.Button EmptyAuditLog;
    }
}

