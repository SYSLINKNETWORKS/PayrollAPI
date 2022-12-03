using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class CheckInOutBaseModel
    {

    }
    public class CheckInOutFoundationModel : CheckInOutBaseModel
    {
        [Required]
        public int UserId { get; set; }
        public DateTime CheckTime { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public bool Approved { get; set; } = false;

        public Guid CheckInOutCategoryId { get; set; }

        public Guid MachineId { get; set; }
    }

    public class CheckInOutViewModel : CheckInOutFoundationModel
    {

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class CheckInOutViewByIdModel : CheckInOutFoundationModel { }
    public class CheckInOutAddModel : CheckInOutFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class CheckInOutUpdateModel : CheckInOutFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class CheckInOutDeleteModel : CheckInOutBaseModel
    {

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class CheckInOutMachineListModel
    {
        public List<CheckInOutMachineModel> CheckInOutMachineModels { get; set; }
    }
    public class CheckInOutMachineModel : CheckInOutBaseModel
    {
        [Required]
        public int UserId { get; set; }
        public string CheckTime { get; set; }

        public Guid? MachineId { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
    public class AttendanceProcessTable
    {
        [Required]
        public int MachineId { get; set; }

        [Required]
        public DateTime Date { get; set; }

    }

}