using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace EmployeeManagementSystem
{
    public class EmployeeData
    {
        public int ID { set; get; }
        public string EmployeeID { set; get; }
        public string Name { set; get; }
        public string Gender { set; get; }
        public string Contact { set; get; }
        public string Position { set; get; }
        public string Image { set; get; }
        public int Salary { set; get; }
        public string Status { set; get; }
        public string Password { set; get; }

        private static string filePath = "employees.json"; // Đường dẫn file JSON

        // Đọc danh sách nhân viên từ JSON
        public static List<EmployeeData> LoadFromJson()
        {
            if (!File.Exists(filePath))
            {
                return new List<EmployeeData>();
            }

            string json = File.ReadAllText(filePath);
            List<EmployeeData> data = JsonConvert.DeserializeObject<List<EmployeeData>>(json);

            if (data == null)
            {
                return new List<EmployeeData>();
            }

            return data;
        }

        // Lưu danh sách nhân viên vào JSON
        public static void SaveToJson(List<EmployeeData> employees)
        {
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Lấy danh sách nhân viên không bị xóa
        public List<EmployeeData> employeeListData()
        {
            List<EmployeeData> listdata = LoadFromJson();
            List<EmployeeData> result = new List<EmployeeData>();

            foreach (EmployeeData emp in listdata)
            {
                if (emp.Status != "Deleted")
                {
                    result.Add(emp);
                }
            }

            return result;
        }

        // Thêm nhân viên
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

            // Tìm ID lớn nhất hiện có
            int maxID = 0;
            foreach (EmployeeData emp in employees)
            {
                if (emp.ID > maxID)
                {
                    maxID = emp.ID;
                }
            }

            newEmployee.ID = maxID + 1;

            employees.Add(newEmployee);
            SaveToJson(employees);
            return true; // Thêm thành công
        }

        // Cập nhật nhân viên
        public static bool UpdateEmployee(EmployeeData updatedEmployee)
        {
            List<EmployeeData> employees = LoadFromJson();
            bool isUpdated = false;

            foreach (EmployeeData emp in employees)
            {
                if (emp.EmployeeID == updatedEmployee.EmployeeID)
                {
                    emp.Name = updatedEmployee.Name;
                    emp.Gender = updatedEmployee.Gender;
                    emp.Contact = updatedEmployee.Contact;
                    emp.Position = updatedEmployee.Position;
                    emp.Image = updatedEmployee.Image;
                    emp.Salary = updatedEmployee.Salary;
                    emp.Status = updatedEmployee.Status;
                    emp.Password = updatedEmployee.Password;
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

        // Xóa nhân viên và sắp xếp lại ID
        public static bool DeleteEmployee(string employeeID)
        {
            List<EmployeeData> employees = LoadFromJson();
            bool isDeleted = false;
            int indexToRemove = -1;

            for (int i = 0; i < employees.Count; i++)
            {
                if (employees[i].EmployeeID == employeeID)
                {
                    indexToRemove = i;
                    isDeleted = true;
                    break;
                }
            }

            if (isDeleted && indexToRemove != -1)
            {
                employees.RemoveAt(indexToRemove);

                // Sắp xếp lại ID từ 1 trở đi
                for (int i = 0; i < employees.Count; i++)
                {
                    employees[i].ID = i + 1;
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
