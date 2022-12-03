using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels
{
    public class BranchBaseModel
    {

    }
    public class BranchFoundationModel : BranchBaseModel
    {
       [Required]
        public Guid BranchId { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public String CompanyName { get; set; }

        [Required]
        public string ShortName { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string Mobile { get; set; }

        public string Email { get; set; }

        public string Web { get; set; }

        public bool HeadOffice { get; set; }

        [Required]
        public string Type { get; set; }

        public bool Active { get; set; }

    }
    public class BranchViewModel : BranchFoundationModel
    {

    }
    // public class BranchViewByIdModel : BranchFoundationModel
    // {
    //     [Required]
    //     public Guid Id { get; set; }

    //     [Required]
    //     public Guid CompanyId { get; set; }
    //     [Required]
    //     public String CompanyName { get; set; }

    //     public String LogoHeader { get; set; }
    //     public String LogoFooter { get; set; }

    // }
    // public class BranchAddModel : BranchFoundationModel
    // {
    //     [Required]
    //     public Guid Menu_Id { get; set; }

    //     [Required]
    //     public Guid CompanyId { get; set; }
    //     public String LogoHeader { get; set; }
    //     public String LogoFooter { get; set; }

    // }
    // public class BranchUpdateModel : BranchFoundationModel
    // {

    //     [Required]
    //     public Guid Id { get; set; }

    //     [Required]
    //     public Guid Menu_Id { get; set; }

    //     [Required]
    //     public Guid CompanyId { get; set; }

    //     public String LogoHeader { get; set; }
    //     public String LogoFooter { get; set; }

    // }
    // public class BranchDeleteModel : BranchBaseModel
    // {
    //     [Required]
    //     public Guid Menu_Id { get; set; }

    //     [Required]
    //     public Guid Id { get; set; }
    // }
}