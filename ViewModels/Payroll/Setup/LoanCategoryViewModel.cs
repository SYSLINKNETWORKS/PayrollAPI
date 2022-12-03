using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class LoanCategoryBaseModel {

    }
    public class LoanCategoryFoundationModel : LoanCategoryBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class LoanCategoryViewModel : LoanCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class LoanCategoryViewByIdModel : LoanCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class LoanCategoryAddModel : LoanCategoryFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class LoanCategoryUpdateModel : LoanCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class LoanCategoryDeleteModel : LoanCategoryBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}