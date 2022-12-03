using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_EmployeeAllowances")]
    public partial class EmployeeAllowances {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
        // Navigation Property
        [ForeignKey ("EmployeeId")]
        public Employee employee { get; set; }

        [Required]
        public Guid AllowanceId { get; set; }
        // Navigation Property
        [ForeignKey ("AllowanceId")]
        public Allowance allowance { get; set; }

    }
}