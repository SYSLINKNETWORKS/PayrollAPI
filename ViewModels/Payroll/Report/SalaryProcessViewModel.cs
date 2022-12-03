using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class SalaryProcessViewModelBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<SalaryProcessViewModel> SalaryProcessViewModels { get; set; }
    }
    public class SalaryProcessViewModel
    {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public Guid DesignationId { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public DateTime DateOfJoining { get; set; }

        [Required]
        public double SalaryAmount { get; set; }

        [Required]
        public double SalaryAllowanceAmount { get; set; }

        [Required]
        public double SalaryGrossAmount { get; set; }

        [Required]
        public int NoOfDaysMonth { get; set; }

        [Required]
        public double LateHours { get; set; }

        [Required]
        public double LateDays { get; set; }

        [Required]
        public double LateDaysTotal { get; set; }

        [Required]
        public int PresentDays { get; set; }

        [Required]
        public int AbsentDays { get; set; }

        [Required]
        public double WorkingDays { get; set; }

        [Required]
        public double AttendanceAllowanceAmount { get; set; }

        [Required]
        public Double AdditionAmount { get; set; }

        [Required]
        public double DeductionAmount { get; set; }

        [Required]
        public double LateDaysActual { get; set; }

        [Required]
        public double LateDaysActualAmount { get; set; }

        [Required]
        public double Takaful { get; set; }

        [Required]
        public double AdvanceAmount { get; set; }

        [Required]
        public double LoanAmount { get; set; }

        [Required]
        public double IncomeTaxAmount { get; set; }

        [Required]
        public double GrossAmount { get; set; }

        [Required]
        public double OvertimeActual { get; set; }

        [Required]
        public double OvertimeActualAmount { get; set; }

        [Required]
        public double PayableAmount { get; set; }
        public bool VoucherPostCk{get;set;}

    }

}