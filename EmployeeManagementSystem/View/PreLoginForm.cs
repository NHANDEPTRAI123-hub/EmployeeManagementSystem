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
    public partial class PreLoginForm : Form
    {
        public PreLoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.Hide();
            using (ManagerLoginForm loginForm = new ManagerLoginForm())
            {
                loginForm.ShowDialog();
            }
            this.Hide(); // Đóng luôn PreLoginForm sau khi login xong

        }

        private void button2_Click(object sender, EventArgs e)
        {

            this.Hide();
            using (EmployeeLoginForm loginForm2 = new EmployeeLoginForm())
            {
                loginForm2.ShowDialog();
            }
            this.Hide(); // Đóng luôn PreLoginForm sau khi login xong

        }


        private void PreLoginForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
