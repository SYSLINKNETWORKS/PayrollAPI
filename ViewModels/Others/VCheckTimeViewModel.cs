using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll
{
    public class vCheckTimeViewModelBaseModel
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }
        public List<vCheckTimeViewModel> vCheckTimeViewModel { get; set; }
    }
    public class vCheckTimeViewModel
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string EmployeeCategory { get; set; }

        [Required]
        public string OfficeWorker { get; set; }

        [Required]
        public string TemporaryPermanent { get; set; }

        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }

        [Required]
        public DateTime DailyDate { get; set; }

        [Required]
        public DateTime DateOfJoin { get; set; }

        [Required]
        public DateTime DateOfResign { get; set; }

        [Required]
        public Boolean ResignationCheck { get; set; }

        [Required]
        public Boolean AttendanceExemptCheck { get; set; }

        [Required]
        public DateTime? CheckDate { get; set; }

        [Required]
        public Guid RosterId { get; set; }

        [Required]
        public string RosterName { get; set; }

        [Required]
        public DateTime? RosterInn { get; set; }

        [Required]
        public DateTime? RosterOut { get; set; }

        [Required]
        public double RosterLate { get; set; }

        [Required]
        public double RosterEarly { get; set; }

        [Required]
        public double RosterOverTime { get; set; }

        [Required]
        public Double RosterWorkingHours { get; set; }

        [Required]
        public Double RosterWorkingHoursMorning { get; set; }

        [Required]
        public Double RosterWorkingHoursEvening { get; set; }

        [Required]
        public Boolean DayOff { get; set; }

        [Required]
        public Boolean HolidayGazette { get; set; }

        [Required]
        public Boolean SandwitchBefore { get; set; }

        [Required]
        public Boolean SandwitchAfter { get; set; }

        [Required]
        public Boolean HolidayCheck { get; set; }

        [Required]
        public DateTime? OutDate { get; set; }

        public Guid? AttendanceMachineInnId { get; set; }

        public string AttendanceMachineInnName { get; set; }

        public Guid? AttendanceMachineOutId { get; set; }

        public string AttendanceMachineOutName { get; set; }

        [Required]
        public DateTime? InnTime { get; set; }

        [Required]
        public DateTime? OutTime { get; set; }

        [Required]
        public bool LateCheck { get; set; }

        [Required]
        public double LateComing { get; set; }

        [Required]
        public double EarlyGoing { get; set; }

        [Required]
        public bool OverTimeCheck { get; set; }

        [Required]
        public bool OverTimeHolidayCheck { get; set; }

        [Required]
        public double OverTime { get; set; }
        [Required]
        public double OverTimeNight { get; set; }

        [Required]
        public double EarlyOverTime { get; set; }

        [Required]
        public double TotalOverTime { get; set; }

        [Required]
        public double WorkingHours { get; set; }

        [Required]
        public bool AbsentCheck { get; set; }

        [Required]
        public bool AbsentAdjustmentCheck { get; set; }

        [Required]

        public string AbsentAdjustmentCategory { get; set; }

        [Required]
        public string Remarks { get; set; }

    }

}
// using System;
// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;

// namespace TWP_API_Payroll.ViewModels.Payroll {
//     public class VCheckTimeViewModel {
//         [Required]
//         public Guid BranchId { get; set; }

//         [Required]
//         //emppro_id
//         public Guid EmployeeId { get; set; }

//         [Required]
//         //emppro_macid
//         public int MachineId { get; set; }

//         [Required]
//         public string OfficeWorker { get; set; }

//         [Required]
//         public string TemporaryPermanent { get; set; }

//         [Required]
//         public string Gender { get; set; }

//         [Required]
//         public string EmployeeCategoryName { get; set; }

//         [Required]
//         //memp_sub_id
//         public Guid DesignationId { get; set; }

//         [Required]
//         //memp_sub_id
//         public string DesignationName { get; set; }

//         [Required]
//         public Guid DepartmentId { get; set; }

//         [Required]
//         public string DepartmentName { get; set; }

//         [Required]
//         //emppro_ot
//         public bool OverTime { get; set; }

//         [Required]
//         //emppro_lde
//         public bool LateDeduction { get; set; }

//         [Required]
//         //emppro_ho
//         public bool OverTimeHoliday { get; set; }

//         // [Required]
//         // //emppro_st
//         // public bool Active { get; set; }

//         // [Required]
//         // //emppro_attexp
//         // public bool AttendanceExempt { get; set; }

//         [Required]
//         //tbldat_Dat
//         public DateTime DailyDate { get; set; }

//         [Required]
//         //checkdate
//         public DateTime? CheckDate { get; set; }

//         //        [Required]
//         //out_dt
//         public DateTime? OutDate { get; set; }

//         [Required]
//         //ros_nam
//         public string RosterName { get; set; }

//         [Required]
//         //rosgp_ota
//         public double RosterOverTime { get; set; }

//         [Required]
//         //rosgp_lat
//         public double RosterLate { get; set; }

//         [Required]
//         //rosgp_ear
//         public double RosterEarly { get; set; }

//         [Required]
//         //rosgp_earot
//         public double RosterEarlyOverTime { get; set; }

//         [Required]
//         //inn
//         public DateTime? Inn { get; set; }

//         //        [Required]
//         //out
//         public DateTime? Out { get; set; }

//         [Required]
//         //Roster Date
//         public DateTime RosterDate { get; set; }

//         [Required]
//         //inn_out_hrs
//         public double WorkingHours { get; set; }

//         [Required]
//         //rosgp_hrs
//         public double RosterHours { get; set; }

//         [Required]
//         //rosgp_wh
//         public double RosterWorkingHours { get; set; }

//         [Required]
//         //rosgp_in
//         public DateTime RosterInn { get; set; }

//         [Required]
//         //rosgp_out
//         public DateTime RosterOut { get; set; }

//         [Required]
//         //rosgp_mwh
//         public double RosterMorningHours { get; set; }

//         [Required]
//         //rosgp_ewh
//         public double RosterEveningHours { get; set; }

//         [Required]
//         //Early_coming
//         public int EarlyComing { get; set; }

//         [Required]
//         //Early_coming OverTime
//         public int EarlyComingOverTime { get; set; }

//         [Required]
//         //Late_inn
//         public int LateInn { get; set; }

//         [Required]
//         //Early_Going
//         public int EarlyGoing { get; set; }

//         [Required]
//         //Early_Overtime
//         public int EarlyOvertime { get; set; }

//         [Required]
//         //OT
//         public int Overtime { get; set; }

//         [Required]
//         //Totalmin
//         public int TotalMinutes { get; set; }

//         [Required]
//         //mholi_dayact
//         public bool HolidayCheck { get; set; }

//         [Required]
//         //mholi_rmks
//         public string HolidayRemarks { get; set; }

//         [Required]
//         //mholi_dat
//         public DateTime HolidayDate { get; set; }

//         [Required]
//         //remarks
//         public string Remarks { get; set; }

//         [Required]
//         //DayOFF
//         public bool DayOFF { get; set; }

//         [Required]
//         //Holiday_Gazetted
//         public bool HolidayGazetted { get; set; }

//         [Required]
//         //Absent_Approval
//         public bool AbsentApproval { get; set; }

//     }
// }