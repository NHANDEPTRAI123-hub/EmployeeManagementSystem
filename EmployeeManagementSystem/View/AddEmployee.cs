using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace EmployeeManagementSystem
{

    public partial class AddEmployee : UserControl
    {
        private List<EmployeeData> employees = new List<EmployeeData>(); // Danh sách nhân viên

        public AddEmployee()
        {
            InitializeComponent();
            LoadEmployeeData();
            DisplayEmployeeData();
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }
            DisplayEmployeeData();
        }

        // Load danh sách nhân viên từ file JSON
        private void LoadEmployeeData()
        {
            List<EmployeeData> loadedData = EmployeeData.LoadFromJson();
            if (loadedData != null)
            {
                employees = loadedData;
            }
            else
            {
                employees = new List<EmployeeData>();
            }
            DisplayEmployeeData();
        }

        // Hiển thị dữ liệu nhân viên lên DataGridView
        public void DisplayEmployeeData()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = employees;
        }

        // Hàm lấy mức lương dựa trên chức vụ
        private int GetSalaryByPosition(string position)
        {
            if (position == "Business Management") return 30000000;
            if (position == "Front-End Developer") return 18000000;
            if (position == "Back-End Developer") return 19000000;
            if (position == "Data Administrator") return 20000000;
            if (position == "UI/UX Design") return 22500000;
            return 3000000; // (Thử việc) Mặc định nếu không có vị trí cụ thể
        }

        // Sự kiện khi nhấn nút thêm nhân viên
        private void addEmployee_addBtn_Click(object sender, EventArgs e)
        {
            string empID = addEmployee_id.Text.Trim();
            string position = addEmployee_position.Text.Trim();

            EmployeeData newEmployee = new EmployeeData();
            newEmployee.EmployeeID = empID;
            newEmployee.Name = addEmployee_fullName.Text.Trim();
            newEmployee.Gender = addEmployee_gender.Text.Trim();
            newEmployee.Contact = addEmployee_phoneNum.Text.Trim();
            newEmployee.Position = position;
            newEmployee.Password = addEmployee_password.Text.Trim();
            newEmployee.Image = addEmployee_picture.ImageLocation;

            if (EmployeeData.AddEmployee(newEmployee))
            {
                // Cập nhật danh sách lương nhân viên
                List<SalaryData> salaryList = SalaryData.LoadFromJson();
                if (salaryList == null)
                {
                    salaryList = new List<SalaryData>();
                }
                SalaryData salaryData = new SalaryData();
                salaryData.EmployeeID = empID;
                salaryData.Salary = GetSalaryByPosition(position);
                salaryList.Add(salaryData);
                SalaryData.SaveToJson(salaryList);

                LoadEmployeeData();
                MessageBox.Show("Added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Employee ID already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi nhấn nút nhập ảnh
        private void addEmployee_importBtn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.Filter = "Image Files (*.jpg; *.png)|*.jpg;*.png";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    addEmployee_picture.ImageLocation = dialog.FileName;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi nhấn vào một dòng trong DataGridView
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];

            addEmployee_id.Text = row.Cells[1].Value != null ? row.Cells[1].Value.ToString() : "";
            addEmployee_fullName.Text = row.Cells[2].Value != null ? row.Cells[2].Value.ToString() : "";
            addEmployee_gender.SelectedIndex = addEmployee_gender.FindString(row.Cells[3].Value != null ? row.Cells[3].Value.ToString() : "");
            addEmployee_phoneNum.Text = row.Cells[4].Value != null ? row.Cells[4].Value.ToString() : "";
            addEmployee_position.SelectedIndex = addEmployee_position.FindString(row.Cells[5].Value != null ? row.Cells[5].Value.ToString() : "");
            addEmployee_password.Text = row.Cells["Password"].Value != null ? row.Cells["Password"].Value.ToString() : "";

            LoadEmployeeData();
        }

        // Hàm xóa dữ liệu trong form
        public void ClearFields()
        {
            addEmployee_id.Text = "";
            addEmployee_fullName.Text = "";
            addEmployee_gender.SelectedIndex = -1;
            addEmployee_phoneNum.Text = "";
            addEmployee_position.SelectedIndex = -1;
            addEmployee_password.Text = "";
            addEmployee_picture.Image = null;
        }

        // Sự kiện khi nhấn nút cập nhật nhân viên
        private void addEmployee_updateBtn_Click(object sender, EventArgs e)
        {
            string empID = addEmployee_id.Text.Trim();
            string position = addEmployee_position.Text.Trim();

            EmployeeData updatedEmployee = new EmployeeData();
            updatedEmployee.EmployeeID = empID;
            updatedEmployee.Name = addEmployee_fullName.Text.Trim();
            updatedEmployee.Gender = addEmployee_gender.Text.Trim();
            updatedEmployee.Contact = addEmployee_phoneNum.Text.Trim();
            updatedEmployee.Position = position;
            updatedEmployee.Password = addEmployee_password.Text.Trim();
            updatedEmployee.Image = addEmployee_picture.ImageLocation;

            if (EmployeeData.UpdateEmployee(updatedEmployee))
            {
                // Cập nhật mức lương dựa trên vị trí
                List<SalaryData> salaryList = SalaryData.LoadFromJson();
                for (int i = 0; i < salaryList.Count; i++)
                {
                    if (salaryList[i].EmployeeID == empID)
                    {
                        salaryList[i].Salary = GetSalaryByPosition(position);
                        break;
                    }
                }
                SalaryData.SaveToJson(salaryList);

                LoadEmployeeData();

                MessageBox.Show("Updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
            }
            else
            {
                MessageBox.Show("Employee not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Sự kiện khi nhấn nút xóa dữ liệu nhập
        private void addEmployee_clearBtn_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        // Sự kiện khi nhấn nút xóa nhân viên
        private void addEmployee_deleteBtn_Click(object sender, EventArgs e)
        {
            string empID = addEmployee_id.Text.Trim();

            if (EmployeeData.DeleteEmployee(empID))
            {
                LoadEmployeeData();
                MessageBox.Show("Deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ClearFields();
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