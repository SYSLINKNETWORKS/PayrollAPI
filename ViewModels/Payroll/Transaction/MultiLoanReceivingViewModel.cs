using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TWP_API_Payroll.ViewModels.Payroll
{


    public class MultiLoanReceivingBaseModel
    {
        public string CompanyName { get; set; }
        public string BranchName { get; set; }
        public string UserName { get; set; }

        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public List<MultiLoanReceivingViewModel> MultiLoanReceivingViewModel { get; set; }
    }

    public class MultiLoanReceivingViewModel
    {

        [Required]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        public Guid EmployeeId { get; set; }
        public int EmployeeMachineId { get; set; }

        public String EmployeeName { get; set; }

        [Required]
        public Guid LoanCatgoryId { get; set; }
        public String LoanCategoryName { get; set; }
        public double LoanAmount { get; set; }
        public double NoofInstalment { get; set; }
        public double InstallmentAmount { get; set; }

        public double Received { get; set; }
        public double Balance { get; set; }
        public double Receiving { get; set; }
        public double TotalBalance { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public bool NewPermission { get; set; }

        [Required]
        public bool UpdatePermission { get; set; }

        [Required]
        public bool DeletePermission { get; set; }

        public Guid MenuId { get; set; }
    }
    public class MultiLoanReceivingGetByIdModel
    {
        public Guid EmployeeId { get; set; }

        public DateTime Date { get; set; }

        public Guid MenuId { get; set; }
    }

    public class MultiLoanReceivingViewByIdModel
    {
        public Guid EmployeeId { get; set; }
        public Guid MenuId { get; set; }
    }

    public class MultiLoanReceivingAddModel
    {
        public List<MultiLoanReceivingListAddModel> MultiLoanReceivingListAddModel { get; set; }

    }
    public class MultiLoanReceivingListAddModel
    {
        public Guid LoanIssueId { get; set; }
        public double Receiving { get; set; }
        public Guid MenuId { get; set; }

    }

    public class MultiLoanReceivingEditModel
    {
        public List<MultiLoanReceivingListAddModel> MultiLoanReceivingListAddModel { get; set; }

    }



}