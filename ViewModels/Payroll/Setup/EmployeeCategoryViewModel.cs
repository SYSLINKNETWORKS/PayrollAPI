using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class EmployeeCategoryBaseModel {

    }
    public class EmployeeCategoryFoundationModel : EmployeeCategoryBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class EmployeeCategoryViewModel : EmployeeCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class EmployeeCategoryViewByIdModel : EmployeeCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class EmployeeCategoryAddModel : EmployeeCategoryFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class EmployeeCategoryUpdateModel : EmployeeCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class EmployeeCategoryDeleteModel : EmployeeCategoryBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}