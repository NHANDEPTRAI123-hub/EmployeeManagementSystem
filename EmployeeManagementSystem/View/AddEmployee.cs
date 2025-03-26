using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace EmployeeManagementSystem
{
    public partial class AddEmployee : UserControl
    {
        private List<EmployeeData> employees = new List<EmployeeData>(); // Danh sách nhân viên

        public AddEmployee()
        {
            InitializeComponent();
            LoadEmployeeData(); // Nạp dữ liệu vào danh sách
            displayEmployeeData();  // Hiển thị danh sách lên DataGridView
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }
            displayEmployeeData();
        }
        // Tải dữ liệu từ JSON vào danh sách employees và hiển thị trên DataGridView
        private void LoadEmployeeData()
        {
            employees = EmployeeData.LoadFromJson() ?? new List<EmployeeData>(); // Tránh lỗi nếu dữ liệu rỗng
            displayEmployeeData();
        }

        public void displayEmployeeData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = employees;
        }

        private int GetSalaryByPosition(string position)
        {
            switch (position)
            {
                case "Business Management": return 30000000; // 30.000.000 VND/tháng
                case "Front-End Developer": return 18000000; // 18.000.000 VND/tháng
                case "Back-End Developer": return 19000000; // 19.000.000 VND/tháng
                case "Data Administrator": return 20000000; // 20.000.000 VND/tháng
                case "UI/UX Design": return 22500000; // 22.500.000 VND/tháng
                default: return 15000000; // Mặc định nếu không có vị trí cụ thể
            }
        }


        private void addEmployee_addBtn_Click(object sender, EventArgs e)
        {
            string empID = addEmployee_id.Text.Trim();
            string position = addEmployee_position.Text.Trim();

            EmployeeData newEmployee = new EmployeeData
            {
                EmployeeID = empID,
                Name = addEmployee_fullName.Text.Trim(),
                Gender = addEmployee_gender.Text.Trim(),
                Contact = addEmployee_phoneNum.Text.Trim(),
                Position = position,
                Password = addEmployee_password.Text.Trim(),
                Image = addEmployee_picture.ImageLocation
            };

            if (EmployeeData.AddEmployee(newEmployee))
            {
                // Tạo dữ liệu lương dựa trên Position
                List<SalaryData> salaryList = SalaryData.LoadFromJson();
                salaryList.Add(new SalaryData { EmployeeID = empID, Salary = GetSalaryByPosition(position) });
                SalaryData.SaveToJson(salaryList);

                LoadEmployeeData();
                MessageBox.Show("Added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields();
            }
            else
            {
                MessageBox.Show("Employee ID already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


            private void addEmployee_importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";
                string imagePath = "";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    imagePath = dialog.FileName;
                    addEmployee_picture.ImageLocation = imagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
                if (e.RowIndex < 0) return; // Tránh lỗi khi click vào header

                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

                // Gán dữ liệu lên các ô nhập liệu
                addEmployee_id.Text = row.Cells[1].Value?.ToString() ?? "";
                addEmployee_fullName.Text = row.Cells[2].Value?.ToString() ?? "";
                addEmployee_gender.SelectedIndex = addEmployee_gender.FindString(row.Cells[3].Value?.ToString());
                addEmployee_phoneNum.Text = row.Cells[4].Value?.ToString() ?? "";
                addEmployee_position.SelectedIndex = addEmployee_position.FindString(row.Cells[5].Value?.ToString());
                addEmployee_password.Text = row.Cells["Password"].Value?.ToString() ?? "";


            LoadEmployeeData();
            dataGridView1.DataSource = null; // Xóa dữ liệu cũ
            dataGridView1.DataSource = EmployeeData.LoadFromJson(); // Load lại danh sách mới
        }

        public void clearFields()
        {
            addEmployee_id.Text = "";
            addEmployee_fullName.Text = "";
            addEmployee_gender.SelectedIndex = -1;
            addEmployee_phoneNum.Text = "";
            addEmployee_position.SelectedIndex = -1;
            addEmployee_password.Text = "";
            addEmployee_picture.Image = null;
        }

        private void addEmployee_updateBtn_Click(object sender, EventArgs e)
        {
            string empID = addEmployee_id.Text.Trim();
            string position = addEmployee_position.Text.Trim();

            if (EmployeeData.UpdateEmployee(new EmployeeData
            {
                EmployeeID = empID,
                Name = addEmployee_fullName.Text.Trim(),
                Gender = addEmployee_gender.Text.Trim(),
                Contact = addEmployee_phoneNum.Text.Trim(),
                Position = position,
                Password = addEmployee_password.Text.Trim(),
                Image = addEmployee_picture.ImageLocation
            }))
            {
                // Cập nhật lương khi thay đổi Position
                List<SalaryData> salaryList = SalaryData.LoadFromJson();
                SalaryData employeeSalary = salaryList.Find(emp => emp.EmployeeID == empID);
                if (employeeSalary != null)
                {
                    employeeSalary.Salary = GetSalaryByPosition(position);
                    SalaryData.SaveToJson(salaryList);
                }

                displayEmployeeData();
                MessageBox.Show("Updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields();
            }
            else
            {
                MessageBox.Show("Employee not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void addEmployee_clearBtn_Click(object sender, EventArgs e)
        {
            clearFields();
        }

        private void addEmployee_deleteBtn_Click(object sender, EventArgs e)
        {
            if (EmployeeData.DeleteEmployee(addEmployee_id.Text.Trim()))
            {
                LoadEmployeeData(); // Load lại dữ liệu từ JSON
                MessageBox.Show("Deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields();
            }
            else
            {
                MessageBox.Show("Employee not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


       

        private void panel3_Paint(object sender, PaintEventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
        }

        private void AddEmployee_Load(object sender, EventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
        }

        private void label7_Click(object sender, EventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
        }

        private void addEmployee_position_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

}