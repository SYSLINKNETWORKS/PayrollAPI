using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_Designation")]
    public partial class Designation {

        [Key]
        public Guid Id { get; set; } = new Guid ();

        [Required]
        [StringLength (250)]
        public string Name { get; set; }

        [Required]
        public bool Director { get; set; } = false;

        [Required]
        public bool Salesman { get; set; } = false;
        [Required]
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
        // Navigation Property

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