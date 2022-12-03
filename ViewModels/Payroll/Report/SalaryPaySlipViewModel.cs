using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class SalaryPaySlipViewModelBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public List<SalaryPaySlipList> SalaryPaySlipLists { get; set; }
        public List<SalaryDetail> SalaryDetails { get; set; }
    }
    public class SalaryDetail
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public double BasicSalary { get; set; }
        [Required]
        public double Allowance { get; set; }
        [Required]
        public double GrossSalary { get; set; }
        [Required]
        public int MonthDays { get; set; }
        [Required]
        public int PresentDays { get; set; }
        [Required]
        public int AbsentDays { get; set; }
        [Required]
        public double LateDays { get; set; }
        [Required]
        public double LateDaysDeduction { get; set; }
        [Required]
        public int PayableDays { get; set; }
        [Required]
        public double AttendanceAllowance { get; set; }
        [Required]
        public double OtherAddition { get; set; }
        [Required]
        public double LateDeduction { get; set; }
        [Required]
        public double Advance { get; set; }
        [Required]
        public double Loan { get; set; }
        [Required]
        public double Takaful { get; set; }
        [Required]
        public double IncomeTax { get; set; }
        [Required]
        public double OtherDeduction { get; set; }
        [Required]
        public double TotalGrossSalary { get; set; }
        [Required]
        public double OThours { get; set; }
        [Required]
        public double OTAmount { get; set; }
        [Required]
        public double NetSalary { get; set; }
    }
    public class SalaryPaySlipList
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public DateTime DailyDate { get; set; }


        [Required]
        public DateTime DateOfJoin { get; set; }

        [Required]
        public DateTime? RosterInn { get; set; }

        [Required]
        public DateTime? RosterOut { get; set; }


        [Required]
        public double RosterOverTime { get; set; }

        [Required]
        public Double RosterWorkingHours { get; set; }

        [Required]
        public Boolean HolidayCheck { get; set; }

        [Required]
        public DateTime? InnTime { get; set; }

        [Required]
        public DateTime? OutTime { get; set; }

        [Required]
        public bool LateCheck { get; set; }

        [Required]
        public double LateComing { get; set; }

        [Required]
        public double EarlyGoing { get; set; }

        [Required]
        public bool OverTimeCheck { get; set; }

        [Required]
        public double OverTime { get; set; }

        [Required]
        public double EarlyOverTime { get; set; }

        [Required]
        public double TotalOverTime { get; set; }

        [Required]
        public double WorkingHours { get; set; }


        [Required]
        public string Remarks { get; set; }

    }



}