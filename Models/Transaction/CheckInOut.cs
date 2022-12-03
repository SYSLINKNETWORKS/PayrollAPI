using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("T_CheckInOut")]
    public partial class CheckInOut {
        [Key]
        [Required]
        //        [Column("USERID")]
        public int MachineId { get; set; }

        [Key]
        [Required]
        //      [Column("CHECKTIME", TypeName = "datetime")]
        public DateTime CheckTime { get; set; }
        //        [Column("CHECKTYPE")]

        [Required]
        //public DateTime Date { get { return Date; } private set { Convert.ToDateTime (CheckTime.ToString ("yyyy-mm-dd")); } }
        public DateTime Date { get; set; }

        [Required]
        [StringLength (1)]
        public string CheckType { get; set; }
        //      [Column("VERIFYCODE")]
        public int? VerifyCode { get; set; }
        //    [Column("SENSORID")]
        [StringLength (5)]
        public string SensorId { get; set; }
        //  [Column("ckinout_st")]
        public bool Status { get; set; } = false;

        //  [Column("check_typ")]
        [StringLength (1)]
        public string Type { get; set; }
        //        [Column("check_app")]
        public bool Approved { get; set; } = false;
        // [Column ("inoutcat_id")]
        // public int? InoutcatId { get; set; }
        //        [Column("check_night")]
        // public bool NightCheck { get; set; } = false;
        //        [Column("checkdate", TypeName = "datetime")]
        //[Column("check_rmk")]
        [StringLength (1000)]
        public string Remarks { get; set; }

        // [Column ("mac_id")]
        // public int? MacId { get; set; }
        //        [Column("latitude")]
        public double Latitude { get; set; }
        // [Column("longitude")]
        public double Longitude { get; set; }
        //[Column("address")]
        public string Address { get; set; }

        public Guid? AttendanceMachineId { get; set; }
        // Navigation Property
        [ForeignKey ("AttendanceMachineId")]
        public AttendanceMachine attendanceMachine { get; set; }

        public Guid? InOutCategoryId { get; set; }
        // Navigation Property
        [ForeignKey ("InOutCategoryId")]
        public InOutCategory inOutCategory { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        [Required]
        [StringLength (1)]
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