using EmployeeManagementSystem.Controller;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class AttendanceForm : Form
    {
        private readonly EmployeeData _currentEmployee;
        private AttendanceController _controller;

        public AttendanceForm(EmployeeData employee)
        {
            InitializeComponent();
            _currentEmployee = employee;
            _controller = new AttendanceController(this, employee);
        }

        public void SetController(AttendanceController controller)
        {
            _controller = controller;
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            _controller.LoadForm();
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            _controller.CheckIn();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            _controller.CheckOut();
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            _controller.Logout();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            _controller.Exit();
        }

        // Các phương thức để Controller gọi và cập nhật giao diện
        public void SetEmployeeInfo(string info)
        {
            lblEmployeeInfo.Text = info;
        }

        public void SetLastStatus(string status)
        {
            lblLastStatus.Text = status;
        }

        public void ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show(message, caption, buttons, icon);
        }

        public DialogResult ShowConfirmation(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            return MessageBox.Show(message, caption, buttons, icon);
        }

        public void CloseForm()
        {
            this.Close();
        }
    }
}
