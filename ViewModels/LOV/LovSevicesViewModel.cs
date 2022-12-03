using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels
{
    public class LovServicesViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }


    public class LovServicesDescriptionSaleOrderViewModel
    {
        public Guid Id { get; set; }
        public int No { get; set; }
        public string Name { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
    }
    public class LovServicesLoanReceivedViewModel
    {
        public Guid LoanId { get; set; }
        public string EmployeeName { get; set; }
        public string LoanCategoryName { get; set; }
        public double Amount { get; set; }
        public double InstallmentAmount { get; set; }
        public double ReceivedAmount { get; set; }
        public double BalanceAmount { get; set; }
        public bool Status { get; set; }
    }
    public class RoleViewByUserIdModel
    {
        public string Role_Id { get; set; }
        public string RoleName { get; set; }
        public List<MenuPerViewByUserID> menuPerViews { get; set; }
    }
    public class MenuPerViewByUserID
    {
        [Required]
        public int Menu_Id { get; set; }

        [Required]
        public string Menu_Name { get; set; }

        [Required]
        public string Alias { get; set; }
        // [Required]
        // public bool View_Permission { get; set; }

        [Required]
        public bool Insert_Permission { get; set; }

        [Required]
        public bool Update_Permission { get; set; }

        [Required]
        public bool Delete_Permission { get; set; }
    }
    public class RosterEmployeeByIdModel
    {
        public Guid RosterId { get; set; }

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public String EmployeeName { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public DateTime? RosterInn { get; set; }

        [Required]
        public DateTime? RosterOut { get; set; }
    }
    public class NightOverTimeEmployeeByIdModel
    {

        [Required]
        public Guid EmployeeId { get; set; }

        [Required]
        public String EmployeeName { get; set; }
        [Required]
        public String DesignationName { get; set; }

        [Required]
        public DateTime Date { get; set; }
        public string Remarks { get; set; }

        [Required]
        public Double OverTime { get; set; }
    }
    public class LovServicesCostSheetItemRateViewModel
    {
        public Guid DetailId { get; set; }
        public string Description { get; set; }
        public string Item { get; set; }
        public string ItemPaperName { get; set; }
        public double Quantity { get; set; }
        public string CustomerName { get; set; }
        public string LocationName { get; set; }
        public string PrintingColorName { get; set; }
        public double GSM { get; set; }
        public double BoardHeight { get; set; }
        public double BoardWidth { get; set; }
        public double LayoutHeight { get; set; }
        public double LayoutWidth { get; set; }
        public int RowIdDetail { get; set; }
    }
    public class LovServicesSalesOrderDetailViewModel
    {
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }

        public string Description { get; set; }

        public double Qty { get; set; }

        public DateTime Date { get; set; }

        public Guid DSO_ID { get; set; }

    }

    public class LovServicesJobOrderViewModel
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ItemId { get; set; }
        public string ItemName { get; set; }
        public DateTime Date { get; set; }
        public double SheetQty { get; set; }
        public double Qty { get; set; }
        public Guid MSO_ID { get; set; }
        public Guid DSO_ID { get; set; }

    }

    public class MSFinancialYearServicesViewModel
    {
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDateDate { get; set; }
    }

}