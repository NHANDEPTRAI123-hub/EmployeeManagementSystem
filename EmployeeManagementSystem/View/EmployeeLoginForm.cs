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
    public partial class EmployeeLoginForm : Form
    {
        public EmployeeLoginForm()
        {
            InitializeComponent();
            // Thiết lập mặc định cho Password TextBox
            EmployeePassword.UseSystemPasswordChar = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Full_name.Focus(); // Đặt con trỏ vào textbox Full_Name khi form load
        }

        private void ShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            // Xử lý hiển thị/ẩn mật khẩu
            EmployeePassword.UseSystemPasswordChar = !ShowPassword.Checked;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            if (!ValidateInput()) return;

            try
            {
                // Lấy danh sách nhân viên từ JSON
                List<EmployeeData> employees = EmployeeData.LoadFromJson();

                // Lấy thông tin từ TextBox
                string inputFullName = Full_name.Text.Trim();
                string inputPassword = EmployeePassword.Text;

                EmployeeData employee = null;

                // Kiểm tra thông tin đăng nhập
                for (int i = 0; i < employees.Count; i++)
                {
                    if (employees[i].Name.Equals(inputFullName, StringComparison.OrdinalIgnoreCase) &&
                        employees[i].Password == inputPassword) 
                    {
                        employee = employees[i];
                        break;
                    }
                }

                if (employee != null)
                {
                    MessageBox.Show("Đăng nhập thành công! Chào " + employee.Name,
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.Hide();
                    using (AttendanceForm attendanceForm = new AttendanceForm(employee))
                    {
                        attendanceForm.ShowDialog();
                    }
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên hoặc mật khẩu không đúng!",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    EmployeePassword.Clear();
                    Full_name.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Validation input
        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(Full_name.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Full_name.Focus();
                return false;
            }
            if (string.IsNullOrWhiteSpace(EmployeePassword.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                EmployeePassword.Focus();
                return false;
            }
            return true;
        }

        // Xử lý phím Enter
        private void EmployeePassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoginButton_Click(sender, e);
            }
        }
    }

}
