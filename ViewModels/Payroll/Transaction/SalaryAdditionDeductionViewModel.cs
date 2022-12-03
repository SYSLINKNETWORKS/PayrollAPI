using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class SalaryAdditionDeductionBaseModel
    {

    }
    public class SalaryAdditionDeductionFoundationModel : SalaryAdditionDeductionBaseModel
    {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        
        public double AdditionAmount { get; set; }
        
        public double DeductionAmount { get; set; }

        

    }

    public class SalaryAdditionDeductionViewModel : SalaryAdditionDeductionFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Employee { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class SalaryAdditionDeductionViewByIdModel : SalaryAdditionDeductionFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string EmployeeName { get; set; }
        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class SalaryAdditionDeductionAddModel : SalaryAdditionDeductionFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class SalaryAdditionDeductionUpdateModel : SalaryAdditionDeductionFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class SalaryAdditionDeductionDeleteModel : SalaryAdditionDeductionBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}