using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;


namespace TWP_API_Payroll.Models {
    [Table ("T_Absent")]
    public partial class Absent {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        //        [Column("Absents_app")]
        public bool Approved { get; set; } = false;
        public bool ApprovedAdjust { get; set; } = false;
        //        [Column("Absents_leave")]
        [StringLength (1)]
        public string ApprovedAdjustType { get; set; }

        //        [Column ("emppro_id")]
        [Required]
        public Guid EmployeeId { get; set; }
        // Navigation Property
        [ForeignKey ("EmployeeId")]
        public Employee employee { get; set; }

        public string Type { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength (1)]
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