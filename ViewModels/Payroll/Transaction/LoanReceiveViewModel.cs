using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class LoanReceiveBaseModel {

    }
    public class LoanReceiveFoundationModel : LoanReceiveBaseModel {

        [Required]
        public Guid LoanId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double Amount { get; set; }

        public string Type { get; set; }

    }

    public class LoanReceiveViewModel : LoanReceiveFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime LoanIssueDate { get; set; }

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
    public class LoanReceiveViewByIdModel : LoanReceiveFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public double InstallmentAmount { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public Guid LoanCategoryId { get; set; }

        [Required]
        public string LoanCategoryName { get; set; }

        [Required]
        public string CheaqueCash { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class LoanReceiveAddModel : LoanReceiveFoundationModel {
        [Required]
        public string CheaqueCash { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class LoanReceiveUpdateModel : LoanReceiveFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string CheaqueCash { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class LoanReceiveDeleteModel : LoanReceiveBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}