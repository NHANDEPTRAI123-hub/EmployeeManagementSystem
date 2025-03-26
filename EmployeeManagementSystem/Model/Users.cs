using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace EmployeeManagementSystem
{

    public abstract class Users
    {
        private int id;
        private string FullName;
        private string password;
        private DateTime dateRegister;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Fullname
        {
            get { return FullName; }
            set { FullName = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public DateTime DateRegister
        {
            get { return dateRegister; }
            set { dateRegister = value; }
        }

        public Users() { }

        public Users(int id, string FullName, string password, DateTime dateRegister)
        {
            ID = id;
            Fullname = FullName;
            Password = password;
            DateRegister = dateRegister;
        }

        // Phương thức ảo cho đa hình
        public virtual string GetDescription()
        {
            return $"User: {Fullname}, Registered on: {DateRegister.ToShortDateString()}";
        }

        // Phương thức trừu tượng
        public abstract string GetUserType();
    }

    // Lớp Employee kế thừa Users
    public class Employee : Users
    {
        private static string filePath = "Employeeusers.json";

        public Employee() { }

        public Employee(int id, string FullName, string password, DateTime dateRegister)
            : base(id, FullName, password, dateRegister)
        {
        }

        // Triển khai phương thức trừu tượng
        public override string GetUserType()
        {
            return "Employee";
        }

        // Ghi đè phương thức ảo (đa hình)
        public override string GetDescription()
        {
            return $"Employee: {Fullname}";
        }

        // Lưu danh sách nhân viên vào file JSON
        public static void SaveToJson(List<Employee> employees)
        {
            string json = JsonConvert.SerializeObject(employees, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Đọc danh sách nhân viên từ file JSON
        public static List<Employee> LoadFromJson()
        {
            if (!File.Exists(filePath))
                return new List<Employee>();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Employee>>(json);
        }
    }

    // Lớp Manager kế thừa Users
    public class Manager : Users
    {
        private static string filePath = "Managerusers.json";

        public Manager() { }

        public Manager(int id, string username, string password, DateTime dateRegister)
            : base(id, username, password, dateRegister)
        {
        }

        // Triển khai phương thức trừu tượng
        public override string GetUserType()
        {
            return "Manager";
        }

        // Ghi đè phương thức ảo (đa hình)
        public override string GetDescription()
        {
            return $"Manager: {Fullname}";
        }

        // Lưu danh sách quản lý vào file JSON
        public static void SaveToJson(List<Manager> managers)
        {
            string json = JsonConvert.SerializeObject(managers, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        // Đọc danh sách quản lý từ file JSON
        public static List<Manager> LoadFromJson()
        {
            if (!File.Exists(filePath))
                return new List<Manager>();

            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Manager>>(json);
        }
    }
}
