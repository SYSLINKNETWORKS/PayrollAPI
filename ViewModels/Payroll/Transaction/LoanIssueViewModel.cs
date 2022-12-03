using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class LoanIssueBaseModel
    {

    }
    public class LoanIssueFoundationModel : LoanIssueBaseModel
    {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public Guid LoanCategoryId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double Amount { get; set; }
        public double NoOfInstalment { get; set; }
        public double InstalmentAmount { get; set; }

        public string Remarks { get; set; }
        public bool LoanStatus { get; set; }
    }

    public class LoanIssueViewModel : LoanIssueFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Employee { get; set; }

        [Required]
        public string LoanCategory { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class LoanIssueViewByIdModel : LoanIssueFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public string LoanCategoryName { get; set; }
        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class LoanIssueAddModel : LoanIssueFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class LoanIssueUpdateModel : LoanIssueFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class LoanIssueDeleteModel : LoanIssueBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}