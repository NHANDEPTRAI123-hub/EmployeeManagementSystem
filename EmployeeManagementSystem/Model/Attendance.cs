using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EmployeeManagementSystem
{
    public class Attendance
    {
        private string employeeID;
        private string fullName;
        private DateTime timestamp;
        private string action;
        private string date;

        public string EmployeeID
        {
            get { return employeeID; }
            set { employeeID = !string.IsNullOrEmpty(value) ? value.Trim() : "Unknown"; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public DateTime Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }

        public string Action
        {
            get { return action; }
            set { action = (value == "CheckIn" || value == "CheckOut") ? value : "Unknown"; }
        }

        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        private static string filePath = "attendance.json";

    // Đọc danh sách chấm công từ JSON
        public static List<Attendance> LoadFromJson()
            {
                if (!File.Exists(filePath))
                {
                    return new List<Attendance>();
                }

                string json = File.ReadAllText(filePath);
                List<Attendance> records = JsonConvert.DeserializeObject<List<Attendance>>(json);

                if (records == null)
                {
                    return new List<Attendance>();
                }

                return records;
            }

            // Lưu danh sách chấm công vào JSON
            public static void SaveToJson(List<Attendance> attendanceRecords)
            {
                string json = JsonConvert.SerializeObject(attendanceRecords, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }

            // Thêm một bản ghi chấm công
            public static void AddAttendance(Attendance record)
            {
                List<Attendance> records = LoadFromJson();
                records.Add(record);
                SaveToJson(records);
            }

            // Lấy danh sách chấm công của một nhân viên (sử dụng string)
            public static List<Attendance> GetAttendanceByEmployee(string employeeID)
            {
                List<Attendance> records = LoadFromJson();
                List<Attendance> employeeRecords = new List<Attendance>();

                for (int i = 0; i < records.Count; i++)
                {
                    if (records[i].EmployeeID == employeeID)
                    {
                        employeeRecords.Add(records[i]);
                    }
                }

                return employeeRecords;
            }
     }
}
