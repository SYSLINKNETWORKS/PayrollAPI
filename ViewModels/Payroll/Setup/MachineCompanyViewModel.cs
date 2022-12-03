using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class MachineCompanyBaseModel {

    }
    public class MachineCompanyFoundationModel : MachineCompanyBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class MachineCompanyViewModel : MachineCompanyFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class MachineCompanyViewByIdModel : MachineCompanyFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class MachineCompanyAddModel : MachineCompanyFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class MachineCompanyUpdateModel : MachineCompanyFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class MachineCompanyDeleteModel : MachineCompanyBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}