using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("T_LoanIssue")]
    public partial class LoanIssue
    {

        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public double Amount { get; set; }
        public double NoOfInstalment { get; set; }
        public double InstalmentAmount { get; set; }

        [Required]
        public Guid LoanCategoryId { get; set; }

        [ForeignKey("LoanCategoryId")]
        public LoanCategory LoanCategory { get; set; }

        public bool LoanStatus { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        [StringLength(1000)]
        public string Remarks { get; set; }

        public bool Approved { get; set; } = false;
        public string UserNameApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        public Guid VoucherNo { get; set; }


        [StringLength(1)]
        public string Type { get; set; }

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