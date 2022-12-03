using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class DailyAttendanceBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<DailyAttendanceLists> DailyAttendanceLists { get; set; }
    }
    public class DailyAttendanceLists {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

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
        public DateTime? OutDate { get; set; }

        [Required]
        public DateTime? InnTime { get; set; }

        [Required]
        public DateTime? OutTime { get; set; }

        [Required]
        public double LateComingMin { get; set; }

        [Required]
        public double LateComingHr { get; set; }

        [Required]
        public double OverTimeMin { get; set; }

        [Required]
        public double OverTimehr { get; set; }
        [Required]
        public double OverTimeNightMin { get; set; }

        [Required]
        public double OverTimeNighthr { get; set; }

        [Required]
        public double TotalOverTime { get; set; }

        [Required]
        public double WorkingHours { get; set; }

        [Required]
        public string Remarks { get; set; }

    }

}