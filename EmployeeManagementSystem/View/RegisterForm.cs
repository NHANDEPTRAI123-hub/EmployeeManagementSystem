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
    public partial class RegisterForm : Form
    {

        public RegisterForm()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signup_loginBtn_Click(object sender, EventArgs e)
        {
            ManagerLoginForm loginForm = new ManagerLoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(signup_username.Text) || string.IsNullOrWhiteSpace(signup_password.Text))
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<Manager> ManagerusersList = Manager.LoadFromJson();

            // Kiểm tra xem tên đăng nhập đã tồn tại chưa (không dùng LINQ)
            bool isUsernameTaken = false;
            for (int i = 0; i < ManagerusersList.Count; i++)
            {
                if (ManagerusersList[i].Fullname.Equals(signup_username.Text.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    isUsernameTaken = true;
                    break;
                }
            }

            if (isUsernameTaken)
            {
                MessageBox.Show(signup_username.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Tạo ID mới (không dùng LINQ)
            int newId = 1;
            for (int i = 0; i < ManagerusersList.Count; i++)
            {
                if (ManagerusersList[i].ID >= newId)
                {
                    newId = ManagerusersList[i].ID + 1;
                }
            }

            // Tạo người dùng mới
            Manager newManager  = new Manager
            {
                ID = newId,
                Fullname = signup_username.Text.Trim(),
                Password = signup_password.Text.Trim(),
                DateRegister = DateTime.Now
            };

            ManagerusersList.Add(newManager);
            Manager.SaveToJson(ManagerusersList);

            MessageBox.Show("Registered successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ManagerLoginForm loginForm = new ManagerLoginForm();
            loginForm.Show();
            this.Hide();
        }

        private void signup_showPass_CheckedChanged(object sender, EventArgs e)
        {
            signup_password.PasswordChar = signup_showPass.Checked ? '\0' : '*';
        }

        private void signup_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
