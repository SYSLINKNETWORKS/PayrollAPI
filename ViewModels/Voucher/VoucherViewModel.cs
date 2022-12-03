using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API.ViewModels
{
    public class VoucherMasterBaseModel
    {

        public DateTime Date { get; set; }

        public DateTime TransactionDate { get; set; }
        public string PaidReceived { get; set; }
        public string Remarks { get; set; }
        public Guid? CostCenterId { get; set; }
        public string VoucherTypeNo { get; set; }
        public Guid CurrencyId { get; set; }
        public bool OnlineCheck { get; set; }
        public int ChqNo { get; set; }
        public DateTime? ChqDate { get; set; }
        public bool ChqClear { get; set; }
        public double ExchangeRate { get; set; }
        public bool Approved { get; set; }
        public bool Check { get; set; }
        public string Type { get; set; }

    }
    public class VoucherMasterAddModel : VoucherMasterBaseModel
    {
        [Required]
        public Guid Menu_Id { get; set; }
        public Guid No { get; set; }
        public List<VoucherDetailViewModel> VoucherDetailViewModel { get; set; }
    }
    public class VoucherMasterUpdateModel : VoucherMasterBaseModel
    {
        [Required]
        public Guid No { get; set; }
        [Required]
        public Guid Menu_Id { get; set; }
        public List<VoucherDetailViewModel> VoucherDetailViewModel { get; set; }
    }


    public class AdvanceVoucherViewModel
    {
        public string CompanyId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public Guid VoucherNo { get; set; }
        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public string EmployeeName { get; set; }
        public string Remarks { get; set; }
        public Guid AdvanceId { get; set; }
        public double Amount { get; set; }


    }
    public class VoucherDetailViewModel
    {
        public Guid AccountNo { get; set; }
        public string Naration { get; set; }

        public double DebitAmount { get; set; }

        public double CreditAmount { get; set; }
    }


    public class SalaryVoucherViewModel
    {
        public string VoucherNo { get; set; }
        public string CompanyId { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }
        public string Narration { get; set; }
        public double Amount { get; set; }
        public double AdvanceAmount { get; set; }
        public double LoanAmount { get; set; }
        public double IncomeTaxAmount { get; set; }
        public double TakafulAmount { get; set; }
        public double OverTimeAmount { get; set; }
        public double NetAmount { get; set; }
        public string Category { get; set; } //Staff/ Worker

    }

    public class VoucherResponse
    {
        public Guid No { get; set; }
        public string Id { get; set; }

    }


}