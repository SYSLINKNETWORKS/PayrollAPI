using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class RosterBaseModel {

    }
    public class RosterFoundationModel : RosterBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class RosterViewModel : RosterFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class RosterViewByIdModel : RosterFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class RosterAddModel : RosterFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class RosterUpdateModel : RosterFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class RosterDeleteModel : RosterBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}