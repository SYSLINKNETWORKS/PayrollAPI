using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("T_NightOverTime")]
    public partial class NightOverTime
    {
        [Key]
        [Required]
        public Guid EmployeeId { get; set; }
        // Navigation Property
        [ForeignKey("EmployeeId")]
        public Employee employee { get; set; }
        [Key]
        [Required]
        public DateTime Date { get; set; }
        public double OverTime { get; set; }


        [StringLength(1)]
        public string Type { get; set; }
        //        [Column("check_app")]
        public bool Approved { get; set; } = false;
        public string UserNameApproved { get; set; }
        public DateTime? DateApproved { get; set; }

        public string Remarks { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

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