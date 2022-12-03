using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class AllowanceBaseModel {

    }
    public class AllowanceFoundationModel : AllowanceBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }

        public double Amount { get; set; }

        public bool Fix { get; set; }

        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class AllowanceViewModel : AllowanceFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class AllowanceViewByIdModel : AllowanceFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class AllowanceAddModel : AllowanceFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class AllowanceUpdateModel : AllowanceFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class AllowanceDeleteModel : AllowanceBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}