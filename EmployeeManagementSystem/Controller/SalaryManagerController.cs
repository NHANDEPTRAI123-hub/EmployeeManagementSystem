using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace EmployeeManagementSystem.Controller
{
    public class SalaryManager
    {
        // Tính toán lương dựa trên danh sách điểm danh của nhân viên
        public void CalculateSalary(string employeeID)
        {
            // Lấy thông tin lương của nhân viên
            SalaryData salary = SalaryData.GetSalaryByEmployeeID(employeeID);
            if (salary == null)
            {
                return;
            }


            // Reset Deduction trước khi tính toán
            salary.Deduction = 0;

            // Lấy danh sách điểm danh của nhân viên
            List<EmployeeManagementSystem.Attendance> attendanceList = EmployeeManagementSystem.Attendance.GetAttendanceByEmployee(employeeID);

            // Tính toán số tiền bị trừ (Deduction)
            foreach (EmployeeManagementSystem.Attendance attendance in attendanceList)
            {
                if (attendance.Status == "Check in late" || attendance.Status == "Check out early")
                {
                    decimal deductionRate = 0.05m; // Trừ 5% lương mỗi lần đi trễ/về sớm
                    decimal deduction = salary.Salary * deductionRate;
                    salary.Deduction += deduction; // Cộng dồn số tiền bị trừ
                }
            }

            // Cập nhật lại CurrentSalary theo công thức: Salary + Bonus - Deduction
            salary.UpdateCurrentSalary();

            // Lưu lại thông tin lương đã cập nhật
            UpdateSalary(salary);
        }

        // Cập nhật thông tin lương của nhân viên
        public void UpdateSalary(SalaryData updatedSalary)
        {
            List<SalaryData> salaryList = SalaryData.LoadFromJson();
            bool found = false;

            for (int i = 0; i < salaryList.Count; i++)
            {
                if (salaryList[i].EmployeeID == updatedSalary.EmployeeID)
                {
                    salaryList[i] = updatedSalary;
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                salaryList.Add(updatedSalary);
            }

            SalaryData.SaveToJson(salaryList);
        }

        // Thêm phần thưởng vào lương của nhân viên
        public void AddReward(string employeeID, decimal rewardAmount)
        {
            SalaryData salary = SalaryData.GetSalaryByEmployeeID(employeeID);
            if (salary == null)
            {
                return;
            }

            // Thêm phần thưởng vào Bonus
            salary.Bonus += rewardAmount;

            // Cập nhật lại CurrentSalary
            salary.UpdateCurrentSalary();

            // Lưu lại thông tin lương đã cập nhật
            UpdateSalary(salary);
        }
    }
}
