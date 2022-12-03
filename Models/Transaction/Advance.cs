using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("T_Advance")]
    public partial class Advance
    {

        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public DateTime Date { get; set; }


        public double Amount { get; set; }
        

        [Required]
        public Guid EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public bool Approved { get; set; } = false;
        public string UserNameApproved { get; set; }
        public DateTime? DateApproved { get; set; }

        [StringLength(1)]
        public string Type { get; set; }


        public Guid VoucherNo { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        
        [Required]
        [StringLength(1)]
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