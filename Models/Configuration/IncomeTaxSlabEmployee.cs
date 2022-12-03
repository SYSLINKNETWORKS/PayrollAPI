using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace TWP_API_Payroll.Models {
        [Table ("Cf_IncomeTaxSlabEmployee")]

    public partial class IncomeTaxSlabEmployee {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public double SlabFrom { get; set; }
        [Required]
        public double SlabTo { get; set; }
        [Required]
        public double Percentage { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
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