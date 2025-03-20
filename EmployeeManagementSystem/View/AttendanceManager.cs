using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;

using EmployeeManagementSystem;
using System.Linq;

namespace EmployeeManagementSystem.View
{
    public partial class AttendanceManager : UserControl
    {

        private List<Attendance> attendances = new List<Attendance>(); // Danh sách chấm công

        public AttendanceManager()
        {
            InitializeComponent();
            LoadAttendanceData(); // Nạp dữ liệu vào danh sách
            displayAttendance();  // Hiển thị danh sách lên DataGridView
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }
            displayAttendance();
        }

        // Tải dữ liệu từ JSON vào danh sách attendances và hiển thị trên DataGridView
        private void LoadAttendanceData()
        {
            attendances = Attendance.LoadFromJson() ?? new List<Attendance>(); // Tránh lỗi nếu dữ liệu rỗng
            displayAttendance();
        }

        // Hiển thị danh sách chấm công lên DataGridView
        private void displayAttendance()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = attendances;
        }
        // Cập nhật lại dữ liệu khi click vào ô bất kỳ trên DataGridView
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadAttendanceData();
        }

        // Lọc danh sách chấm công theo ca làm
        private void refresh_refreshBtn_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn ca làm việc!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn Action!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (attendances.Count == 0)
            {
                MessageBox.Show("Danh sách chấm công trống!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectedShift = comboBox1.SelectedItem.ToString().Trim();
            string selectedAction = comboBox2.SelectedItem.ToString().Trim();
            DateTime selectedDate = dateTimePicker1.Value.Date;

            List<Attendance> filteredList = new List<Attendance>();

            foreach (var a in attendances)
            {
                if (!string.IsNullOrEmpty(a.date) && DateTime.TryParse(a.date, out DateTime attendanceDate))
                {
                    // Kiểm tra nếu ngày khớp với ngày được chọn
                    if (attendanceDate.Date == selectedDate)
                    {
                        // Kiểm tra điều kiện Shift và Action
                        bool matchShift = selectedShift == "All" || (!string.IsNullOrEmpty(a.ShiftName) && a.ShiftName.Trim().Equals(selectedShift, StringComparison.OrdinalIgnoreCase));
                        bool matchAction = selectedAction == "All" || (!string.IsNullOrEmpty(a.Action) && a.Action.Trim().Equals(selectedAction, StringComparison.OrdinalIgnoreCase));

                        // Nếu cả Shift và Action đều phù hợp, thêm vào danh sách
                        if (matchShift && matchAction)
                        {
                            filteredList.Add(a);
                        }
                    }
                }
            }

            filteredList = filteredList
                .Where(a => !string.IsNullOrEmpty(a.date) && DateTime.TryParse(a.date, out _)) // Lọc bỏ dòng có date rỗng hoặc không hợp lệ
                .OrderBy(a => DateTime.Parse(a.date))
                .ToList();


            if (filteredList.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu chấm công cho ngày và bộ lọc đã chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Cập nhật DataGridView
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = filteredList;
        }






        // Xuất file báo cáo 
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json";
            saveFileDialog.Title = "Lưu báo cáo chấm công";
            saveFileDialog.FileName = "Attendance_Report.json";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    List<object> attendanceList = new List<object>();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (!row.IsNewRow) // Bỏ qua hàng trống cuối cùng
                        {
                            var attendanceRecord = new
                            {
                                ShiftName = row.Cells["ShiftName"].Value?.ToString(),
                                EmployeeID = row.Cells["EmployeeID"].Value?.ToString(),
                                FullName = row.Cells["FullName"].Value?.ToString(),
                                Action = row.Cells["Action"].Value?.ToString(),
                                Date = row.Cells["Date"].Value?.ToString(),
                                Status = row.Cells["Status"].Value?.ToString()
                            };

                            attendanceList.Add(attendanceRecord);
                        }
                    }

                    string jsonOutput = JsonConvert.SerializeObject(attendanceList, Formatting.Indented);

                    File.WriteAllText(saveFileDialog.FileName, jsonOutput, Encoding.UTF8);
                    MessageBox.Show("Xuất báo cáo JSON thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi xuất báo cáo: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
