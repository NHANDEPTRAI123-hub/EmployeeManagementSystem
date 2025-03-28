using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.IO; // Already included for File operations
using ClosedXML.Excel; // Add this for ClosedXML
using EmployeeManagementSystem.View;

namespace EmployeeManagementSystem.Controller
{
    public class AttendanceManagerController
    {
        private readonly AttendanceManager _view;
        private List<Attendance> _attendances;
        public delegate void ShowMessageHandler(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        public event ShowMessageHandler OnShowMessage;
        public AttendanceManagerController(AttendanceManager view)
        {
            _view = view;
            _view.SetController(this);
            // Đăng ký sự kiện OnShowMessage với phương thức ShowMessage của view
            OnShowMessage += _view.ShowMessage;
            LoadAttendanceData();
        }

        public void LoadAttendanceData()
        {
            _attendances = Attendance.LoadFromJson() ?? new List<Attendance>();
            _view.DisplayAttendance(_attendances);
        }

        public void RefreshData()
        {
            if (_view.InvokeRequired)
            {
                _view.Invoke((MethodInvoker)RefreshData);
                return;
            }
            _view.DisplayAttendance(_attendances);
        }

        public void FilterAttendance(string selectedShift, string selectedAction, DateTime selectedDate)
        {
            if (string.IsNullOrEmpty(selectedShift))
            {
                OnShowMessage?.Invoke("Please select shift!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(selectedAction))
            {
                OnShowMessage?.Invoke("Please select action!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_attendances.Count == 0)
            {
                OnShowMessage?.Invoke("Empty attendance list!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            List<Attendance> filteredList = new List<Attendance>();

            foreach (var a in _attendances)
            {
                if (!string.IsNullOrEmpty(a.date) && DateTime.TryParse(a.date, out DateTime attendanceDate))
                {
                    if (attendanceDate.Date == selectedDate)
                    {
                        bool matchShift = selectedShift == "All" || (!string.IsNullOrEmpty(a.ShiftName) && a.ShiftName.Trim().Equals(selectedShift, StringComparison.OrdinalIgnoreCase));
                        bool matchAction = selectedAction == "All" || (!string.IsNullOrEmpty(a.Action) && a.Action.Trim().Equals(selectedAction, StringComparison.OrdinalIgnoreCase));

                        if (matchShift && matchAction)
                        {
                            filteredList.Add(a);
                        }
                    }
                }
            }

            List<Attendance> tempList = new List<Attendance>();
            foreach (var item in filteredList)
            {
                if (!string.IsNullOrEmpty(item.date) && DateTime.TryParse(item.date, out _))
                {
                    tempList.Add(item);
                }
            }

            for (int i = 0; i < tempList.Count - 1; i++)
            {
                for (int j = 0; j < tempList.Count - i - 1; j++)
                {
                    DateTime date1 = DateTime.Parse(tempList[j].date);
                    DateTime date2 = DateTime.Parse(tempList[j + 1].date);
                    if (date1 > date2)
                    {
                        Attendance temp = tempList[j];
                        tempList[j] = tempList[j + 1];
                        tempList[j + 1] = temp;
                    }
                }
            }

            filteredList = tempList;

            if (filteredList.Count == 0)
            {
                OnShowMessage?.Invoke("No attendance data!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _view.DisplayAttendance(filteredList);
        }

        public void ExportReport()
        {
            if (_view.GetRowCount() == 0)
            {
                OnShowMessage?.Invoke("Data not found!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files (*.xlsx)|*.xlsx", // Change filter to Excel files
                Title = "Save Report",
                FileName = "Attendance_Report.xlsx" // Change default file name to .xlsx
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Get the attendance data from the view
                    List<object> attendanceList = _view.GetAttendanceDataForExport();

                    // Create a new Excel workbook using ClosedXML
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("Attendance Report");

                        // Add headers
                        worksheet.Cell(1, 1).Value = "ShiftName";
                        worksheet.Cell(1, 2).Value = "EmployeeID";
                        worksheet.Cell(1, 3).Value = "FullName";
                        worksheet.Cell(1, 4).Value = "Action";
                        worksheet.Cell(1, 5).Value = "Date";
                        worksheet.Cell(1, 6).Value = "Status";

                        // Add data rows
                        for (int i = 0; i < attendanceList.Count; i++)
                        {
                            var record = (dynamic)attendanceList[i];
                            int row = i + 2; // Start from row 2 (row 1 is for headers)

                            worksheet.Cell(row, 1).Value = record.ShiftName?.ToString();
                            worksheet.Cell(row, 2).Value = record.EmployeeID?.ToString();
                            worksheet.Cell(row, 3).Value = record.FullName?.ToString();
                            worksheet.Cell(row, 4).Value = record.Action?.ToString();
                            worksheet.Cell(row, 5).Value = record.Date?.ToString();
                            worksheet.Cell(row, 6).Value = record.Status?.ToString();
                        }

                        // Auto-adjust column widths
                        worksheet.Columns().AdjustToContents();

                        // Save the Excel file
                        workbook.SaveAs(saveFileDialog.FileName);
                    }

                    OnShowMessage?.Invoke("Export report successfully!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    OnShowMessage?.Invoke($"Error when export report: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
