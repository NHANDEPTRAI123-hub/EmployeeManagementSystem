using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagementSystem.Controller
{
    public class AttendanceController
    {
        private readonly AttendanceForm _view;
        private readonly EmployeeData _currentEmployee;
        private const string Company_WIFI_SSID = "Nhan"; 
        public delegate void ShowMessageHandler(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        public delegate DialogResult ShowConfirmationHandler(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);

        public event ShowMessageHandler OnShowMessage;
        public event ShowConfirmationHandler OnShowConfirmation;
        public AttendanceController(AttendanceForm view, EmployeeData employee)
        {
            _view = view;
            _currentEmployee = employee;
            _view.SetController(this); // Liên kết Controller với View
            // Đăng ký các event với các phương thức của view
            OnShowMessage += _view.ShowMessage;
            OnShowConfirmation += _view.ShowConfirmation;
        }

        public void LoadForm()
        {
            _view.SetEmployeeInfo($"Employee: {_currentEmployee.Name} | ID: {_currentEmployee.EmployeeID} | Position: {_currentEmployee.Position}");
            UpdateLastAttendanceStatus();
        }

        public void CheckIn()
        {
            try
            {
                if (!IsConnectedToCompanyWiFi())
                {
                    OnShowMessage?.Invoke("You can only check in when using the companies wifi",
                         "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string today = DateTime.Today.ToString("yyyy-MM-dd");
                List<Attendance> records = Attendance.GetAttendanceByEmployee(_currentEmployee.EmployeeID);
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
                    OnShowMessage?.Invoke("You have already check in!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeID = _currentEmployee.EmployeeID,
                    FullName = _currentEmployee.Name,
                    Timestamp = DateTime.Now,
                    Action = "CheckIn",
                    Date = today
                };

                Attendance.AddAttendance(attendance);

                OnShowMessage?.Invoke($"Check in successfully at {DateTime.Now.ToString("HH:mm:ss")}!",
                     "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateLastAttendanceStatus();
            }
            catch (Exception ex)
            {
                OnShowMessage?.Invoke($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CheckOut()
        {
            try
            {
                if (!IsConnectedToCompanyWiFi())
                {
                    OnShowMessage?.Invoke("You can only check out when using the company wifi",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string today = DateTime.Today.ToString("yyyy-MM-dd");
                List<Attendance> records = Attendance.GetAttendanceByEmployee(_currentEmployee.EmployeeID);
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
                    OnShowMessage?.Invoke("You have not check yet in!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (todayCheckOut != null)
                {
                    OnShowMessage?.Invoke("You have already check out!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Attendance attendance = new Attendance
                {
                    EmployeeID = _currentEmployee.EmployeeID,
                    FullName = _currentEmployee.Name,
                    Timestamp = DateTime.Now,
                    Action = "CheckOut",
                    Date = today,
                    ShiftName = todayCheckIn.ShiftName // Sử dụng cùng ca làm với CheckIn
                };

                Attendance.AddAttendance(attendance);

                OnShowMessage?.Invoke($"Check-out successfully at {DateTime.Now.ToString("HH:mm:ss")}!",
                     "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateLastAttendanceStatus();
            }
            catch (Exception ex)
            {
                OnShowMessage?.Invoke($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void UpdateLastAttendanceStatus()
        {
            List<Attendance> records = Attendance.GetAttendanceByEmployee(_currentEmployee.EmployeeID);
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
                string shiftInfo = !string.IsNullOrEmpty(lastRecord.ShiftName) ? " (Shift: " + lastRecord.ShiftName + ")" : "";
                _view.SetLastStatus($"Last time check in/out: {lastRecord.Action}{shiftInfo} at {lastRecord.Timestamp.ToString("HH:mm:ss dd/MM/yyyy")}{statusInfo}");
            }
            else
            {
                _view.SetLastStatus("No attendance record.");
            }
        }

        private bool IsConnectedToCompanyWiFi()
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

                string[] lines = output.Split('\n');
                foreach (string line in lines)
                {
                    if (line.Contains("SSID") && !line.Contains("BSSID"))
                    {
                        string ssid = line.Split(':')[1].Trim();
                        if (ssid == Company_WIFI_SSID)
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Logout()
        {
            DialogResult check = _view.ShowConfirmation("Are you sure you want to logout?",
                "Confirmation Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (check == DialogResult.Yes)
            {
                PreLoginForm preLoginForm = new PreLoginForm();
                preLoginForm.Show();
                _view.CloseForm();
            }
        }

        public void Exit()
        {
            Application.Exit();
        }
    }
}
