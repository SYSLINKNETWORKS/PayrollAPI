using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace TWP_API_Payroll.Models
{
    [Table("T_StaffSalary")]
    public partial class StaffSalary
    {
        [Key]
        [Required]
        public Guid EmployeeId { get; set; } //emppro_id
        // Navigation Property
        [ForeignKey("EmployeeId")]
        public Employee employee { get; set; }

        [Key]
        [Required]
        public DateTime Date { get; set; } //salary_date
        public bool CheckIncomeTax { get; set; } //salary_ckinctax
        public double Takaful { get; set; } //salary_takaful
        public DateTime DateOfJoining { get; set; } //salary_doj
        public DateTime? DateOfResign { get; set; } //salary_regdate
        public bool AttendanceExempted { get; set; } //salary_attendace_exempted
        public bool CheckAttendanceAllowance { get; set; } //salary_ckattendanceallownace
        public bool CheckOvertime { get; set; } //salary_ckovertime
        public double SalaryAmount { get; set; } //salary_amount
        public double SalaryAllowanceAmount { get; set; } //salary_allowanceamount
        public double SalaryGrossAmount { get; set; } //salary_grossamount
        public int NoOfDaysMonth { get; set; } //salary_noofdaysmonth
        public int WorkingDays { get; set; } //salary_WorkingDays
        public int ResignDays { get; set; } //employee_salary_staff_resign_days
        public int JoinDays { get; set; } //employee_salary_staff_join_days
        public double WorkingHours { get; set; } //salary_workinghours
        public double SalaryPerDay { get; set; } //salary_perday
        public double SalaryPerHour { get; set; } //salary_perhour
        public double SalaryPerminute { get; set; } //salary_perminute
        public double SalaryGrossPerDay { get; set; } //salary_grossperday
        public double SalaryGrossPerHour { get; set; } //salary_grossperhour
        public double SalaryGrossPerMinute { get; set; } //salary_grossperminute
        public int PresentDays { get; set; } //salary_presentdays
        public int AbsentDays { get; set; } //salary_absentdays
        public int AbsentDaysApproval { get; set; } //salary_absentdaysapproval
        public int TotalAbsentDays { get; set; } //salary_totalabsentdays
        public int AdjustedDays { get; set; } //salary_adjusteddays
        public double AdvanceAmount { get; set; } //salary_advance
        public double LoanAmount { get; set; } //salary_loan
        public double IncomeTaxAmount { get; set; } //salary_incometaxamount
        public double AdditionAmount { get; set; } //salary_addition
        public double DeductionAmount { get; set; } //salary_deduction
        public double AttendanceAllowanceAmount { get; set; } //salary_attendanceallowanceamount
        public double OvertimeMinutes { get; set; } //employee_salary_staff_overtime_minutes
        public double OvertimeHours { get; set; } //employee_salary_staff_overtime_hours
        public double OvertimeDays { get; set; } //employee_salary_staff_overtime_days
        public double LateMinutes { get; set; } //employee_salary_staff_late_minutes
        public double LateHours { get; set; } //employee_salary_staff_late_hours
        public double LateDays { get; set; } //employee_salary_staff_late_days
        public double LateDaysTotal { get; set; } //employee_salary_staff_late_days_Total
        public double AbsentMinutes { get; set; } //employee_salary_staff_absent_minutes
        public double AbsentHours { get; set; } //employee_salary_staff_absent_hours
        public double OvertimeRate { get; set; } //employee_salary_staff_overtime_rate
        public double OvertimeActual { get; set; } //employee_salary_staff_overtime_actual
        public double OvertimeActualAmount { get; set; } //employee_salary_staff_overtime_actual_amount
        public double AbsentDaysActual { get; set; } //employee_salary_staff_absentdays_actual
        public double AbsentDaysActualAmount { get; set; } //employee_salary_staff_absentdays_actual_amount
        public double LateDaysActual { get; set; } //employee_salary_staff_latedays_actual
        public double LateDaysActualAmount { get; set; } //employee_salary_staff_latedays_actual_amount
        public double Amount { get; set; } //employee_salary_staff_amount
        public double GrossAmount { get; set; } //employee_salary_staff_amount_gross
        public double PayableAmount { get; set; } //employee_salary_staff_amount_payable
        public int AllowanceAbsent { get; set; } //salary_allowanceabsent
        public bool VoucherPostCk { get; set; }
        public string UserNamePost { get; set; }
        public DateTime? VoucherPostDate { get; set; }
        public Guid? VoucherNo { get; set; }

        [Required]
        public Guid CompanyId { get; set; }
        public string UserNameInsert { get; set; }

        [Required]
        public DateTime InsertDate { get; set; } = DateTime.Now;

    }
}