using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace EmployeeManagementSystem
{
    public class Attendance
    {
        private string employeeID;
        private string fullName;
        private DateTime timestamp;
        private string action;
        public string date;
        private string shiftName; // Tên ca làm
        private string status; // Trạng thái chấm công


        public string ShiftName
        {
            get { return shiftName; }
            set { shiftName = value; }
        }

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
        public string Status
        {
            get { return status; }
            set { status = value; }
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
        // Thêm vào phương thức AddAttendance trong class Attendance
        public static void AddAttendance(Attendance record)
        {
            List<Attendance> records = LoadFromJson();
            List<Shift> shifts = new List<Shift>
    {
        new Shift("Morning", new TimeSpan(7, 0, 0), new TimeSpan(11, 0, 0)),
        new Shift("Afternoon", new TimeSpan(13, 0, 0), new TimeSpan(17, 0, 0))
    };

            // Xác định ca làm việc dựa trên thời gian
            TimeSpan currentTime = record.Timestamp.TimeOfDay;
            bool shiftFound = false;

            foreach (var shift in shifts)
            {
                if ((currentTime >= shift.StartTime.Add(new TimeSpan(-1, 0, 0)) &&
                     currentTime <= shift.EndTime.Add(new TimeSpan(1, 0, 0))))
                {
                    record.ShiftName = shift.ShiftName;
                    shiftFound = true;

                    // Xác định status
                    if (record.Action == "CheckIn")
                    {
                        record.Status = currentTime > shift.StartTime ? "Check in late" : "On time";
                    }
                    else if (record.Action == "CheckOut")
                    {
                        record.Status = currentTime < shift.EndTime ? "Check out early" : "On time";
                    }
                    break;
                }
            }

            // Nếu không tìm thấy ca làm nào phù hợp
            if (!shiftFound)
            {
                record.ShiftName = "Unknown";
                record.Status = "Unknown";
            }

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
