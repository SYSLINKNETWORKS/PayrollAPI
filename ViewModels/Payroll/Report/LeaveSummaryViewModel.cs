using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class LeaveSummaryBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }

        public List<LeaveSummaryList> LeaveSummaryList { get; set; }
    }
    public class LeaveSummaryList {
        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        
        [Required]
        public int AnnualLeave { get; set; }

        [Required]
        public int AnnualLeaveAvail { get; set; }

        [Required]
        public int balAnnualLeave { get; set; }

        [Required]
        public int SickLeave { get; set; }

        [Required]
        public int SickLeaveAvail { get; set; }

        [Required]
        public int balSickLeave { get; set; }

        [Required]
        public int 	CasualLeave { get; set; }

        [Required]
        public int 	CasualLeaveAvail { get; set; }

        [Required]
        public int balCasualLeave { get; set; }


        
    }
    

}