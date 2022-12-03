using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_AnnualLeaves")]
    public partial class AnnualLeaves {
        [Key]
        public Guid Id { get; set; }
        public DateTime Date { get; set; }

        [StringLength (100)]
        public string Name { get; set; }
        public bool AnnualLeaveAllow { get; set; } = false;
        public int AnnualLeaveDays { get; set; }
        public bool SickLeaveAllow { get; set; } = false;
        public int SickLeaveDays { get; set; }
        public bool CasualLeaveAllow { get; set; } = false;
        public int CasualLeaveDays { get; set; }

        [StringLength (1)]
        public string Type { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public bool Active { get; set; }

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