using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class CurrentSalariesReportBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }

        public List<CurrentSalariesReportList> CurrentSalariesReportLists { get; set; }
    }
    public class CurrentSalariesReportList {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public DateTime DateOfJoin { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public double CurrentSalary { get; set; }

        [Required]
        public double Allowance { get; set; }

    }

}