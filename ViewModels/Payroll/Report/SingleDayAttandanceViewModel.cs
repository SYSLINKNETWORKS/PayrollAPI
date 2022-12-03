using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class SingleDayAttandanceBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }
        public List<SingleDayAttandance> SingleDayAttandances { get; set; }
    }
    public class SingleDayAttandance {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeTemporaryPermanent { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public string EmployeeCategory { get; set; }

        [Required]
        public DateTime DailyDate { get; set; }

        public DateTime? OutDate { get; set; }

        [Required]
        public DateTime? CheckDate { get; set; }

        [Required]
        public DateTime? InnTime { get; set; }

        [Required]
        public DateTime? OutTime { get; set; }

        [Required]
        public string Remarks { get; set; }
    }
    

}