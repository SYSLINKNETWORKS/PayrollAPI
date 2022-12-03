using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class AdvanceReportBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }
        
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<AdvanceReport> AdvancesList { get; set; }
    }
    public class AdvanceReport {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeTemporaryPermanent { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public string EmployeeCategory { get; set; }

        [Required]
        public DateTime Date { get; set; }

        
        [Required]
        public string CashBank { get; set; }

        
        [Required]
        public double Amount { get; set; }

        
        public string Voucher { get; set; }

        
    }
    

}