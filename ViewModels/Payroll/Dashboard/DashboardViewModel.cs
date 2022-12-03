using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TWP_API_Payroll.ViewModels.Payroll.Dashboard
{
    public class DashboardViewModel
    {
        public Int32 TotalStrength { get; set; }
        public List<AttendanceMachineInfo> attendanceMachineInfos { get; set; }
        public List<PresentViewModel> present { get; set; }
        public List<AbsentViewModel> absent { get; set; }
        public List<LateViewModel> late { get; set; }
    }
    public class AttendanceMachineInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public Guid AttendanceMachineGroupId { get; set; }
        public String AttendanceMachineGroupName { get; set; }
    }
    public class PresentViewModel
    {
        public Guid EmployeeId { get; set; }
        public Int32 EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string AttendanceMachineName { get; set; }
        public DateTime? InnTime { get; set; }
        public DateTime? OutTime { get; set; }
    }

    public class LateViewModel
    {
        public Guid EmployeeId { get; set; }
        public Int32 EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }
        public DateTime? InnTime { get; set; }
        public double LateMinutes { get; set; }
    }
    public class AbsentViewModel
    {
        public Guid EmployeeId { get; set; }
        public Int32 EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string Remarks { get; set; }
    }
    public class InOutViewModel
    {
        public Guid EmployeeId { get; set; }
        public Int32 EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string CheckType { get; set; }
        public DateTime CheckTime { get; set; }
        public Boolean Approved { get; set; }
    }
    public class InOutMapViewModel
    {
        public Guid EmployeeId { get; set; }
        public Int32 EmployeeNo { get; set; }
        public string EmployeeName { get; set; }
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid DesignationId { get; set; }
        public string DesignationName { get; set; }
        public string CheckType { get; set; }
        public DateTime CheckTime { get; set; }
        public Boolean Approved { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Address { get; set; }
    }
    public class DashboardFilterViewModel
    {
        public string No { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public Guid MenuId { get; set; }
    }
    public class InOutEditorApprovalDashboardViewModel
    {
        public int MachineId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public DateTime CheckTime { get; set; }
        public bool Approved { get; set; }

    }
    public class NightOverTimeDashboardViewModel
    {
        public Guid EmployeeId { get; set; }
        public int MachineId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public bool Approved { get; set; }

    }
    public class AdvanceDashboardViewModel
    {
        public Guid Id { get; set; }
        public int MachineId { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set;}
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool Approved { get; set; }

    }

    public class LoanDashboardViewModel
    {
        public Guid Id { get; set; }
        public int MachineId { get; set; }
        public string EmployeeName { get; set; }

        public string DepartmentName { get; set; }
        public string DesignationName { get; set;}
        public DateTime Date { get; set; }
        public double Amount { get; set; }
        public bool Approved { get; set; }

    }

    public class EmployeeDashboardViewModel
    {
        public Guid Id { get; set; }
        public int MachineId { get; set; }
        public DateTime Date { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public bool Approved { get; set; }

    }


    public class UpdateNightOverTimeDashboardViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public double Hours { get; set; }
        [Required]
        public string Approved { get; set; }
    }
    public class UpdateDashboardViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Approved { get; set; }
    }

}