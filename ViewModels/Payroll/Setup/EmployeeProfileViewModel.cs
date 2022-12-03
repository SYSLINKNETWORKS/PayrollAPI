using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class EmployeeProfileBaseModel
    {

    }
    public class EmployeeProfileFoundationModel : EmployeeProfileBaseModel
    {

        [Required]
        public int MachineId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        //[Column ("emppro_fnam")]
        [StringLength(250)]
        public string FatherName { get; set; }

        //        [Column ("emppro_add")]
        public string Address { get; set; }

        [StringLength(250)]
        public string AddressPermanent { get; set; }

        [Required]
        public DateTime DateofJoin { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Married { get; set; }

        [Required]
        public DateTime DateofBirth { get; set; }

        [Required]
        [StringLength(15)]
        public string CNIC { get; set; }

        public DateTime CNICExpire { get; set; }

        [Required]
        [StringLength(20)]
        public string NTN { get; set; }

        [StringLength(100)]
        public string Phone { get; set; }

        [StringLength(100)]
        public string Mobile { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100)]
        public string CompanyExpirence { get; set; }

        [StringLength(100)]
        public string CompanyExpirenceDescription { get; set; }

        public DateTime CompanyExpirenceFrom { get; set; }

        public DateTime CompanyExpirenceTo { get; set; }

        [StringLength(100)]
        public string CompanyExpirenceRemarks { get; set; }

        [StringLength(100)]
        public string QualificationInstitute { get; set; }

        [StringLength(50)]
        public string Qualification { get; set; }

        public DateTime QualificationYear { get; set; }

        [StringLength(100)]
        public string QualificationRemarks { get; set; }

        public bool Gratuity { get; set; } = false;

        public DateTime EOBIRegistrationDate { get; set; }

        [StringLength(50)]
        public string EOBIRegistrationNo { get; set; }

        public DateTime SESSIRegistrationDate { get; set; }

        public string SESSIRegistrationNo { get; set; }

        public bool StopPayment { get; set; } = false;

        public bool ResignationCheck { get; set; } = false;

        public DateTime ResignationDate { get; set; }

        [StringLength(100)]
        public string ResignationRemarks { get; set; }

        public string ModeOfPayment { get; set; } = "Cash";

        [StringLength(100)]
        public string SalaryAccount { get; set; }

        public bool OverTime { get; set; } = false;

        public bool OverTimeHoliday { get; set; }

        public double OverTimeRate { get; set; }

        public bool OverTimeFactory { get; set; } = false;

        public bool LateDeduction { get; set; } = false;

        public bool AttendanceAllowance { get; set; } = false;

        public bool AttendanceExempt { get; set; } = false;

        public bool OverTimeRateCheck { get; set; } = false;

        public bool DocumentAuthorize { get; set; } = false;

        public string TemporaryPermanent { get; set; }

        public bool IncomeTax { get; set; } = false;

        public int ProvisionPeriod { get; set; }

        public DateTime DateofParmanent { get; set; }

        [StringLength(100)]
        public string EmergencyContactOne { get; set; }

        [StringLength(100)]
        public string EmergencyContactTwo { get; set; }

        [StringLength(1000)]
        public string Remarks { get; set; }

        public double TakafulRate { get; set; }

        public string OfficeWorker { get; set; }

        [StringLength(250)]
        public string ReferenceOne { get; set; }

        [StringLength(15)]
        public string ReferenceCNICOne { get; set; }

        [StringLength(250)]
        public string ReferenceAddressOne { get; set; }

        [StringLength(100)]
        public string ReferenceContactOne { get; set; }

        [StringLength(250)]
        public string ReferenceTwo { get; set; }

        [StringLength(15)]
        public string ReferenceCNICTwo { get; set; }

        [StringLength(250)]
        public string ReferenceAddressTwo { get; set; }

        [StringLength(100)]
        public string ReferenceContactTwo { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public Guid EmployeeCategoryId { get; set; }

        [Required]
        public Guid DesignationId { get; set; }

        [Required]
        public Guid DepartmentId { get; set; }

        [Required]
        public Guid AnnualLeavesId { get; set; }

        [Required]
        public Guid RosterId { get; set; }

        [Required]
        [StringLength(1)]
        public string Type { get; set; }

        public bool Active { get; set; }
        [Required]
        public Guid ReportOfficerId { get; set; }

    }

    public class EmployeeProfileViewModel : EmployeeProfileFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public int MachineIdMax { get; set; }
        public string ReportOfficerName { get; set; }


        [Required]
        public string Department { get; set; }

        [Required]
        public string Designation { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class EmployeeProfileViewByIdModel : EmployeeProfileFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string BranchName { get; set; }

        [Required]
        public string EmployeeCategoryName { get; set; }

        [Required]
        public string DesignationName { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        [Required]
        public string AnnualLeavesName { get; set; }

        [Required]
        public string RosterName { get; set; }
        public string ReportOfficerName { get; set; }


        public List<EmployeeProfileDocumentList> EmployeeProfileDocumentLists { get; set; }
    }
    public class EmployeeProfileAddModel : EmployeeProfileFoundationModel
    {


        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class EmployeeProfileUpdateModel : EmployeeProfileFoundationModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class EmployeeProfileDeleteModel : EmployeeProfileBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}