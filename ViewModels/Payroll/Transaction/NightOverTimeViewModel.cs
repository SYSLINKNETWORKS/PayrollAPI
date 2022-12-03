using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll
{

    public class NightOverTimeViewModel
    {
        public Guid EmployeeId { get; set; }

        public String EmployeeName { get; set; }
        public int MachineId { get; set; }
        public DateTime Date { get; set; }
        public string CheckType { get; set; }
        public bool Approved { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }
    }
    public class NightOverTimeGetByIdModel
    {
        public Guid EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public Guid MenuId { get; set; }
    }

    public class NightOverTimeViewByIdModel
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public DateTime Date { get; set; }
        public DateTime Checktime { get; set; }

        public string CheckType { get; set; }
        public bool Approved { get; set; }

        public Guid MenuId { get; set; }
    }

    public class NightOverTimeAddModel
    {
        public List<NightOverTimeListAddModel> NightOverTimeListAddModel { get; set; }

    }
    public class NightOverTimeListAddModel
    {
        public Guid EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public double OverTime { get; set; }
        public string Remarks { get; set; }

        public Guid MenuId { get; set; }

    }

    public class NightOverTimeEditModel
    {
        public Guid EmployeeId { get; set; }
        public Guid InOutCategoryId { get; set; }
        public bool checkinn { get; set; }
        public DateTime CheckinnTime { get; set; }
        public bool checkout { get; set; }
        public DateTime CheckoutTime { get; set; }
        public string CheckType { get; set; }
        public Guid MenuId { get; set; }

    }

    public class NightOverTimeGetApprovalModel
    {
        public List<NightOverTimeGetApprovalDetailModel> NightOverTimeGetApprovalDetailModels { get; set; }
        public List<NightOverTimeGetApprovalSummaryModel> NightOverTimeGetApprovalSummaryModels { get; set; }
    }
    public class NightOverTimeGetApprovalDetailModel
    {
        public int MachineId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckTime { get; set; }
        public bool Approved { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }

        public Guid MenuId { get; set; }

    }
    public class NightOverTimeGetApprovalSummaryModel
    {
        public DateTime Date { get; set; }
        public bool Approved { get; set; }
    }

    public class NightOverTimeApprovalEditModel
    {
        public int MachineId { get; set; }
        public string Date { get; set; }
        public string CheckTime { get; set; }
        public string Approved { get; set; }

        public Guid MenuId { get; set; }
    }
}