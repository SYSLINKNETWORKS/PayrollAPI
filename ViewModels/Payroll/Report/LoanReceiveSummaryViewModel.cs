using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class LoanReceiveSummaryBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<LoanReceiveSummaryList> LoanReceiveSummaryLists { get; set; }
    }
    public class LoanReceiveSummaryList
    {
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
        public Guid LoanCatgoryId { get; set; }
        
        [Required]
        public string LoanCatgoryName { get; set; }
        
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        [Required]
        public double Amount { get; set; }

        [Required]
        public double Receiving { get; set; }

        [Required]
        public double Balance { get; set; }

        public string CashBank { get; set; }
        
        public string Check { get; set; }

        public string Account { get; set; }

        public string voucher { get; set; }



    }


}