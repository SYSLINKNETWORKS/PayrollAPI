using System;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class RosterGroupBaseModel
    {

    }
    public class RosterGroupFoundationModel : RosterGroupBaseModel
    {

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public Guid RosterId { get; set; }

        [Required]
        public double Overtime { get; set; }

        [Required]
        public double WorkingHours { get; set; }

        [Required]
        public double Late { get; set; }

        [Required]
        public double EarlyGoing { get; set; }

        [Required]
        public double EarlyOvertime { get; set; }

        [Required]
        public int MorningWorkingHours { get; set; }
        [Required]
        public int EveningWorkingHours { get; set; }

        [Required]
        public bool MondayCheck { get; set; }

        [Required]
        public DateTime MondayInn { get; set; }


        [Required]
        public DateTime MondayOut { get; set; }

        [Required]
        public bool TuesdayCheck { get; set; }

        [Required]
        public DateTime TuesdayInn { get; set; }


        [Required]
        public DateTime TuesdayOut { get; set; }

        [Required]
        public bool WednesdayCheck { get; set; }

        [Required]
        public DateTime WednesdayInn { get; set; }


        [Required]
        public DateTime WednesdayOut { get; set; }

        [Required]
        public bool ThursdayCheck { get; set; }

        [Required]
        public DateTime ThursdayInn { get; set; }


        [Required]
        public DateTime ThursdayOut { get; set; }

        [Required]
        public bool FridayCheck { get; set; }

        [Required]
        public DateTime FridayInn { get; set; }


        [Required]
        public DateTime FridayOut { get; set; }

        [Required]
        public bool SaturdayCheck { get; set; }

        [Required]
        public DateTime SaturdayInn { get; set; }


        [Required]
        public DateTime SaturdayOut { get; set; }

        [Required]
        public bool SundayCheck { get; set; }

        [Required]
        public DateTime SundayInn { get; set; }


        [Required]
        public DateTime SundayOut { get; set; }
        


        [Required]
        [StringLength(1)]
        public string Type { get; set; }

        public bool Active { get; set; }

    }

    public class RosterGroupViewModel : RosterGroupFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        


        [Required]
        public string Roster { get; set; }

        
        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class RosterGroupViewByIdModel : RosterGroupFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string RosterName { get; set; }
    }
    public class RosterGroupAddModel : RosterGroupFoundationModel
    {

        [Required]
        public Guid Menu_Id { get; set; }

    }
    public class RosterGroupUpdateModel : RosterGroupFoundationModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class RosterGroupDeleteModel : RosterGroupBaseModel
    {

        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }

}