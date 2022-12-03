using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("T_CheckAttendance")]
    public partial class CheckAttendance {
        [Required]
        [Key, Column (Order = 0)]
        public Guid EmployeeId { get; set; }
        // Navigation Property
        [ForeignKey ("EmployeeId")]
        public Employee employee { get; set; }

        [Required]
        [Key, Column (Order = 1)]
        public DateTime Date { get; set; }
        public DateTime Inn { get; set; }
        public DateTime? Out { get; set; }
        //[Column("CheckAttendance_min")]
        [Required]
        public int Minutes { get; set; }
        public bool Approved { get; set; } = false;
        public Guid? AttendanceMachineIdInn { get; set; }
        // Navigation Property
        [ForeignKey ("AttendanceMachineIdInn")]
        public AttendanceMachine attendanceMachineInn { get; set; }
        public Guid? AttendanceMachineIdOut { get; set; }
        // Navigation Property
        [ForeignKey ("AttendanceMachineIdOut")]
        public AttendanceMachine attendanceMachineOut { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public string UserNameInsert { get; set; }

        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;

    }
}