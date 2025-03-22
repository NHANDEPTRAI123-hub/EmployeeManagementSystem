using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class ManagerLoginForm : Form
    {
        public ManagerLoginForm()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void login_signupBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            using (RegisterForm registerForm = new RegisterForm())
            {
                registerForm.ShowDialog();
            }
            this.Show(); // Hiện lại LoginForm sau khi đóng RegisterForm
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            if (login_showPass.Checked)
            {
                login_password.PasswordChar = '\0';
            }
            else
            {
                login_password.PasswordChar = '*';
            }
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (login_username.Text.Trim() == "" || login_password.Text.Trim() == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Manager> ManagerUserList = Manager.LoadFromJson(); // Đọc danh sách người dùng từ JSON
            Users user = null;

            for (int i = 0; i < ManagerUserList.Count; i++)
            {
                if (ManagerUserList[i].Username.Equals(login_username.Text.Trim(), StringComparison.OrdinalIgnoreCase) &&
                    ManagerUserList[i].Password == login_password.Text.Trim())
                {
                    user = ManagerUserList[i];
                    break;
                }
            }

            if (user != null)
            {
                MessageBox.Show("Login successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                using (MainForm mForm = new MainForm())
                {
                    mForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Incorrect Username/Password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void login_username_TextChanged(object sender, EventArgs e)
        {
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
