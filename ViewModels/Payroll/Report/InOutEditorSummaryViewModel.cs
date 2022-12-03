using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class InOutEditorSummaryBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<InOutEditorSummary> InOutEditorSummaryLists { get; set; }
    }
    public class InOutEditorSummary {
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
        public DateTime Date { get; set; }

        [Required]
        public DateTime? CheckTime { get; set; }

        [Required]
        public string CheckType { get; set; }
        [Required]
        public Guid? InoutCategoryId { get; set; }
        [Required]
        public string InoutCategoryName { get; set; }

        [Required]
        public bool Approved { get; set; }
    }
    

}