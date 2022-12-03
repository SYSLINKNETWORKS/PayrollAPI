using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("Cf_AttendanceMachine")]
    public partial class AttendanceMachine
    {

        [Key]
        public Guid Id { get; set; }

        [StringLength(250)]
        public string Name { get; set; }

        [StringLength(100)]
        public string Ip { get; set; }
        public int Port { get; set; }

        // [Column("mac_typ")]
        // [StringLength(1)]
        // public string MacTyp { get; set; }
        //        [Column("mac_cat")]
        [StringLength(100)]
        public string Category { get; set; }

        public Guid AttendanceMachineCategoryId { get; set; }
        // Navigation Property
        [ForeignKey("AttendanceMachineCategoryId")]
        public AttendanceMachineCategory attendanceMachineCategory { get; set; }

        public Guid AttendanceMachineGroupId { get; set; }
        // Navigation Property
        [ForeignKey("AttendanceMachineGroupId")]
        public AttendanceMachineGroup attendanceMachineGroup { get; set; }

        [Required]
        [StringLength(1)]
        public string Type { get; set; }    

        public Guid CompanyId { get; set; }

        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(1)]
        public string Action { get; set; }

        public string UserNameInsert { get; set; }

        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;

        public string UserNameUpdate { get; set; }
        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public string UserNameDelete { get; set; }
        [Required]
        public DateTime DeleteDate { get; set; } = DateTime.Now;

    }
}