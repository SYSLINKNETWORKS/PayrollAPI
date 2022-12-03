using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class AttendanceMachineGroupBaseModel {

    }
    public class AttendanceMachineGroupFoundationModel : AttendanceMachineGroupBaseModel {
        [Required]
        [StringLength (250)]
        public string Name { get; set; }


        [Required]
        [StringLength (1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class AttendanceMachineGroupViewModel : AttendanceMachineGroupFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class AttendanceMachineGroupViewByIdModel : AttendanceMachineGroupFoundationModel {
        [Required]
        public Guid Id { get; set; }
    }
    public class AttendanceMachineGroupAddModel : AttendanceMachineGroupFoundationModel {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class AttendanceMachineGroupUpdateModel : AttendanceMachineGroupFoundationModel {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class AttendanceMachineGroupDeleteModel : AttendanceMachineGroupBaseModel {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}