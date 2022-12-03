using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll
{

    public class InOutEditorViewModel
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
    public class InOutEditorGetByIdModel
    {
        public Guid EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public Guid MenuId { get; set; }
    }

    public class InOutEditorViewByIdModel
    {
        public Guid EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public Guid InOutCategoryId { get; set; }

        public string InOutCategoryName { get; set; }

        public DateTime Date { get; set; }
        public DateTime Checktime { get; set; }

        public string CheckType { get; set; }
        public bool Approved { get; set; }

        public Guid MenuId { get; set; }
    }

    public class InOutEditorAddModel
    {
        public List<InOutEditorListAddModel> InOutEditorListAddModel { get; set; }

    }
    public class InOutEditorListAddModel
    {
        public Guid EmployeeId { get; set; }
        public Guid InOutCategoryId { get; set; }
        public DateTime CheckTime { get; set; }
        public string CheckType { get; set; }
        public Guid MenuId { get; set; }
        public bool Tag { get; set; }

    }

    public class InOutEditorEditModel
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

    public class InOutEditorGetApprovalModel
    {
        public List<InOutEditorGetApprovalDetailModel> InOutEditorGetApprovalDetailModels { get; set; }
        public List<InOutEditorGetApprovalSummaryModel> inOutEditorGetApprovalSummaryModels { get; set; }
    }
    public class InOutEditorGetApprovalDetailModel
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
    public class InOutEditorGetApprovalSummaryModel
    {
        public DateTime Date { get; set; }
        public bool Approved { get; set; }
    }

    public class InOutEditorApprovalEditModel
    {
        public int MachineId { get; set; }
        public string Date { get; set; }
        public string CheckTime { get; set; }
        public string Approved { get; set; }

        public Guid MenuId { get; set; }
    }
}