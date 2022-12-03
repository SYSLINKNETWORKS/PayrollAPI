using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class DesignationBaseModel {

    }
    public class DesignationFoundationModel : DesignationBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }

        [Required]
        public bool Director { get; set; }

        [Required]
        public bool Salesman { get; set; }

        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class DesignationViewModel : DesignationFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class DesignationViewByIdModel : DesignationFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class DesignationAddModel : DesignationFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class DesignationUpdateModel : DesignationFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class DesignationDeleteModel : DesignationBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}