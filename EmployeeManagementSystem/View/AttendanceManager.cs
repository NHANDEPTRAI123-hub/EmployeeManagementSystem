using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using EmployeeManagementSystem.Controller;


namespace EmployeeManagementSystem.View
{
    public partial class AttendanceManager : UserControl
    {
        private AttendanceManagerController _controller;

        public AttendanceManager()
        {
            InitializeComponent();
            _controller = new AttendanceManagerController(this);
        }

        public void SetController(AttendanceManagerController controller)
        {
            _controller = controller;
        }

        public void RefreshData()
        {
            _controller.RefreshData();
        }

        public void DisplayAttendance(List<Attendance> attendances)
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = attendances;
        }

        public int GetRowCount()
        {
            return dataGridView1.Rows.Count;
        }

        public List<object> GetAttendanceDataForExport()
        {
            List<object> attendanceList = new List<object>();
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (!row.IsNewRow)
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
            return attendanceList;
        }

        public void ShowMessage(string message, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            MessageBox.Show(message, caption, buttons, icon);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            _controller.LoadAttendanceData();
        }

        private void refresh_refreshBtn_Click(object sender, EventArgs e)
        {
            _controller.FilterAttendance(comboBox1.SelectedItem?.ToString().Trim(), 
                                        comboBox2.SelectedItem?.ToString().Trim(), 
                                        dateTimePicker1.Value.Date);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _controller.ExportReport();
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

        private void AttendanceManager_Load(object sender, EventArgs e)
        {
        }
    }
}
