using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels
{
    public class UserLoginInfoBaseModel
    {

        [Required]
        public string UserId { get; set; }
        public string UserName { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public String CompanyName { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        public String BranchName { get; set; }
        public Guid? YearId { get; set; }
        public String YearName { get; set; }

        public String YearStartDate { get; set; }
        public String YearEndDate { get; set; }
        public bool ckSalesman { get; set; }
        public bool ckDirector { get; set; }
        public Guid? EmployeeId { get; set; }
    }
}