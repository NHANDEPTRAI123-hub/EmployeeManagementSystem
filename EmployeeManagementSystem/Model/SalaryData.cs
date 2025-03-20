using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class SalaryData
{
    public string EmployeeID { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public decimal Salary { get; set; } // Mức lương mặc định
    public decimal Deduction { get; set; } // Số tiền bị trừ do đi trễ/về sớm
    public decimal Bonus { get; set; } // Lương thưởng
    public decimal CurrentSalary { get; set; } // Lương sau khi tính toán

    private static string filePath = "salary_data.json"; // File JSON lưu lương

    public SalaryData()
    {
        Deduction = 0;
        Bonus = 0;
        UpdateCurrentSalary();
    }

    // Cập nhật lại CurrentSalary sau mỗi lần thay đổi Deduction hoặc Bonus
    public void UpdateCurrentSalary()
    {
        CurrentSalary = Salary - Deduction + Bonus;
        if (CurrentSalary < 0) CurrentSalary = 0; // Đảm bảo lương không âm
    }

    // Đọc danh sách lương từ JSON
    public static List<SalaryData> LoadFromJson()
    {
        if (!File.Exists(filePath)) return new List<SalaryData>();

        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<SalaryData>>(json) ?? new List<SalaryData>();
    }

    // Lưu danh sách lương vào JSON
    public static void SaveToJson(List<SalaryData> salaries)
    {
        string json = JsonConvert.SerializeObject(salaries, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // Lấy mức lương theo EmployeeID
    public static SalaryData GetSalaryByEmployeeID(string employeeID)
    {
        List<SalaryData> salaryList = LoadFromJson();
        return salaryList.Find(s => s.EmployeeID == employeeID);
    }
}
