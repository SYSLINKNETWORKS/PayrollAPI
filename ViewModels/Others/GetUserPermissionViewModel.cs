using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels
{
    public class GetUserPermissionViewModel
    {
        public string UserId { get; set; }

        public string UserName { get; set; }


        [Required]
        public Guid MenuId { get; set; }


        [Required]
        public string MenuName { get; set; }
        [Required]
        public string MenuAlias { get; set; }

        [Required]
        public bool View_Permission { get; set; }

        [Required]
        public bool Insert_Permission { get; set; }

        [Required]
        public bool Update_Permission { get; set; }

        [Required]
        public bool Delete_Permission { get; set; }
        [Required]
        public bool Print_Permission { get; set; }

        [Required]
        public bool Check_Permission { get; set; }

        [Required]
        public bool Approve_Permission { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        [Required]
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        [Required]
        public Guid FinancialYearId { get; set; }
        public string FinancialYearName { get; set; }
        public DateTime YearStartDate { get; set; }
        public DateTime YearEndDate { get; set; }
        public DateTime PermissionDateFrom { get; set; }
        public DateTime PermissionDateTo { get; set; }
        public Guid? EmployeeId { get; set; }
        public bool ckSalesman { get; set; }
        public bool ckDirector { get; set; }

    }
}