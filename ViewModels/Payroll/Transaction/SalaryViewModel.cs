using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class SalaryBaseModel {

    }
    public class SalaryFoundationModel : SalaryBaseModel {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double PreviousAmount { get; set; }
        public double IncreamentPercentage { get; set; }
        public double IncreamentAmount { get; set; }
        public double CurrentAmount { get; set; }

    }

    public class SalaryViewModel : SalaryFoundationModel {
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
    public class SalaryViewByIdModel : SalaryFoundationModel {
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
    public class SalaryAddModel : SalaryFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class SalaryUpdateModel : SalaryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class SalaryDeleteModel : SalaryBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}