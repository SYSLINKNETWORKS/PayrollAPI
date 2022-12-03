using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll

{
    public class HolidayBaseModel
    {

    }
    public class HolidayFoundationModel : HolidayBaseModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool Holidaycheck { get; set; }

        [Required]
        public bool FactorOverTime { get; set; }

        [Required]
        public string Remarks { get; set; }
        public string Type { get; set; }
        

    }

    public class HolidayViewModel : HolidayFoundationModel
    {
        
        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class HolidayViewByIdModel : HolidayFoundationModel
    {
       
        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    
    public class HolidayUpdateModel : HolidayFoundationModel
    {
        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class HolidayDeleteModel : HolidayBaseModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public Guid Menu_Id { get; set; }
    }
    public class HolidayAddModel {
        public List<HolidayListAddModel> HolidayListAddModel { get; set; }

    }
    public class HolidayListAddModel {
        public string Date { get; set; }
        public string HolidayCheck { get; set; }
        public string Remarks { get; set; }
        public Guid MenuId { get; set; }

    }
}