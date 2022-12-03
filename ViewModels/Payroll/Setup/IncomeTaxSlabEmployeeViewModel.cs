using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class IncomeTaxSlabEmployeeBaseModel {

    }
    public class IncomeTaxSlabEmployeeFoundationModel : IncomeTaxSlabEmployeeBaseModel {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double SlabFrom { get; set; }
        [Required]
        public double SlabTo { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double Amount { get; set; }
        
        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class IncomeTaxSlabEmployeeViewModel : IncomeTaxSlabEmployeeFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class IncomeTaxSlabEmployeeViewByIdModel : IncomeTaxSlabEmployeeFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class IncomeTaxSlabEmployeeAddModel : IncomeTaxSlabEmployeeFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class IncomeTaxSlabEmployeeUpdateModel : IncomeTaxSlabEmployeeFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class IncomeTaxSlabEmployeeDeleteModel : IncomeTaxSlabEmployeeBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}