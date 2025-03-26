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
    public partial class Dashboard : UserControl
    {
        public Dashboard()
        {
            InitializeComponent();
            RefreshData();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayTE();
            displayAE();
            displayIE();
        }

        public void displayTE()
        {
            List<EmployeeData> employees = EmployeeData.LoadFromJson();
            int count = employees.Count; // Đếm số nhân viên hiện có

            dashboard_TE.Text = count.ToString();
        }


        public void displayAE()
        {
            List<Attendance> attendanceRecords = Attendance.LoadFromJson();
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            // Đếm số nhân viên đã CheckIn hôm nay
            int checkedInCount = attendanceRecords.Count(a => a.Action == "CheckIn" && a.Date == today);

            dashboard_AE.Text = checkedInCount.ToString();
        }

        public void displayIE()
        {
            int totalEmployees = int.Parse(dashboard_TE.Text); // Tổng số nhân viên
            int checkedInEmployees = int.Parse(dashboard_AE.Text); // Nhân viên đã CheckIn
            int notCheckedInEmployees = totalEmployees - checkedInEmployees;

            dashboard_IE.Text = notCheckedInEmployees.ToString();
        }



        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void Dashboard_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dashboard_TE_Click(object sender, EventArgs e)
        {

        }
    }
}

