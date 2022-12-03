using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report {
    public class EmployeeProfileDocumentReportBaseModel {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        public List<EmployeeProfileDocumentReportList> EmployeeProfileDocumentReportLists { get; set; }
    }
    public class EmployeeProfileDocumentReportList {

        //Prsonal

        [Required]
        public string ImageName { get; set; }

        [Required]
        public string ImageBytes { get; set; }

    }

}