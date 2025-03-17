using System;

namespace EmployeeManagementSystem
{
    partial class EmployeeLoginForm
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
            this.Full_name = new System.Windows.Forms.TextBox();
            this.EmployeePassword = new System.Windows.Forms.TextBox();
            this.ShowPassword = new System.Windows.Forms.RadioButton();
            this.LoginButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Full_name
            // 
            this.Full_name.Location = new System.Drawing.Point(412, 253);
            this.Full_name.Name = "Full_name";
            this.Full_name.Size = new System.Drawing.Size(266, 20);
            this.Full_name.TabIndex = 0;
            // 
            // EmployeePassword
            // 
            this.EmployeePassword.Location = new System.Drawing.Point(412, 311);
            this.EmployeePassword.Name = "EmployeePassword";
            this.EmployeePassword.Size = new System.Drawing.Size(266, 20);
            this.EmployeePassword.TabIndex = 1;
            // 
            // ShowPassword
            // 
            this.ShowPassword.AutoSize = true;
            this.ShowPassword.Location = new System.Drawing.Point(638, 358);
            this.ShowPassword.Name = "ShowPassword";
            this.ShowPassword.Size = new System.Drawing.Size(100, 17);
            this.ShowPassword.TabIndex = 2;
            this.ShowPassword.TabStop = true;
            this.ShowPassword.Text = "Show password";
            this.ShowPassword.UseVisualStyleBackColor = true;
            this.ShowPassword.CheckedChanged += new System.EventHandler(this.ShowPassword_CheckedChanged);
            // 
            // LoginButton
            // 
            this.LoginButton.Location = new System.Drawing.Point(504, 367);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(75, 23);
            this.LoginButton.TabIndex = 3;
            this.LoginButton.Text = "Đăng nhập";
            this.LoginButton.UseVisualStyleBackColor = true;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // EmployeeLoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoginButton);
            this.Controls.Add(this.ShowPassword);
            this.Controls.Add(this.EmployeePassword);
            this.Controls.Add(this.Full_name);
            this.Name = "EmployeeLoginForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

       

        #endregion

        private System.Windows.Forms.TextBox Full_name;
        private System.Windows.Forms.TextBox EmployeePassword;
        private System.Windows.Forms.RadioButton ShowPassword;
        private System.Windows.Forms.Button LoginButton;
    }
}