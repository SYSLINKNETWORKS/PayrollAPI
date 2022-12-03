using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class LoanSummaryBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateAsOn { get; set; }

        public List<LoanSummaryList> LoanSummaryLists { get; set; }
    }
    public class LoanSummaryList
    {
        [Required]
        public Guid Id { get; set; }

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
        public Guid LoanCatgoryId { get; set; }

        [Required]
        public string LoanCatgoryName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Amount { get; set; }

        [Required]
        public double NoofInstalment { get; set; }

        [Required]
        public double InstalmentAmount { get; set; }

        [Required]
        public double Receiving { get; set; }

        [Required]
        public double Balance { get; set; }

        [Required]
        public string Status { get; set; }

    }

}