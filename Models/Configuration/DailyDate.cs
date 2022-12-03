using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_DailyDate")]
    public partial class DailyDate {
        //        [Column("DailyDate_dat", TypeName = "datetime")]
        [Key]
        public DateTime Date { get; set; }
        //        [Column("DailyDate_hol")]
        public bool HolidayCheck { get; set; }

        [StringLength (250)]
        public string Remarks { get; set; }
    }
}