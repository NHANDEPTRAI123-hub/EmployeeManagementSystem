using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace EmployeeManagementSystem
{
    public class EmployeeData
    {
        public int Order { get; set; }  // Thứ tự nhân viên
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public string Password { get; set; }

        private static string filePath = "employees.json"; // Đường dẫn file JSON

        // Đọc danh sách nhân viên từ JSON
        public static List<EmployeeData> LoadFromJson()
        {
            if (!File.Exists(filePath))
                return new List<EmployeeData>();

            string json = File.ReadAllText(filePath);
            List<EmployeeData> data = JsonConvert.DeserializeObject<List<EmployeeData>>(json);

            return data ?? new List<EmployeeData>(); // Tránh lỗi nếu dữ liệu rỗng
        }

        // Lưu danh sách nhân viên vào JSON
        public static void SaveToJson(List<EmployeeData> employees)
        {
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Thêm nhân viên (Tự động tăng `Order`)
        public static bool AddEmployee(EmployeeData newEmployee)
        {
            List<EmployeeData> employees = LoadFromJson();

            // Kiểm tra trùng EmployeeID
            foreach (EmployeeData emp in employees)
            {
                if (emp.EmployeeID == newEmployee.EmployeeID)
                {
                    return false; // Không thêm nếu EmployeeID đã tồn tại
                }
            }

            // Tìm Order lớn nhất hiện có
            int maxOrder = employees.Count > 0 ? employees[employees.Count - 1].Order : 0;

            // Gán Order mới
            newEmployee.Order = maxOrder + 1;
            employees.Add(newEmployee);

            SaveToJson(employees);
            return true;
        }


        // Cập nhật nhân viên (Không thay đổi `Order`)
        public static bool UpdateEmployee(EmployeeData updatedEmployee)
        {
            List<EmployeeData> employees = LoadFromJson();
            bool isUpdated = false;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].EmployeeID == updatedEmployee.EmployeeID)
                {
                    // Giữ nguyên `Order`
                    updatedEmployee.Order = employees[i].Order;

                    // Cập nhật thông tin
                    employees[i] = updatedEmployee;
                    isUpdated = true;
                    break;
                }
            }

            if (isUpdated)
            {
                SaveToJson(employees);
            }

            return isUpdated;
        }

        // Xóa nhân viên và sắp xếp lại `Order`
        // Xóa nhân viên và cập nhật lại Order
        public static bool DeleteEmployee(string employeeID)
        {
            List<EmployeeData> employees = LoadFromJson();
            bool isDeleted = false;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].EmployeeID == employeeID)
                {
                    employees.RemoveAt(i); // Xóa nhân viên
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted)
            {
                // Cập nhật lại Order từ 1
                for (int i = 0; i < employees.Count; i++)
                {
                    employees[i].Order = i + 1;
                }

                SaveToJson(employees);
            }

            return isDeleted;
        }

        // Tìm nhân viên theo EmployeeID
        public static EmployeeData FindEmployeeByID(string employeeID)
        {
            List<EmployeeData> employees = LoadFromJson();

            foreach (EmployeeData emp in employees)
            {
                if (emp.EmployeeID == employeeID)
                {
                    return emp;
                }
            }

            return null;
        }


        

    }
}
