using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("Cf_Employee")]
    public partial class Employee
    {

        [Key]
        public Guid Id { get; set; }
        //        public int emppro_id { get; set; }

        [Required]
        public int MachineId { get; set; }

        [Required]
        [StringLength(250)]
        //[Column ("emppro_nam")]
        public string Name { get; set; }

        [Required]
        //[Column ("emppro_fnam")]
        [StringLength(250)]
        public string FatherName { get; set; }

        //        [Column ("emppro_add")]
        public string Address { get; set; }
        //        [Column ("emppro_peradd")]
        [StringLength(250)]
        public string AddressPermanent { get; set; }

        //        [Column ("emppro_doj", TypeName = "datetime")]
        [Required]
        public DateTime DateofJoin { get; set; }

        //        [Column ("emppro_gen")]
        [Required]
        public string Gender { get; set; }

        //[Column ("emppro_mar")]
        [Required]
        public string Married { get; set; }

        //        [Column ("emppro_dob", TypeName = "datetime")]
        [Required]
        public DateTime DateofBirth { get; set; }

        //        [Column ("emppro_cnic")]
        [Required]
        [StringLength(15)]
        public string CNIC { get; set; }

        //        [Column ("emppro_cnicexp", TypeName = "datetime")]
        public DateTime CNICExpire { get; set; }

        //        [Column ("emppro_ntn")]
        [Required]
        [StringLength(20)]
        public string NTN { get; set; }

        //        [Column ("emppro_pho")]
        [StringLength(100)]
        public string Phone { get; set; }

        //      [Column ("emppro_mob")]
        [StringLength(100)]
        public string Mobile { get; set; }

        //        [Column ("emppro_eml")]
        [StringLength(100)]
        public string Email { get; set; }

        //        [Column ("emppro_expcom")]
        [StringLength(100)]
        public string CompanyExpirence { get; set; }

        //[Column ("emppro_expdes")]
        [StringLength(100)]
        public string CompanyExpirenceDescription { get; set; }

        //  [Column ("emppro_expyrfrm", TypeName = "datetime")]
        public DateTime CompanyExpirenceFrom { get; set; }

        //[Column ("emppro_expyrto", TypeName = "datetime")]
        public DateTime CompanyExpirenceTo { get; set; }

        //        [Column ("emppro_exprmk")]
        [StringLength(100)]
        public string CompanyExpirenceRemarks { get; set; }

        // [Column ("emppro_quains")]
        [StringLength(100)]
        public string QualificationInstitute { get; set; }

        //   [Column ("emppro_quaqua")]
        [StringLength(50)]
        public string Qualification { get; set; }

        // [Column ("emppro_quayr", TypeName = "datetime")]
        public DateTime QualificationYear { get; set; }

        //        [Column ("emppro_quarmk")]
        [StringLength(100)]
        public string QualificationRemarks { get; set; }

        //      [Column ("emppro_sal")]
        //        public double Salary { get; set; }

        //[Column ("emppro_salgra")]

        public bool Gratuity { get; set; } = false;

        //        [Column ("emppro_saleobi")]
        public bool EOBI { get; set; } = false;
        //[Column ("emppro_saleobi_dor", TypeName = "datetime")]
        public DateTime EOBIRegistrationDate { get; set; }

        // [Column ("emppro_saleobi_reg")]
        [StringLength(50)]
        public string EOBIRegistrationNo { get; set; }

        //[Column ("emppro_salsessi")]
        public bool SESSI { get; set; } = false;
        //      [Column ("emppro_dat_sessi", TypeName = "datetime")]
        public DateTime SESSIRegistrationDate { get; set; }

        //  [Column ("emppro_sessi_no")]
        public string SESSIRegistrationNo { get; set; }

        //  [Column ("emppro_salsp")]
        public bool StopPayment { get; set; } = false;

        //  [Column ("emppro_reg")]
        public bool ResignationCheck { get; set; } = false;

        //[Column ("emppro_reg_dat", TypeName = "datetime")]
        public DateTime ResignationDate { get; set; }

        //[Column ("emppro_reg_rmk")]
        [StringLength(100)]
        public string ResignationRemarks { get; set; }

        // [Column ("emppro_img", TypeName = "image")]
        // public byte[] EmpproImg { get; set; }

        //  [Column ("emppro_salpay")]
        // [StringLength (1)]
        public string ModeOfPayment { get; set; } = "Cash";

        // [Column ("emppro_salpay_acc")]
        [StringLength(100)]
        public string SalaryAccount { get; set; }

        //[Column ("emppro_ot")]
        public bool OverTime { get; set; } = false;

        //        [Column ("emppro_ho")]
        public bool OverTimeHoliday { get; set; }

        //      [Column ("emppro_rat")]
        public double OverTimeRate { get; set; }

        //      [Column ("emppro_sot")]
        public bool OvertimeSaturday { get; set; } = false;

        //        [Column ("emppro_fot")]
        public bool OverTimeFactory { get; set; } = false;

        // [Column ("emppro_salpt")]
        // public double? EmpproSalpt { get; set; }

        // [Column ("emppro_salpot")]
        // public double? EmpproSalpot { get; set; }

        // [Column ("msal_id")]
        // public int? MsalId { get; set; }

        // [Column ("emppro_userid")]
        // public int? EmpproUserid { get; set; }

        //        [Column ("emppro_lde")]
        public bool LateDeduction { get; set; } = false;

        //[Column ("emppro_att")]
        public bool AttendanceAllowance { get; set; } = false;
        // [Column ("emppro_attexp")]
        public bool AttendanceExempt { get; set; } = false;

        //        [Column ("emppro_ckrat")]
        public bool OverTimeRateCheck { get; set; } = false;

        //  [Column ("emppro_autact")]
        public bool DocumentAuthorize { get; set; } = false;

        //        [Column ("emppro_tp")]
        //        [StringLength (1)]
        public string TemporaryPermanent { get; set; }

        //[Column ("emppro_srat")]
        public double OverTimeSpecialRate { get; set; }

        // [Column ("mtermres_id")]
        // public Guid MtermresId { get; set; }

        // [Column ("emppro_restyp")]
        // [StringLength (1)]
        // public string EmpproRestyp { get; set; }

        // [Column ("log_act")]
        // [StringLength (1)]
        // public string LogAct { get; set; }

        // [Column ("usr_id_ins")]
        // [StringLength (2)]
        // public string UsrIdIns { get; set; }

        // [Column ("ins_dat", TypeName = "datetime")]
        // public DateTime? InsDat { get; set; }

        // [Column ("rosemp_id")]
        // public int? RosempId { get; set; }

        // [Column ("acc_no")]
        // public int? AccNo { get; set; }

        // [Column ("usr_id_upd")]
        // [StringLength (2)]
        // public string UsrIdUpd { get; set; }

        // [Column ("upd_dat", TypeName = "datetime")]
        // public DateTime? UpdDat { get; set; }

        //[Column ("emppro_incometax")]
        public bool IncomeTax { get; set; } = false;

        //        [Column ("emppro_period")]
        public int ProvisionPeriod { get; set; }

        //        [Column ("emppro_dop", TypeName = "datetime")]
        public DateTime DateofParmanent { get; set; }

        //[Column ("emppro_emergcontone")]
        [StringLength(100)]
        public string EmergencyContactOne { get; set; }

        //       [Column ("emppro_emergconttwo")]
        [StringLength(100)]
        public string EmergencyContactTwo { get; set; }

        //[Column ("emppro_rmk")]
        [StringLength(1000)]
        public string Remarks { get; set; }

        // [Column ("emppro_takaful")]
        public double TakafulRate { get; set; }

        //        [Column ("emppro_ow")]
        //      [StringLength (1)]
        public string OfficeWorker { get; set; }

        //[Column ("emppro_ref")]
        [StringLength(250)]
        public string ReferenceOne { get; set; }

        //        [Column ("emppro_refcnicone")]
        [StringLength(15)]
        public string ReferenceCNICOne { get; set; }

        //  [Column ("emppro_refaddone")]
        [StringLength(250)]
        public string ReferenceAddressOne { get; set; }

        // [Column ("emppro_refconone")]
        [StringLength(100)]
        public string ReferenceContactOne { get; set; }

        //        [Column ("emppro_reftwo")]
        [StringLength(250)]
        public string ReferenceTwo { get; set; }
        //  [Column ("emppro_refcnictwo")]
        [StringLength(15)]
        public string ReferenceCNICTwo { get; set; }

        //  [Column ("emppro_refaddtwo")]
        [StringLength(250)]
        public string ReferenceAddressTwo { get; set; }

        //   [Column ("emppro_refcontwo")]
        [StringLength(100)]
        public string ReferenceContactTwo { get; set; }

        [Required]
        public Guid BranchId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        //[Column ("emppro_cat")]
        public Guid EmployeeCategoryId { get; set; }
        // Navigation Property
        [ForeignKey("EmployeeCategoryId")]

        public EmployeeCategory employeeCategory { get; set; }

        [Required]
        //[Column ("memp_sub_id")]
        public Guid DesignationId { get; set; }
        // Navigation Property
        [ForeignKey("DesignationId")]
        public Designation designation { get; set; }

        [Required]
        //[Column ("dpt_id")]
        public Guid DepartmentId { get; set; }
        // Navigation Property
        [ForeignKey("DepartmentId")]
        public Department department { get; set; }

        [Required]
        //[Column ("anl_id")]
        public Guid AnnualLeavesId { get; set; }
        // Navigation Property
        [ForeignKey("AnnualLeavesId")]
        public AnnualLeaves annualLeaves { get; set; }

        // [Column ("ros_id")]
        [Required]
        public Guid RosterId { get; set; }
        // Navigation Property
        [ForeignKey("RosterId")]
        public Roster roster { get; set; }

        [Required]
        public Guid ReportOfficerId { get; set; }

        public bool Approved { get; set; } = false;
        public string UserNameApproved { get; set; }
        public DateTime? DateApproved { get; set; }
        //[Column ("emppro_typ")]
        [StringLength(1)]
        public string Type { get; set; }
        //[Column ("emppro_st")]
        [Required]
        public bool Active { get; set; }

        [Required]
        [StringLength(1)]
        public string Action { get; set; }
        public string UserNameInsert { get; set; }
        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;

        public string UserNameUpdate { get; set; }

        [Required]
        public DateTime UpdateDate { get; set; } = DateTime.Now;

        public string UserNameDelete { get; set; }

        [Required]
        public DateTime DeleteDate { get; set; } = DateTime.Now;

    }
}