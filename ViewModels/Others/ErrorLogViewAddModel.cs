using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels
{
    public class ErrorLogViewAddModel
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string CompanyId { get; set; }

        [Required]
        public string BranchId { get; set; }

        [Required]
        public string UserId { get; set; }


        [Required]
        public string ErrorId { get; set; }

        [Required]
        public string Description { get; set; }
        [Required]
        public string StackTrace { get; set; }

        [Required]
        public string Type { get; set; }


    }
}