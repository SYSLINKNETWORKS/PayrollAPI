using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Controllers.Payroll.Report;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Report;

namespace TWP_API_Payroll.Controllers.Payroll
{
    public class SalaryWorkerTable
    {
        private readonly DataContext _context = null;
        private ErrorLog _ErrorLog = new ErrorLog();
        SecurityHelper _SecurityHelper = new SecurityHelper();

        vCheckTimeTable _vCheckTimeTable = null;
        public SalaryWorkerTable(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
        }
        public async Task<ApiResponse> SalaryWorkerAsync(string _TokenString, DateTime _Date, string _EmployeeIdStr, ClaimsPrincipal _User)
        {

            var ApiResponse = new ApiResponse();
            UserLoginInfoBaseModel _userInformation = new UserLoginInfoBaseModel();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                DateTime DateFrom = new DateTime(_Date.Year, _Date.Month, 1);
                DateTime DateTo = new DateTime(_Date.Year, _Date.Month, DateTime.DaysInMonth(_Date.Year, _Date.Month));
                var _NoOfDaysMonth = DateTime.DaysInMonth(_Date.Year, _Date.Month);
                var _YearEndDate = Convert.ToDateTime(_userInformation.YearEndDate);// (await _context.FinancialYears.OrderByDescending(o => o.EndDate).FirstOrDefaultAsync()).EndDate;

                Guid _EmployeeId = Guid.Empty;
                if (_EmployeeIdStr != "0") { _EmployeeId = new Guid(_EmployeeIdStr); } else { _EmployeeIdStr = ""; }

                PayrollReportCreteria _PayrollReportCreteria = new PayrollReportCreteria();
                _PayrollReportCreteria.DateFrom = DateFrom;
                _PayrollReportCreteria.DateTo = DateTo;
                _PayrollReportCreteria.EmployeeId = _EmployeeIdStr;

                //Delete Existing Record
                var _SalaryWorkerRemove = await _context.WorkerSalaries.Where(x => x.Date >= DateFrom && x.Date <= DateTo && (_EmployeeId == Guid.Empty ? true : x.EmployeeId == _EmployeeId)).ToListAsync();
                if (_SalaryWorkerRemove.Count > 0)
                {
                    if (_SalaryWorkerRemove.FirstOrDefault().VoucherPostCk)
                    {
                        ApiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                        ApiResponse.message = "Voucher Post";

                        return ApiResponse;
                    }
                    _context.WorkerSalaries.RemoveRange(_SalaryWorkerRemove);
                    _context.SaveChanges();

                }


                var _CheckTimeTable = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

                List<SalaryProcessWorkerModel> _SalaryProcessWorkerModel = new List<SalaryProcessWorkerModel>();

                var _EmployeeTable = (from _Employee in _context.Employees
                                      where _Employee.Active == true && _Employee.StopPayment == false && _Employee.ResignationCheck == false &&
                                      _Employee.DateofJoin <= DateTo && _Employee.OfficeWorker == Enums.Payroll.Worker.ToString() &&
                                      (_EmployeeId == Guid.Empty ? true : _Employee.Id == _EmployeeId)
                                      select _Employee).ToList().Union(from _Employee in _context.Employees
                                                                       where _Employee.Active == true && _Employee.StopPayment == false && _Employee.ResignationCheck == true &&
                                                                       _Employee.ResignationDate >= DateFrom && _Employee.ResignationDate <= DateTo &&
                                                                       _Employee.DateofJoin <= DateTo && _Employee.OfficeWorker == Enums.Payroll.Worker.ToString() &&
                                                                       (_EmployeeId == Guid.Empty ? true : _Employee.Id == _EmployeeId)
                                                                       select _Employee).ToList();

                if (_EmployeeTable == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Employee not found";
                    return ApiResponse;
                }
                if (_EmployeeTable.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Employee not found";
                    return ApiResponse;
                }

                foreach (var _EmployeeRecord in _EmployeeTable)
                {
                    // var _Branch = await _context.Branches.Where(x => x.Id == _EmployeeRecord.BranchId).FirstOrDefaultAsync();

                    var _RosterGroups = await _context.RosterGroups.Where(rg => rg.RosterId == _EmployeeRecord.RosterId && rg.Date <= DateTo).OrderByDescending(rgo => rgo.Date).FirstOrDefaultAsync();

                    var _Salary = await _context.Salaries.Where(s => s.EmployeeId == _EmployeeRecord.Id && s.Date <= DateTo).OrderByDescending(sal => sal.Date).FirstOrDefaultAsync();
                    double _CurrentAmount = 0;
                    if (_Salary != null) { _CurrentAmount = _Salary.CurrentAmount; }

                    var _AdvanceAmount = await _context.Advances.Where(a => a.EmployeeId == _EmployeeRecord.Id && a.Date >= DateFrom && a.Date <= DateTo).SumAsync(s => s.Amount);
                    var _LoanReceiveAmount = await (_context.LoanReceives).Include(l => l.LoanIssue).Where(s => s.LoanIssue.EmployeeId == _EmployeeRecord.Id && s.Type == Enums.Operations.S.ToString() && s.Date >= DateFrom && s.Date <= DateTo).SumAsync(s => s.Amount);

                    var _SalaryAddition = await (_context.SalaryAdditionDeductions).Where(s => s.EmployeeId == _EmployeeRecord.Id && s.Date >= DateFrom && s.Date <= DateTo).ToListAsync();
                    var _SalaryAdditionAmount = (from g in _SalaryAddition select g.AdditionAmount).Sum();
                    var _SalaryDeductionAmount = (from g in _SalaryAddition select g.DeductionAmount).Sum();


                    int _JoinDays = _EmployeeRecord.DateofJoin <= DateFrom ? 0 : _EmployeeRecord.DateofJoin.Subtract(DateFrom).Days;
                    int _ResignDays = !_EmployeeRecord.ResignationCheck ? 0 : DateTo.Subtract(_EmployeeRecord.ResignationDate).Days;
                    int _totalPersentDays = _NoOfDaysMonth;
                    int _absentDays = 0;
                    int _AbsentDaysApproval = 0;
                    double _Late_Hours = 0;
                    double _Late_Days = 0;
                    double _Late_Days_Total = 0;
                    double _Late_Days_Actual = 0;
                    double _OverTime_Minutes = 0;
                    double _OverTime_Hours = 0;

                    if (!_EmployeeRecord.AttendanceExempt)
                    {
                        var vchecktimeview = _CheckTimeTable.vCheckTimeViewModel.Where(e => e.EmployeeId == _EmployeeRecord.Id).ToList();

                        var _WorkingHours = vchecktimeview.FirstOrDefault().WorkingHours;

                        var _presentDays = vchecktimeview.Where(x => x.CheckDate >= DateFrom && x.CheckDate <= DateTo && x.CheckDate >= _EmployeeRecord.DateofJoin && x.DayOff == false && x.HolidayGazette == false).Count();
                        //Day Off
                        var _DaysOff = vchecktimeview.Where(x => x.DayOff == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                        //Gazette Holidays
                        var _HolidaysGazette = vchecktimeview.Where(x => x.DayOff == false && x.HolidayGazette == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                        //Absent Approval
                        var _AbsentApproval = vchecktimeview.Where(x => x.DayOff == false && x.AbsentAdjustmentCheck == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                        //Sandwitch Working

                        var _SandwitchBeforeTable = vchecktimeview.Where(x => x.SandwitchBefore == true).ToList();
                        var _SandwitchAfterTable = vchecktimeview.Where(x => x.SandwitchAfter == true).ToList();

                        var _SandwitchBefore = vchecktimeview.Where(x => x.SandwitchBefore == true).Count();
                        var _SandwitchAfter = vchecktimeview.Where(x => x.SandwitchAfter == true).Count();

                        //Present
                        _totalPersentDays = _presentDays + _DaysOff + _HolidaysGazette + _AbsentApproval - _SandwitchBefore - _SandwitchAfter;
                        //Absent
                        _absentDays = _NoOfDaysMonth - (_totalPersentDays + _JoinDays + _ResignDays);
                        var _absentMinutes = (_absentDays * _WorkingHours) * 60;

                        _AbsentDaysApproval = vchecktimeview.Where(x => x.AbsentCheck == true).Count();

                        //OverTime
                        _OverTime_Minutes = (vchecktimeview.Where(x => x.OverTimeCheck == true && x.AttendanceExemptCheck == false).Sum(s => s.OverTime + s.EarlyOverTime + s.OverTimeNight - s.EarlyGoing - s.LateComing) - _absentMinutes);
                        _OverTime_Hours = _OverTime_Minutes > 0 ? _OverTime_Minutes / 60 : 0;

                        //Late Days
                        _Late_Hours = vchecktimeview.Where(x => x.LateCheck == true && x.AttendanceExemptCheck == false).Sum(s => s.LateComing) / 60;
                        _Late_Days_Total = vchecktimeview.Where(x => x.LateComing > 0 && x.LateCheck == true && x.AttendanceExemptCheck == false).Count();
                        _Late_Days = _Late_Days_Total >= 4 ? _Late_Days_Total / 4 : 0;
                        var _HalfDays_Adjustment = vchecktimeview.Where(x => x.LateCheck == true && x.AttendanceExemptCheck == false && x.LateComing > (x.RosterWorkingHoursMorning * 60)).Count() > 1 ? 1 : 0;
                        var _Late_Days_Actual_Calculate = _OverTime_Minutes / 60;
                        _Late_Days_Actual = _Late_Days_Actual_Calculate < 0 ? -_Late_Days_Actual_Calculate : 0; //_Late_Days - _HalfDays_Adjustment;
                    }
                    var _SalaryWorker = new SalaryProcessWorkerModel
                    {

                        EmployeeId = _EmployeeRecord.Id,
                        Date = DateTo,
                        CheckIncomeTax = _EmployeeRecord.IncomeTax,
                        Takaful = _EmployeeRecord.TakafulRate,
                        DateOfJoining = _EmployeeRecord.DateofJoin,
                        DateOfResign = _EmployeeRecord.ResignationCheck == false ? null : _EmployeeRecord.ResignationDate, //_EmployeeRecord.ResignationDate.
                        JoinDays = _JoinDays,
                        ResignDays = _ResignDays,
                        NoOfDaysMonth = _NoOfDaysMonth,
                        WorkingHours = _RosterGroups.WorkingHours,
                        AttendanceExempted = _EmployeeRecord.AttendanceExempt,
                        CheckAttendanceAllowance = _EmployeeRecord.AttendanceAllowance,
                        CheckOvertime = _EmployeeRecord.OverTime,
                        SalaryAmount = ((_CurrentAmount * 0.6)),
                        SalaryAllowanceAmount = ((_CurrentAmount * 0.4)),
                        SalaryGrossAmount = _CurrentAmount,
                        //Present Days
                        PresentDays = _totalPersentDays,
                        //Absent Days
                        AbsentDays = _absentDays,
                        AbsentDaysActual = _absentDays,
                        AbsentDaysApproval = _AbsentDaysApproval,
                        //Late
                        LateHours = _Late_Hours,
                        LateDays = _Late_Days,
                        LateDaysTotal = _Late_Days_Total,
                        LateDaysActual = _Late_Days_Actual,

                        AdvanceAmount = _AdvanceAmount,
                        LoanAmount = _LoanReceiveAmount,
                        AdditionAmount = _SalaryAdditionAmount,
                        DeductionAmount = _SalaryDeductionAmount,

                        OvertimeMinutes = _OverTime_Minutes,
                        OvertimeHours = _OverTime_Hours,
                        OvertimeDays = _OverTime_Hours > 0 ? _OverTime_Hours / _RosterGroups.WorkingHours : 0,
                        OvertimeActual = _OverTime_Hours,
                        OvertimeRate = _EmployeeRecord.OverTimeRate,
                        // OvertimeRate = _OverTime_Rate,
                        // OvertimeActualAmount = _OverTime_Amount,
                        CompanyId = _userInformation.CompanyId,// _Branch.CompanyId,
                    };
                    _SalaryProcessWorkerModel.Add(_SalaryWorker);
                }

                //Income Tax
                foreach (var _IncomeTax in _SalaryProcessWorkerModel.Where(i => i.CheckIncomeTax == true))
                {
                    var _EmployeeAllowances = await _context.EmployeeAllowances.Include(a => a.allowance).Where(x => x.EmployeeId == _IncomeTax.EmployeeId).SumAsync(s => s.allowance.Amount);
                    var _Working_months = _YearEndDate.Subtract(_IncomeTax.DateOfJoining).Days;
                    _Working_months = Convert.ToInt32(Math.Ceiling(_Working_months > 1 ? ((_Working_months + 1) / 30.436875) : (_Working_months / 30.436875)));

                    DateTime _year = new DateTime(_Date.Year, 1, 1);

                    if (_Working_months > 12)
                    {
                        _Working_months = 12;
                    }
                    double _PreviousSalaryAmount = 0;
                    int _PreviousSalaryCount = 0;

                    var _PreviousSalary = await _context.WorkerSalaries.Where(s => s.EmployeeId == _IncomeTax.EmployeeId && (s.Date > _year && s.Date < DateFrom)).ToListAsync();
                    if (_PreviousSalary != null)
                    {
                        _PreviousSalaryAmount = _PreviousSalary.Sum(s => s.SalaryGrossAmount);
                        _PreviousSalaryCount = _PreviousSalary.Count();
                    }
                    var _SalaryAllowance = _IncomeTax.SalaryGrossAmount + _EmployeeAllowances;
                    var _IncomeTaxAnnually = _SalaryAllowance * (_Working_months - _PreviousSalaryCount) + _PreviousSalaryAmount;
                    var _IncomeTaxSlab = await _context.IncomeTaxSlabEmployees.OrderByDescending(o => o.Date).ToListAsync();
                    var _IncomeTaxSlabAmount = _IncomeTaxSlab.Where(w => w.Date <= DateTo && _IncomeTaxAnnually >= w.SlabFrom && _IncomeTaxAnnually <= w.SlabTo).OrderByDescending(o => o.Date).FirstOrDefault();
                    {
                        var _EmployeeIncomeTaxAmount = _IncomeTaxAnnually - _IncomeTaxSlabAmount.SlabFrom;
                        var _EmployeeIncomeTaxAmountTotal = (_EmployeeIncomeTaxAmount * (_IncomeTaxSlabAmount.Percentage / 100)) + _IncomeTaxSlabAmount.Amount;
                        _IncomeTax.IncomeTaxAmount = Math.Round((_EmployeeIncomeTaxAmountTotal / 12), 0);
                    }
                }

                //Net Salary
                foreach (var _SalaryWorkerTableRecord in _SalaryProcessWorkerModel)
                {
                    //Salary 
                    _SalaryWorkerTableRecord.SalaryPerDay = _SalaryWorkerTableRecord.SalaryAmount / _SalaryWorkerTableRecord.NoOfDaysMonth;
                    _SalaryWorkerTableRecord.SalaryPerHour = _SalaryWorkerTableRecord.SalaryPerDay / _SalaryWorkerTableRecord.WorkingHours;
                    _SalaryWorkerTableRecord.SalaryGrossPerMinute = _SalaryWorkerTableRecord.SalaryPerHour / 60;

                    _SalaryWorkerTableRecord.SalaryGrossPerDay = _SalaryWorkerTableRecord.SalaryGrossAmount / _SalaryWorkerTableRecord.NoOfDaysMonth;
                    _SalaryWorkerTableRecord.SalaryGrossPerHour = _SalaryWorkerTableRecord.SalaryGrossPerDay / _SalaryWorkerTableRecord.WorkingHours;
                    _SalaryWorkerTableRecord.SalaryGrossPerMinute = _SalaryWorkerTableRecord.SalaryGrossPerHour / 60;

                    //Overtime Amount
                    var _OverTime_Rate = _SalaryWorkerTableRecord.SalaryPerHour * _SalaryWorkerTableRecord.OvertimeRate;
                    var _OverTime_Amount = _SalaryWorkerTableRecord.OvertimeActual * _OverTime_Rate;
                    _SalaryWorkerTableRecord.OvertimeRate = _OverTime_Rate;
                    _SalaryWorkerTableRecord.OvertimeActualAmount = _OverTime_Amount;

                    //Attendance Allownace Amount
                    var _AttendanceAllownaceAmount = _SalaryWorkerTableRecord.LateDaysTotal == 0 ? (0.05 * _SalaryWorkerTableRecord.SalaryGrossAmount) : _SalaryWorkerTableRecord.LateDaysTotal <= 4 ? (0.03 * _SalaryWorkerTableRecord.SalaryGrossAmount) : 0;
                    var _AttendanceAllownace = _SalaryWorkerTableRecord.PresentDays == _SalaryWorkerTableRecord.NoOfDaysMonth && _SalaryWorkerTableRecord.CheckAttendanceAllowance == true && _SalaryWorkerTableRecord.AbsentDaysApproval == 0 ? _AttendanceAllownaceAmount : 0;
                    _SalaryWorkerTableRecord.AttendanceAllowanceAmount = _AttendanceAllownace;

                    //Late Deduction Amount
                    var _LateDeductionAmount = _SalaryWorkerTableRecord.LateDaysActual * _SalaryWorkerTableRecord.OvertimeRate;// _SalaryWorkerTableRecord.ResignDays > 0 ? _SalaryWorkerTableRecord.LateDaysActual * _SalaryWorkerTableRecord.SalaryGrossPerHour : _SalaryWorkerTableRecord.LateDaysActual * _SalaryWorkerTableRecord.OvertimeRate;
                    _SalaryWorkerTableRecord.LateDaysActualAmount = _LateDeductionAmount;

                    //Absent Days Actual Amount
                    _SalaryWorkerTableRecord.AbsentDaysActualAmount = _SalaryWorkerTableRecord.AbsentDaysActual * _SalaryWorkerTableRecord.SalaryGrossPerDay;
                    //Payable Salary
                    _SalaryWorkerTableRecord.Amount = _SalaryWorkerTableRecord.PresentDays == 0 ? 0 : _SalaryWorkerTableRecord.SalaryGrossPerDay * (_SalaryWorkerTableRecord.NoOfDaysMonth - (_SalaryWorkerTableRecord.JoinDays + _SalaryWorkerTableRecord.ResignDays));
                    _SalaryWorkerTableRecord.GrossAmount = _SalaryWorkerTableRecord.PresentDays == 0 ? 0 : _SalaryWorkerTableRecord.Amount + (_SalaryWorkerTableRecord.AdditionAmount + _SalaryWorkerTableRecord.AttendanceAllowanceAmount) - (_SalaryWorkerTableRecord.Takaful + _SalaryWorkerTableRecord.AdvanceAmount + _SalaryWorkerTableRecord.LoanAmount + _SalaryWorkerTableRecord.IncomeTaxAmount + _SalaryWorkerTableRecord.DeductionAmount + _SalaryWorkerTableRecord.LateDaysActualAmount);
                    _SalaryWorkerTableRecord.PayableAmount = _SalaryWorkerTableRecord.PresentDays == 0 ? 0 : _SalaryWorkerTableRecord.GrossAmount + _SalaryWorkerTableRecord.OvertimeActualAmount;

                }

                List<WorkerSalary> _SalaryWorkerTable = new List<WorkerSalary>();
                foreach (var _SalaryProcessWorkerModelRecord in _SalaryProcessWorkerModel)
                {

                    _SalaryWorkerTable.Add(new WorkerSalary
                    {

                        EmployeeId = _SalaryProcessWorkerModelRecord.EmployeeId,
                        Date = DateTo,
                        CheckIncomeTax = _SalaryProcessWorkerModelRecord.CheckIncomeTax,
                        Takaful = _SalaryProcessWorkerModelRecord.Takaful,
                        DateOfJoining = _SalaryProcessWorkerModelRecord.DateOfJoining,
                        DateOfResign = _SalaryProcessWorkerModelRecord.DateOfResign,
                        NoOfDaysMonth = _NoOfDaysMonth,
                        WorkingHours = _SalaryProcessWorkerModelRecord.WorkingHours,
                        AttendanceExempted = _SalaryProcessWorkerModelRecord.AttendanceExempted,
                        CheckAttendanceAllowance = _SalaryProcessWorkerModelRecord.CheckAttendanceAllowance,
                        AttendanceAllowanceAmount = _SalaryProcessWorkerModelRecord.AttendanceAllowanceAmount,
                        LateDaysActualAmount = _SalaryProcessWorkerModelRecord.LateDaysActualAmount,
                        CheckOvertime = _SalaryProcessWorkerModelRecord.CheckOvertime,
                        SalaryAmount = _SalaryProcessWorkerModelRecord.SalaryAmount,
                        SalaryAllowanceAmount = _SalaryProcessWorkerModelRecord.SalaryAllowanceAmount,
                        SalaryGrossAmount = _SalaryProcessWorkerModelRecord.SalaryGrossAmount,
                        PresentDays = _SalaryProcessWorkerModelRecord.PresentDays,
                        AbsentDays = _SalaryProcessWorkerModelRecord.AbsentDays,
                        AllowanceAbsent = _SalaryProcessWorkerModelRecord.AllowanceAbsent,
                        AdvanceAmount = _SalaryProcessWorkerModelRecord.AdvanceAmount,
                        LoanAmount = _SalaryProcessWorkerModelRecord.LoanAmount,
                        AdditionAmount = _SalaryProcessWorkerModelRecord.AdditionAmount,
                        DeductionAmount = _SalaryProcessWorkerModelRecord.DeductionAmount,

                        IncomeTaxAmount = _SalaryProcessWorkerModelRecord.IncomeTaxAmount,

                        OvertimeMinutes = _SalaryProcessWorkerModelRecord.OvertimeMinutes,
                        OvertimeHours = _SalaryProcessWorkerModelRecord.OvertimeHours,
                        OvertimeDays = _SalaryProcessWorkerModelRecord.OvertimeDays,
                        OvertimeRate = _SalaryProcessWorkerModelRecord.OvertimeRate,
                        OvertimeActual = _SalaryProcessWorkerModelRecord.OvertimeActual,
                        OvertimeActualAmount = _SalaryProcessWorkerModelRecord.OvertimeActualAmount,

                        SalaryPerDay = _SalaryProcessWorkerModelRecord.SalaryPerDay,
                        SalaryPerHour = _SalaryProcessWorkerModelRecord.SalaryPerHour,
                        SalaryPerminute = _SalaryProcessWorkerModelRecord.SalaryPerminute,

                        SalaryGrossPerDay = _SalaryProcessWorkerModelRecord.SalaryGrossPerDay,
                        SalaryGrossPerHour = _SalaryProcessWorkerModelRecord.SalaryGrossPerHour,
                        SalaryGrossPerMinute = _SalaryProcessWorkerModelRecord.SalaryGrossPerMinute,

                        Amount = _SalaryProcessWorkerModelRecord.Amount,
                        GrossAmount = _SalaryProcessWorkerModelRecord.GrossAmount,
                        PayableAmount = _SalaryProcessWorkerModelRecord.PayableAmount,

                        CompanyId = _SalaryProcessWorkerModelRecord.CompanyId,
                        UserNameInsert = _CheckTimeTable.UserName,
                        InsertDate = DateTime.Now
                    });
                }

                await _context.WorkerSalaries.AddRangeAsync(_SalaryWorkerTable);
                await _context.SaveChangesAsync();
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Salary process for month : " + DateTo.ToString("MMM-yyyy");
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User); ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User); ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }

    }
}