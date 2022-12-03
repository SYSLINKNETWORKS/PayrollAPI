using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models {
    [Table ("Cf_RosterGroup")]
    public partial class RosterGroup {
        [Key]
        //        [Column("rosgp_id")]
        public Guid Id { get; set; }
        //        [Column("rosgp_dat", TypeName = "datetime")]
        public DateTime Date { get; set; }
        //      [Column("rosgp_nam")]
        //    [StringLength(100)]
        //  public string RosgpNam { get; set; }
        // [Column("rosgp_in", TypeName = "datetime")]
        //        public DateTime RosgpIn { get; set; }
        //      [Column("rosgp_out", TypeName = "datetime")]
        //    public DateTime? RosgpOut { get; set; }
        //   [Column("rosgp_ota")]
        public double OverTime { get; set; }
        //        [Column("rosgp_wh")]
        public double WorkingHours { get; set; }
        // [Column("rosgp_typ")]
        // [StringLength(1)]
        // public string RosgpTyp { get; set; }
        //        [Column("rosgp_lat")]
        public double Late { get; set; }
        //        [Column("rosgp_ear")]
        public double EarlyGoing { get; set; }

        //        [Column ("rosgp_earot")]
        public double EarlyOvertime { get; set; }

        //        [Column ("rosgp_mwh")]
        public int MorningWorkingHours { get; set; }

        //      [Column ("rosgp_ewh")]
        public int EveningWorkingHours { get; set; }

        //        [Column ("rosgp_ck_mon")]
        public bool MondayCheck { get; set; } = false;
        //        [Column ("rosgp_in_mon", TypeName = "datetime")]
        public DateTime MondayInn { get; set; }

        //        [Column ("rosgp_out_mon", TypeName = "datetime")]
        public DateTime MondayOut { get; set; }

       // [Column ("rosgp_mon_wh")]
        public int MondayWorkingHours { get; set; }

        //        [Column ("rosgp_ck_tue")]
        public bool TuesdayCheck { get; set; } = false;

        //        [Column ("rosgp_in_tue", TypeName = "datetime")]
        public DateTime TuesdayInn { get; set; }

      //  [Column ("rosgp_out_tue", TypeName = "datetime")]
        public DateTime TuesdayOut { get; set; }

       // [Column ("rosgp_tue_wh")]
        public int TuesdayWorkingHours { get; set; }
        //        [Column ("rosgp_ck_wed")]
        public bool WednesdayCheck { get; set; } = false;

        //        [Column ("rosgp_in_wed", TypeName = "datetime")]
        public DateTime WednesdayInn { get; set; }

        //        [Column ("rosgp_out_wed", TypeName = "datetime")]
        public DateTime WednesdayOut { get; set; }

        //        [Column ("rosgp_wed_wh")]
        public int WednesdayWorkingHours { get; set; }

        //        [Column ("rosgp_ck_thu")]
        public bool ThursdayCheck { get; set; } = false;

        //        [Column ("rosgp_in_thu", TypeName = "datetime")]
        public DateTime ThursdayInn { get; set; }

        //        [Column ("rosgp_out_thu", TypeName = "datetime")]
        public DateTime ThursdayOut { get; set; }

        //        [Column ("rosgp_thu_wh")]
        public int ThursdayWorkingHours { get; set; }

        //        [Column ("rosgp_ck_fri")]
        public bool FridayCheck { get; set; } = false;

        //        [Column ("rosgp_in_fri", TypeName = "datetime")]
        public DateTime FridayInn { get; set; }

        //        [Column ("rosgp_out_fri", TypeName = "datetime")]
        public DateTime FridayOut { get; set; }

        //        [Column ("rosgp_fri_wh")]
        public int FridayWorkingHours { get; set; }

        //        [Column ("rosgp_ck_sat")]
        public bool SaturdayCheck { get; set; } = false;

        //        [Column ("rosgp_in_sat", TypeName = "datetime")]
        public DateTime SaturdayInn { get; set; }

        //      [Column ("rosgp_out_sat", TypeName = "datetime")]
        public DateTime SaturdayOut { get; set; }

        //      [Column ("rosgp_sat_wh")]
        public int SaturdayWorkingHours { get; set; }

        //        [Column ("rosgp_ck_sun")]
        public bool SundayCheck { get; set; } = false;

        // [Column ("rosgp_in_sun", TypeName = "datetime")]
        public DateTime SundayInn { get; set; }

        // [Column ("rosgp_out_sun", TypeName = "datetime")]
        public DateTime SundayOut { get; set; }

        //  [Column ("rosgp_sun_wh")]
        public int SundayWorkingHours { get; set; }

        //        [Column("ros_id")]
        public Guid RosterId { get; set; }
        // Navigation Property
        [ForeignKey ("RosterId")]
        public Roster roster { get; set; }

        [Required]
        [StringLength (1)]
        public string Type { get; set; }

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