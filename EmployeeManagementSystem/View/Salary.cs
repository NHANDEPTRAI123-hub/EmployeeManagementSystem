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
    public partial class Salary : UserControl
    {

        public Salary()
        {
            InitializeComponent();

            displayEmployees();
            disableFields();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayEmployees(); // Hiển thị danh sách mới từ JSON
            disableFields();
        }


        public void disableFields()
        {
            salary_employeeID.Enabled = false;
            salary_name.Enabled = false;
            salary_position.Enabled = false;
        }

        public void displayEmployees()
        {
            List<SalaryData> listData = SalaryData.LoadFromJson(); // Đọc dữ liệu từ JSON
            dataGridView1.DataSource = listData;
        }


        private void salary_updateBtn_Click(object sender, EventArgs e)
        {
            if (salary_employeeID.Text == ""
                || salary_name.Text == ""
                || salary_position.Text == ""
                || salary_salary.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                DialogResult check = MessageBox.Show("Are you sure you want to UPDATE Employee ID: "
                    + salary_employeeID.Text.Trim() + "?", "Confirmation Message",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (check == DialogResult.Yes)
                {
                    try
                    {
                        // Đọc dữ liệu từ JSON
                        List<SalaryData> employees = SalaryData.LoadFromJson();
                        string empID = salary_employeeID.Text.Trim();
                        SalaryData employee = employees.Find(emp => emp.EmployeeID == empID);

                        if (employee != null)
                        {
                            employee.Salary = int.Parse(salary_salary.Text.Trim()); // Cập nhật lương
                            SalaryData.SaveToJson(employees); // Lưu lại dữ liệu vào JSON

                            displayEmployees(); // Hiển thị danh sách cập nhật

                            MessageBox.Show("Updated successfully!", "Information Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            clearFields();
                        }
                        else
                        {
                            MessageBox.Show("Employee ID not found!", "Error Message",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex, "Error Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Cancelled", "Information Message",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        public void clearFields()
        {
            salary_employeeID.Text = "";
            salary_name.Text = "";
            salary_position.Text = "";
            salary_salary.Text = "";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                salary_employeeID.Text = row.Cells["EmployeeID"].Value.ToString();
                salary_name.Text = row.Cells["Name"].Value.ToString();
                salary_position.Text = row.Cells["Position"].Value.ToString();
                salary_salary.Text = row.Cells["Salary"].Value.ToString();
            }
        }


        private void salary_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void salary_salary_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Salary_Load(object sender, EventArgs e)
        {

        }
    }
}
