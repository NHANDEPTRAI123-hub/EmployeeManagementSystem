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
            var employees = EmployeeData.LoadFromJson();
            int count = 0;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Status != "Deleted")
                {
                    count++;
                }
            }

            dashboard_TE.Text = count.ToString();
        }

        public void displayAE()
        {
            var employees = EmployeeData.LoadFromJson();
            int count = 0;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Status == "Active")
                {
                    count++;
                }
            }

            dashboard_AE.Text = count.ToString();
        }

        public void displayIE()
        {
            var employees = EmployeeData.LoadFromJson();
            int count = 0;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].Status == "Inactive")
                {
                    count++;
                }
            }

            dashboard_IE.Text = count.ToString();
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
    }
}

