using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class TimeSheetViewModelBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<TimeSheetViewModel> TimeSheetViewModels { get; set; }
    }
    public class TimeSheetViewModel
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
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public DateTime DailyDate { get; set; }

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
        public Boolean HolidayCheck { get; set; }

        // [Required]
        // public DateTime? CheckDate { get; set; }

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