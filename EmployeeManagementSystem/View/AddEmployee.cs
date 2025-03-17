using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace EmployeeManagementSystem
{
    public partial class AddEmployee : UserControl
    {
        private List<EmployeeData> employees = new List<EmployeeData>(); // Danh sách nhân viên

        public AddEmployee()
        {
            InitializeComponent();
            displayEmployeeData();
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

        public void displayEmployeeData()
        {
            EmployeeData ed = new EmployeeData();
            List<EmployeeData> listData = ed.employeeListData();
            dataGridView1.DataSource = listData;
        }

        private void addEmployee_addBtn_Click(object sender, EventArgs e)
        {
            EmployeeData newEmployee = new EmployeeData
            {
                EmployeeID = addEmployee_id.Text.Trim(),
                Name = addEmployee_fullName.Text.Trim(),
                Gender = addEmployee_gender.Text.Trim(),
                Contact = addEmployee_phoneNum.Text.Trim(),
                Position = addEmployee_position.Text.Trim(),
                Password = addEmployee_password.Text.Trim(),
                Image = addEmployee_picture.ImageLocation, // Lấy đường dẫn ảnh nếu có
                Salary = 0, // Giá trị mặc định, bạn có thể thêm TextBox để nhập
                Status = "Active" // Giá trị mặc định
            };

            if (EmployeeData.AddEmployee(newEmployee))
            {
                displayEmployeeData();
                MessageBox.Show("Added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields(); // Xóa các trường sau khi thêm thành công
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
            if (e.RowIndex != -1)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                addEmployee_id.Text = row.Cells[1].Value.ToString(); // EmployeeID
                addEmployee_fullName.Text = row.Cells[2].Value.ToString(); // Name
                addEmployee_gender.Text = row.Cells[3].Value.ToString(); // Gender
                addEmployee_phoneNum.Text = row.Cells[4].Value.ToString(); // Contact
                addEmployee_position.Text = row.Cells[5].Value.ToString(); // Position

                string imagePath = row.Cells[6].Value?.ToString(); // Image
                if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                {
                    addEmployee_picture.Image = Image.FromFile(imagePath);
                }
                else
                {
                    addEmployee_picture.Image = null;
                }

                addEmployee_password.Text = row.Cells[9].Value.ToString(); // Password (cột 9)
            }
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
            EmployeeData updatedEmployee = new EmployeeData
            {
                EmployeeID = addEmployee_id.Text.Trim(),
                Name = addEmployee_fullName.Text.Trim(),
                Gender = addEmployee_gender.Text.Trim(),
                Contact = addEmployee_phoneNum.Text.Trim(),
                Position = addEmployee_position.Text.Trim(),
                Password = addEmployee_password.Text.Trim(),
                Image = addEmployee_picture.ImageLocation,
                Salary = 0, // Giá trị mặc định, bạn có thể thêm TextBox để nhập
                Status = "Active" // Giá trị mặc định
            };

            if (EmployeeData.UpdateEmployee(updatedEmployee))
            {
                displayEmployeeData();
                MessageBox.Show("Updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields(); // Xóa các trường sau khi cập nhật
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
                displayEmployeeData();
                MessageBox.Show("Deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                clearFields(); // Xóa các trường sau khi xóa
            }
            else
            {
                MessageBox.Show("Employee not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Giữ nguyên nếu không cần thay đổi
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
    }

}
