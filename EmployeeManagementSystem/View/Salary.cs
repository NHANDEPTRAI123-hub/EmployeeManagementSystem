using EmployeeManagementSystem.Controller;
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
            List<EmployeeData> employees = EmployeeData.LoadFromJson();
            List<SalaryData> salaries = SalaryData.LoadFromJson();
            List<SalaryData> mergedData = new List<SalaryData>();

            foreach (EmployeeData emp in employees)
            {
                // Gọi tính toán lại lương trước khi hiển thị
                SalaryManager salaryManager = new SalaryManager();
                salaryManager.CalculateSalary(emp.EmployeeID); // 🔥 Dòng quan trọng cần thêm

                SalaryData salaryInfo = SalaryData.GetSalaryByEmployeeID(emp.EmployeeID);

                SalaryData newSalaryData = new SalaryData
                {
                    EmployeeID = emp.EmployeeID,
                    Name = emp.Name,
                    Position = emp.Position,
                    Salary = salaryInfo?.Salary ?? 0,
                    Bonus = salaryInfo?.Bonus ?? 0,
                    Deduction = salaryInfo?.Deduction ?? 0, // Đảm bảo hiển thị giá trị Deduction
                    CurrentSalary = salaryInfo?.CurrentSalary ?? 0
                };

                mergedData.Add(newSalaryData);
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = mergedData;
        }



        private void salary_updateBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(salary_employeeID.Text) ||
                string.IsNullOrWhiteSpace(reward_reward.Text))
            {
                MessageBox.Show("Please fill in all required fields!", "Error Message",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(reward_reward.Text.Trim(), out decimal rewardAmount))
            {
                MessageBox.Show("Invalid reward amount!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult check = MessageBox.Show("Are you sure you want to UPDATE Employee ID: "
                + salary_employeeID.Text.Trim() + "?", "Confirmation Message",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                try
                {
                    string empID = salary_employeeID.Text.Trim();
                    List<SalaryData> salaries = SalaryData.LoadFromJson();
                    SalaryData salaryEntry = null;

                    // Tìm nhân viên trong danh sách lương
                    for (int i = 0; i < salaries.Count; i++)
                    {
                        if (salaries[i].EmployeeID == empID)
                        {
                            salaryEntry = salaries[i];
                            break;
                        }
                    }

                    if (salaryEntry != null)
                    {
                        salaryEntry.Bonus += rewardAmount;
                        salaryEntry.UpdateCurrentSalary();
                        SalaryData.SaveToJson(salaries);
                        displayEmployees();

                        MessageBox.Show("Updated successfully!", "Information Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clearFields();
                    }
                    else
                    {
                        MessageBox.Show("Employee not found!", "Error Message",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error Message",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                salary_employeeID.Text = row.Cells["EmployeeID"].Value?.ToString();
                salary_name.Text = row.Cells["Name"].Value?.ToString();
                salary_position.Text = row.Cells["Position"].Value?.ToString();

                // Để trống ô Reward để nhập số tiền thưởng mới
                reward_reward.Text = "";
            }
        }



        public void clearFields()
        {
            salary_employeeID.Text = "";
            salary_name.Text = "";
            salary_position.Text = "";
            reward_reward.Text = "";
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
