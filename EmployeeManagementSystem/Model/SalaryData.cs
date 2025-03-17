using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EmployeeManagementSystem
{
    class SalaryData
    {
        public string EmployeeID { set; get; }
        public string Name { set; get; }
        public string Gender { set; get; }
        public string Contact { set; get; }
        public string Position { set; get; }
        public int Salary { set; get; }

        private static string filePath = "employees.json"; // File JSON lưu dữ liệu nhân viên

        // Đọc danh sách nhân viên từ JSON
        public static List<SalaryData> LoadFromJson()
        {
            if (!File.Exists(filePath)) return new List<SalaryData>();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<SalaryData>>(json) ?? new List<SalaryData>();
        }

        // Lưu danh sách nhân viên vào JSON
        public static void SaveToJson(List<SalaryData> employees)
        {
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Lấy danh sách nhân viên có trạng thái "Active"
        public List<SalaryData> salaryEmployeeListData()
        {
            List<SalaryData> listdata = LoadFromJson();
            return listdata.FindAll(emp => emp.Salary > 0); // Giữ nguyên logic lọc lương
        }
    }
}
