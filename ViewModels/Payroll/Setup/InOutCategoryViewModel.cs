using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class InOutCategoryBaseModel {

    }
    public class InOutCategoryFoundationModel : InOutCategoryBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class InOutCategoryViewModel : InOutCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class InOutCategoryViewByIdModel : InOutCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class InOutCategoryAddModel : InOutCategoryFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class InOutCategoryUpdateModel : InOutCategoryFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class InOutCategoryDeleteModel : InOutCategoryBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}