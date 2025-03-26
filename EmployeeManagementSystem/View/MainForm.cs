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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to logout?",
                "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
            PreLoginForm preLoginForm = new PreLoginForm();
            preLoginForm.Show();
            this.Close(); // Đóng form hiện tại hoàn toàn để tránh hiện tượng tồn tại form cũ
            }
        }

        private void dashboard_btn_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = true;
            addEmployee1.Visible = false;
            salary1.Visible = false;
            attendanceManager1.Visible = false;



            if (dashboard1 is Dashboard)
            {
                Dashboard dashForm = (Dashboard)dashboard1;
                dashForm.RefreshData();
            }
        }

        private void addEmployee_btn_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            addEmployee1.Visible = true;
            salary1.Visible = false;
            attendanceManager1.Visible = false;



            if (addEmployee1 is AddEmployee)
            {
                AddEmployee addEmForm = (AddEmployee)addEmployee1;
                addEmForm.RefreshData();
            }
        }

        private void salary_btn_Click(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            addEmployee1.Visible = false;
            salary1.Visible = true;
            attendanceManager1.Visible = false;

            if (salary1 is Salary)
            {
                Salary salaryForm = (Salary)salary1;
                salaryForm.RefreshData();
            }
        }

       
        private void dashboard1_Load(object sender, EventArgs e)
        {

        }

      

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnManageAttendance_Click_1(object sender, EventArgs e)
        {
            dashboard1.Visible = false;
            addEmployee1.Visible = false;
            salary1.Visible = false;
            attendanceManager1.Visible = true;

        }

        private void dashboard1_Load_1(object sender, EventArgs e)
        {

        }
    }
}
