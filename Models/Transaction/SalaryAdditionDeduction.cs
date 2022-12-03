using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("T_SalaryAdditionDeduction")]
    public partial class SalaryAdditionDeduction
    {

        [Key]
        public Guid Id { get; set; } = new Guid();

        [Required]
        public DateTime Date { get; set; }


        public double AdditionAmount { get; set; }
        public double DeductionAmount { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
        
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }


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