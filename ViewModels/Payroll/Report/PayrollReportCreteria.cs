using System;

namespace TWP_API_Payroll.ViewModels.Payroll.Report
{
    public class PayrollReportCreteria
    {
        public DateTime DateFrom { get; set; } = new DateTime();
        public DateTime DateAsOn { get; set; } = new DateTime();
        public DateTime DateTo { get; set; } = new DateTime();
        public string Id { get; set; }
        public string EmployeeId { get; set; }
        public string BranchId { get; set; }
        public string OfficeWorker { get; set; }
        public string TemporaryPermanent { get; set; }
        public string DesignationId { get; set; }
        public string DepartmentId { get; set; }
        public string Gender { get; set; }
        public string Active { get; set; }
        public string LoanStatus { get; set; }

    }
}