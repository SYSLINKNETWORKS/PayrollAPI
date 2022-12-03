using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Report
{
    public class EmployeeProfileSummaryBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }
        public DateTime DailyDate { get; set; }

        public List<EmployeeProfileSummaryList> EmployeeProfileSummaryLists { get; set; }
    }
    public class EmployeeProfileSummaryList
    {

        //Prsonal

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public int EmployeeNo { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string FatherName { get; set; }

        [Required]
        public string AddressPermanent { get; set; }

        [Required]
        public string PresentAddress { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Mobile { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        public string CNIC { get; set; }

        [Required]
        public DateTime CNICExpire { get; set; }

        [Required]
        public double CNICExpireDays { get; set; }

        [Required]
        public string NTN { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Married { get; set; }
        public string RosterName { get; set; }
        public string Remarks { get; set; }

        // Employee Detail

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public string EmployeeCategory { get; set; }

        [Required]
        public DateTime DateofJoin { get; set; }

        [Required]
        public double BasicSalary { get; set; }

        [Required]
        public double Allowance { get; set; }

        [Required]
        public double GrossSalary { get; set; }

        [Required]
        public Guid AnnualLeaveId { get; set; }

        [Required]
        public double SickLeave { get; set; }

        [Required]
        public double CasualLeave { get; set; }

        [Required]
        public double AnnualLeave { get; set; }

        // Others Detail

        // Reference one
        public string ReferenceOneName { get; set; }

        public string ReferenceOneCNIC { get; set; }

        public string ReferenceOneAddress { get; set; }

        public string ReferenceOneContact { get; set; }

        // Reference Two
        public string ReferenceTwoName { get; set; }

        public string ReferenceTwoCNIC { get; set; }

        public string ReferenceTwoAddress { get; set; }

        public string ReferenceTwoContact { get; set; }
        public Int32 AttachDocuments { get; set; }

    }

}