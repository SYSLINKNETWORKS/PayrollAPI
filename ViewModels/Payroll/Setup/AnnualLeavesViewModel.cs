using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class AnnualLeavesBaseModel {

    }
    public class AnnualLeavesFoundationModel : AnnualLeavesBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }

        
        public bool AnnualLeaveAllow { get; set; }

        public int AnnualLeaveDays { get; set; }

        public bool SickLeaveAllow { get; set; }

        public int SickLeaveDays { get; set; }

        public bool CasualLeaveAllow { get; set; }

        public int CasualLeaveDays { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class AnnualLeavesViewModel : AnnualLeavesFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class AnnualLeavesViewByIdModel : AnnualLeavesFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class AnnualLeavesAddModel : AnnualLeavesFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class AnnualLeavesUpdateModel : AnnualLeavesFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class AnnualLeavesDeleteModel : AnnualLeavesBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}