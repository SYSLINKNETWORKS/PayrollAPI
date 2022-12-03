using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class AdvanceBaseModel
    {

    }
    public class AdvanceFoundationModel : AdvanceBaseModel
    {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public double Amount { get; set; }
        

    }

    public class AdvanceViewModel : AdvanceFoundationModel
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
    public class AdvanceViewByIdModel : AdvanceFoundationModel
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
    public class AdvanceAddModel : AdvanceFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class AdvanceUpdateModel : AdvanceFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class AdvanceDeleteModel : AdvanceBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}