using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class DepartmentBaseModel {

    }
    public class DepartmentFoundationModel : DepartmentBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class DepartmentViewModel : DepartmentFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class DepartmentViewByIdModel : DepartmentFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class DepartmentAddModel : DepartmentFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class DepartmentUpdateModel : DepartmentFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class DepartmentDeleteModel : DepartmentBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}