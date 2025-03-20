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
        private EmployeeData currentEmployee;
        private const string HOME_WIFI_SSID = "A2 1804"; // Thay bằng SSID Wi-Fi tại nhà bạn

        public AttendanceForm(EmployeeData employee)
        {
            InitializeComponent();
            this.currentEmployee = employee;
        }

        private void AttendanceForm_Load(object sender, EventArgs e)
        {
            lblEmployeeInfo.Text = "Nhân viên: " + currentEmployee.Name + " | ID: " + currentEmployee.EmployeeID + " | Chức vụ: " + currentEmployee.Position;
            UpdateLastAttendanceStatus();
        }

        // Kiểm tra xem có kết nối với Wi-Fi tại nhà hay không
        private bool IsConnectedToHomeWiFi()
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "netsh";
                process.StartInfo.Arguments = "wlan show interfaces";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();

                // Tìm SSID trong output
                string[] lines = output.Split('\n');
                foreach (string line in lines)
                {
                    if (line.Contains("SSID") && !line.Contains("BSSID"))
                    {
                        string ssid = line.Split(':')[1].Trim();
                        if (ssid == HOME_WIFI_SSID)
                        {
                            return true;
                        }
                    }
                }
                return false; // Không kết nối với Wi-Fi tại nhà
            }
            catch
            {
                return false; // Nếu có lỗi, coi như không kết nối
            }
        }

        private void btnCheckIn_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra Wi-Fi tại nhà trước khi cho phép Check-in
                if (!IsConnectedToHomeWiFi())
                {
                    MessageBox.Show("Bạn không kết nối với Wi-Fi tại công ty! Chỉ có thể Check-in khi dùng Wi-Fi tại công ty.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string today = DateTime.Today.ToString("yyyy-MM-dd");
                List<Attendance> records = Attendance.GetAttendanceByEmployee(currentEmployee.EmployeeID);
                Attendance todayRecord = null;

                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Date == today && records[i].Action == "CheckIn")
                    {
                        todayRecord = records[i];
                        break;
                    }
                }

                if (todayRecord != null)
                {
                    MessageBox.Show("Bạn đã Check-in hôm nay rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeID = currentEmployee.EmployeeID,
                    FullName = currentEmployee.Name,
                    Timestamp = DateTime.Now,
                    Action = "CheckIn",
                    Date = today
                };

                Attendance.AddAttendance(attendance);

                MessageBox.Show("Check-in thành công lúc " + DateTime.Now.ToString("HH:mm:ss") + "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateLastAttendanceStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra Wi-Fi tại nhà trước khi cho phép Check-out
                if (!IsConnectedToHomeWiFi())
                {
                    MessageBox.Show("Bạn không kết nối với Wi-Fi tại nhà! Chỉ có thể Check-out khi dùng Wi-Fi tại nhà.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string today = DateTime.Today.ToString("yyyy-MM-dd");
                List<Attendance> records = Attendance.GetAttendanceByEmployee(currentEmployee.EmployeeID);
                Attendance todayCheckIn = null;
                Attendance todayCheckOut = null;

                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].Date == today && records[i].Action == "CheckIn")
                    {
                        todayCheckIn = records[i];
                    }
                    else if (records[i].Date == today && records[i].Action == "CheckOut")
                    {
                        todayCheckOut = records[i];
                    }
                }

                if (todayCheckIn == null)
                {
                    MessageBox.Show("Bạn chưa Check-in hôm nay!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (todayCheckOut != null)
                {
                    MessageBox.Show("Bạn đã Check-out hôm nay rồi!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeID = currentEmployee.EmployeeID,
                    FullName = currentEmployee.Name,
                    Timestamp = DateTime.Now,
                    Action = "CheckOut",
                    Date = today,
                    ShiftName = todayCheckIn.ShiftName // Sử dụng cùng ca làm với CheckIn
                };

                Attendance.AddAttendance(attendance);

                MessageBox.Show("Check-out thành công lúc " + DateTime.Now.ToString("HH:mm:ss") + "!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateLastAttendanceStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateLastAttendanceStatus()
        {
            List<Attendance> records = Attendance.GetAttendanceByEmployee(currentEmployee.EmployeeID);
            Attendance lastRecord = null;

            for (int i = 0; i < records.Count; i++)
            {
                if (lastRecord == null || records[i].Timestamp > lastRecord.Timestamp)
                {
                    lastRecord = records[i];
                }
            }

            if (lastRecord != null)
            {
                string statusInfo = !string.IsNullOrEmpty(lastRecord.Status) ? " - " + lastRecord.Status : "";
                string shiftInfo = !string.IsNullOrEmpty(lastRecord.ShiftName) ? " (Ca " + lastRecord.ShiftName + ")" : "";

                lblLastStatus.Text = "Lần chấm công gần nhất: " + lastRecord.Action + shiftInfo +
                                     " lúc " + lastRecord.Timestamp.ToString("HH:mm:ss dd/MM/yyyy") + statusInfo;
            }
            else
            {
                lblLastStatus.Text = "Chưa có bản ghi chấm công nào.";
            }
        }

        private void logout_btn_Click(object sender, EventArgs e)
        {
            DialogResult check = MessageBox.Show("Are you sure you want to logout?",
                "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                PreLoginForm preLoginForm = new PreLoginForm();
                preLoginForm.Show();
                this.Close(); // Đóng form hiện tại hoàn toàn để tránh hiện tượng tồn tại form cũ
            }
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
