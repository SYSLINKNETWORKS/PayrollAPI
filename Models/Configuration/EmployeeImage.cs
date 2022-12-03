using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_EmployeeImage")]
    public partial class EmployeeImage {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public bool ImageProfileCheck { get; set; }

        [Required]
        public string ImageName { get; set; }

        [Required]
        public string ImageBytes { get; set; }

        [Required]
        public string ImageExtension { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }
        // Navigation Property
        [ForeignKey ("EmployeeId")]
        public Employee employee { get; set; }
    }
}