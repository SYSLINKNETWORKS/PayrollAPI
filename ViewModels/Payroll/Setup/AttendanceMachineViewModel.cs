using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class AttendanceMachineBaseModel
    {

    }
    public class AttendanceMachineFoundationModel : AttendanceMachineBaseModel
    {
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string MacIP { get; set; }

        [Required]

        public int Port { get; set; }


        [Required]
        [StringLength(1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class AttendanceMachineViewModel : AttendanceMachineFoundationModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string AttendanceMachineCategoryName { get; set; }
        [Required]
        public string AttendanceMachineGroupName { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class AttendanceMachineViewByIdModel : AttendanceMachineFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid AttendanceMachineCategoryId { get; set; }

        [Required]
        public string AttendanceMachineCategoryName { get; set; }

        [Required]
        public Guid AttendanceMachineGroupId { get; set; }
        [Required]
        public string AttendanceMachineGroupName { get; set; }
    }
    public class AttendanceMachineAddModel : AttendanceMachineFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }
        [Required]
        public Guid AttendanceMachineCategoryId { get; set; }

        [Required]
        public Guid AttendanceMachineGroupId { get; set; }

    }
    public class AttendanceMachineUpdateModel : AttendanceMachineFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
        [Required]
        public Guid AttendanceMachineCategoryId { get; set; }
        [Required]
        public Guid AttendanceMachineGroupId { get; set; }

    }
    public class AttendanceMachineDeleteModel : AttendanceMachineBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}