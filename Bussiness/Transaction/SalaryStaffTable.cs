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
    public class SalaryStaffTable
    {
        private readonly DataContext _context = null;
        private ErrorLog _ErrorLog = new ErrorLog();
        SecurityHelper _SecurityHelper = new SecurityHelper();

        vCheckTimeTable _vCheckTimeTable = null;
        public SalaryStaffTable(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
        }
        public async Task<ApiResponse> SalaryStaffAsync(String _TokenString, DateTime _Date, string _EmployeeIdStr, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            UserLoginInfoBaseModel _userInformation = new UserLoginInfoBaseModel();
            string _UserName = "";
            try
            {

                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;
                _UserName = _userInformation.UserName;

                DateTime _AnnualLeaveDateFrom = new DateTime(_Date.Year, 1, 1);

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
                var _SalaryStaffRemove = await _context.StaffSalaries.Where(x => x.Date >= DateFrom && x.Date <= DateTo && (_EmployeeId == Guid.Empty ? true : x.EmployeeId == _EmployeeId)).ToListAsync();
                if (_SalaryStaffRemove.Count > 0)
                {
                    if (_SalaryStaffRemove.FirstOrDefault().VoucherPostCk)
                    {
                        ApiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                        ApiResponse.message = "Voucher Post";

                        return ApiResponse;
                    }
                    _context.StaffSalaries.RemoveRange(_SalaryStaffRemove);
                    _context.SaveChanges();


                }


                var _CheckTimeTable = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

                //Delete Existing Record from Annual Leave Salary
                var _AnnualLeaveTableSalaryRemove = await _context.AnnualLeaveAdjustments.Where(s => s.Date == DateTo && s.Type == Enums.Operations.S.ToString() && (_EmployeeId == Guid.Empty ? true : s.EmployeeId == _EmployeeId)).ToListAsync();
                if (_AnnualLeaveTableSalaryRemove != null)
                {
                    _context.AnnualLeaveAdjustments.RemoveRange(_AnnualLeaveTableSalaryRemove);
                    _context.SaveChanges();

                }

                List<SalaryProcessStaffModel> _SalaryProcessStaffModel = new List<SalaryProcessStaffModel>();

                var _EmployeeTable = (from _Employee in _context.Employees
                                          //join _branch in _context.Branches on _Employee.BranchId equals _branch.Id \
                                      where _Employee.Active == true && _Employee.StopPayment == false && _Employee.ResignationCheck == false &&
                                      _Employee.DateofJoin <= DateTo && _Employee.OfficeWorker == Enums.Payroll.Office.ToString() &&
                                      (_EmployeeId == Guid.Empty ? true : _Employee.Id == _EmployeeId)
                                      select _Employee).ToList().Union(from _Employee in _context.Employees
                                                                           // join _branch in _context.Branches on _Employee.BranchId equals _branch.Id
                                                                       where _Employee.Active == true && _Employee.StopPayment == false && _Employee.ResignationCheck == true &&
                                                                       _Employee.ResignationDate >= DateFrom && _Employee.ResignationDate <= DateTo &&
                                                                       _Employee.DateofJoin <= DateTo && _Employee.OfficeWorker == Enums.Payroll.Office.ToString() &&
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

                    var vchecktimeview = _CheckTimeTable.vCheckTimeViewModel.Where(e => e.EmployeeId == _EmployeeRecord.Id).ToList();

                    var _presentDays = vchecktimeview.Where(x => x.CheckDate >= DateFrom && x.CheckDate <= DateTo && x.CheckDate >= _EmployeeRecord.DateofJoin && x.DayOff == false && x.HolidayGazette == false).Count();
                    //Day Off
                    var _DaysOff = vchecktimeview.Where(x => x.DayOff == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                    //Gazette Holidays
                    var _HolidaysGazette = vchecktimeview.Where(x => x.DayOff == false && x.HolidayGazette == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                    //Absent Approval
                    var _AbsentApproval = vchecktimeview.Where(x => x.DayOff == false && x.AbsentAdjustmentCheck == true && x.DailyDate >= _EmployeeRecord.DateofJoin && x.DailyDate <= (_EmployeeRecord.ResignationCheck == true ? _EmployeeRecord.ResignationDate : DateTo)).Count();
                    //Sandwitch Working
                    var _SandwitchBefore = vchecktimeview.Where(x => x.SandwitchBefore == true).Count();
                    var _SandwitchAfter = vchecktimeview.Where(x => x.SandwitchAfter == true).Count();

                    var _JoinDays = _EmployeeRecord.DateofJoin <= DateTo ? 0 : _EmployeeRecord.DateofJoin.Subtract(DateTo).Days;
                    var _ResignDays = !_EmployeeRecord.ResignationCheck ? 0 : DateTo.Subtract(_EmployeeRecord.ResignationDate).Days;
                    //Present
                    var _totalPersentDays = _presentDays + _DaysOff + _HolidaysGazette + _AbsentApproval - _SandwitchBefore - _SandwitchAfter;
                    //Absent
                    var _absentDays = _NoOfDaysMonth - (_totalPersentDays + _JoinDays + _ResignDays);
                    var _AbsentDaysApproval = vchecktimeview.Where(x => x.AbsentCheck == true).Count();

                    //OverTime
                    var _OverTime_Minutes = vchecktimeview.Where(x => x.OverTimeCheck == true && x.AttendanceExemptCheck == false).Sum(s => s.OverTime + s.OverTimeNight + s.EarlyOverTime - s.EarlyGoing);
                    var _OverTime_Hours = _OverTime_Minutes > 0 ? _OverTime_Minutes / 60 : 0;

                    //Late Days
                    var _Late_Hours = vchecktimeview.Where(x => x.LateCheck == true && x.AttendanceExemptCheck == false).Sum(s => s.LateComing) / 60;
                    var _Late_Days_Total = vchecktimeview.Where(x => x.LateComing > 0 && x.LateCheck == true && x.AttendanceExemptCheck == false).Count();
                    var _Late_Days = _Late_Days_Total >= 4 ? _Late_Days_Total / 4 : 0;
                    var _HalfDays_Adjustment = vchecktimeview.Where(x => x.LateCheck == true && x.AttendanceExemptCheck == false && x.LateComing > (x.RosterWorkingHoursMorning * 60)).Count() > 1 ? 1 : 0;
                    var _Late_Days_Actual = _Late_Days - _HalfDays_Adjustment;

                    int _AnnualLeaveCount = 0;
                    int _AnnualLeaveAdjustSalary = 0;
                    var _AnnualLeaveTable = await _context.AnnualLeaves.Where(a => a.Id == _EmployeeRecord.AnnualLeavesId && a.Date <= DateTo).OrderBy(r => r.Date).LastOrDefaultAsync();
                    if (_AnnualLeaveTable != null)
                    {

                        _AnnualLeaveCount = _AnnualLeaveTable.AnnualLeaveDays;
                    }

                    int AnnualLeaveAdjustment = await _context.AnnualLeaveAdjustments.Where(a => a.Date >= _AnnualLeaveDateFrom && a.Date <= DateTo && a.EmployeeId == _EmployeeRecord.Id && a.ApprovedAdjustType == Enums.Payroll.A.ToString()).SumAsync(s => s.LeaveAdjust);
                    int AnnualLeaveBalance = _AnnualLeaveCount - AnnualLeaveAdjustment <= 0 ? 0 : _AnnualLeaveCount - AnnualLeaveAdjustment;
                    if (AnnualLeaveBalance > 0)
                    {
                        _AnnualLeaveAdjustSalary = AnnualLeaveBalance - _Late_Days_Actual >= 0 ? _Late_Days_Actual : 0;
                    }
                    _Late_Days_Actual = _Late_Days_Actual - _AnnualLeaveAdjustSalary;

                    var _SalaryStaff = new SalaryProcessStaffModel
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
                    _SalaryProcessStaffModel.Add(_SalaryStaff);

                    //Annual Leave Adjustment by Salary
                    if (_AnnualLeaveAdjustSalary > 0)
                    {
                        var _AnnualLeaveAdjustmentsTable = new AnnualLeaveAdjustment();
                        _AnnualLeaveAdjustmentsTable.Date = DateTo;
                        _AnnualLeaveAdjustmentsTable.LeaveAdjust = _AnnualLeaveAdjustSalary;
                        _AnnualLeaveAdjustmentsTable.ApprovedAdjustType = Enums.Payroll.A.ToString();
                        _AnnualLeaveAdjustmentsTable.EmployeeId = _EmployeeRecord.Id;
                        _AnnualLeaveAdjustmentsTable.Type = Enums.Operations.S.ToString(); ;
                        _AnnualLeaveAdjustmentsTable.Action = Enums.Operations.A.ToString();
                        _AnnualLeaveAdjustmentsTable.CompanyId = _userInformation.CompanyId;// _Branch.CompanyId;
                        _AnnualLeaveAdjustmentsTable.InsertDate = DateTime.Now;
                        _AnnualLeaveAdjustmentsTable.UserNameInsert = _UserName;
                        await _context.AnnualLeaveAdjustments.AddAsync(_AnnualLeaveAdjustmentsTable);
                        _context.SaveChanges();
                    }
                }
                //Income Tax
                foreach (var _IncomeTax in _SalaryProcessStaffModel.Where(i => i.CheckIncomeTax == true))
                {
                    var _Key = _User.Claims.FirstOrDefault(x => x.Type == Enums.Misc.Key.ToString())?.Value.ToString();

                    //Get FinancialYear API
                    var _FinancialYearApiResponse = await _SecurityHelper.GetFinancialYearAsync(_Key);
                    if (_FinancialYearApiResponse.statusCode != StatusCodes.Status200OK.ToString()) { return _FinancialYearApiResponse; }
                    var _FinancialYearViewModel = (MSFinancialYearServicesViewModel)_FinancialYearApiResponse.data;

                    DateTime _year = new DateTime(_Date.Year, 1, 1);

                    var _EmployeeAllowances = await _context.EmployeeAllowances.Include(a => a.allowance).Where(x => x.EmployeeId == _IncomeTax.EmployeeId).SumAsync(s => s.allowance.Amount);
                    var _Working_months = _YearEndDate.Subtract(_IncomeTax.DateOfJoining).Days;
                    _Working_months = Convert.ToInt32(Math.Ceiling(_Working_months > 1 ? ((_Working_months + 1) / 30.436875) : (_Working_months / 30.436875)));

                    if (_Working_months > 12)
                    {
                        _Working_months = 12;
                    }
                    double _PreviousSalaryAmount = 0;
                    int _PreviousSalaryCount = 0;

                    var _PreviousSalary = await _context.StaffSalaries.Where(s => s.EmployeeId == _IncomeTax.EmployeeId && (s.Date > _year && s.Date < DateFrom)).ToListAsync();
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
                foreach (var _SalaryStaffTableRecord in _SalaryProcessStaffModel)
                {
                    //Salary 
                    _SalaryStaffTableRecord.SalaryPerDay = _SalaryStaffTableRecord.SalaryAmount / _SalaryStaffTableRecord.NoOfDaysMonth;
                    _SalaryStaffTableRecord.SalaryPerHour = _SalaryStaffTableRecord.SalaryPerDay / _SalaryStaffTableRecord.WorkingHours;
                    _SalaryStaffTableRecord.SalaryGrossPerMinute = _SalaryStaffTableRecord.SalaryPerHour / 60;

                    _SalaryStaffTableRecord.SalaryGrossPerDay = _SalaryStaffTableRecord.SalaryGrossAmount / _SalaryStaffTableRecord.NoOfDaysMonth;
                    _SalaryStaffTableRecord.SalaryGrossPerHour = _SalaryStaffTableRecord.SalaryGrossPerDay / _SalaryStaffTableRecord.WorkingHours;
                    _SalaryStaffTableRecord.SalaryGrossPerMinute = _SalaryStaffTableRecord.SalaryGrossPerHour / 60;

                    //Overtime Amount
                    var _OverTime_Rate = _SalaryStaffTableRecord.SalaryGrossPerHour * _SalaryStaffTableRecord.OvertimeRate;
                    var _OverTime_Amount = _SalaryStaffTableRecord.OvertimeActual * _OverTime_Rate;
                    _SalaryStaffTableRecord.OvertimeRate = _OverTime_Rate;
                    _SalaryStaffTableRecord.OvertimeActualAmount = _OverTime_Amount;

                    //Attendance Allownace Amount
                    var _AttendanceAllownace = _SalaryStaffTableRecord.PresentDays == _SalaryStaffTableRecord.NoOfDaysMonth && _SalaryStaffTableRecord.CheckAttendanceAllowance == true && _SalaryStaffTableRecord.AbsentDaysApproval == 0 && _SalaryStaffTableRecord.LateDaysTotal <= 4 ? (0.03 * _SalaryStaffTableRecord.SalaryGrossAmount) : 0;
                    _SalaryStaffTableRecord.AttendanceAllowanceAmount = _AttendanceAllownace;

                    //Late Deduction Amount
                    var _LateDeductionAmount = _SalaryStaffTableRecord.LateDaysActual * _SalaryStaffTableRecord.SalaryGrossPerDay;
                    _SalaryStaffTableRecord.LateDaysActualAmount = _LateDeductionAmount;

                    //Absent Days Actual Amount
                    _SalaryStaffTableRecord.AbsentDaysActualAmount = _SalaryStaffTableRecord.AbsentDaysActual * _SalaryStaffTableRecord.SalaryGrossPerDay;
                    //Payable Salary
                    _SalaryStaffTableRecord.Amount = _SalaryStaffTableRecord.ResignDays > 0 ? _SalaryStaffTableRecord.SalaryGrossPerDay * (_SalaryStaffTableRecord.PresentDays + _SalaryStaffTableRecord.AbsentDays) : _SalaryStaffTableRecord.JoinDays > 0 ? _SalaryStaffTableRecord.SalaryGrossPerDay * (_SalaryStaffTableRecord.PresentDays + _SalaryStaffTableRecord.AbsentDays) : _SalaryStaffTableRecord.SalaryGrossPerDay * _SalaryStaffTableRecord.NoOfDaysMonth;
                    _SalaryStaffTableRecord.GrossAmount = _SalaryStaffTableRecord.Amount + (_SalaryStaffTableRecord.AdditionAmount + _SalaryStaffTableRecord.AttendanceAllowanceAmount) - (_SalaryStaffTableRecord.Takaful + _SalaryStaffTableRecord.AdvanceAmount + _SalaryStaffTableRecord.LoanAmount + _SalaryStaffTableRecord.IncomeTaxAmount + _SalaryStaffTableRecord.DeductionAmount + _SalaryStaffTableRecord.LateDaysActualAmount + _SalaryStaffTableRecord.AbsentDaysActualAmount);
                    _SalaryStaffTableRecord.PayableAmount = _SalaryStaffTableRecord.GrossAmount + _SalaryStaffTableRecord.OvertimeActualAmount;

                }

                List<StaffSalary> _SalaryStaffTable = new List<StaffSalary>();
                foreach (var _SalaryProcessStaffModelRecord in _SalaryProcessStaffModel)
                {

                    _SalaryStaffTable.Add(new StaffSalary
                    {

                        EmployeeId = _SalaryProcessStaffModelRecord.EmployeeId,
                        Date = DateTo,
                        CheckIncomeTax = _SalaryProcessStaffModelRecord.CheckIncomeTax,
                        Takaful = _SalaryProcessStaffModelRecord.Takaful,
                        DateOfJoining = _SalaryProcessStaffModelRecord.DateOfJoining,
                        DateOfResign = _SalaryProcessStaffModelRecord.DateOfResign,
                        NoOfDaysMonth = _NoOfDaysMonth,
                        WorkingHours = _SalaryProcessStaffModelRecord.WorkingHours,
                        AttendanceExempted = _SalaryProcessStaffModelRecord.AttendanceExempted,
                        CheckAttendanceAllowance = _SalaryProcessStaffModelRecord.CheckAttendanceAllowance,
                        AttendanceAllowanceAmount = _SalaryProcessStaffModelRecord.AttendanceAllowanceAmount,
                        LateDaysActualAmount = _SalaryProcessStaffModelRecord.LateDaysActualAmount,
                        CheckOvertime = _SalaryProcessStaffModelRecord.CheckOvertime,
                        SalaryAmount = _SalaryProcessStaffModelRecord.SalaryAmount,
                        SalaryAllowanceAmount = _SalaryProcessStaffModelRecord.SalaryAllowanceAmount,
                        SalaryGrossAmount = _SalaryProcessStaffModelRecord.SalaryGrossAmount,
                        PresentDays = _SalaryProcessStaffModelRecord.PresentDays,
                        AbsentDays = _SalaryProcessStaffModelRecord.AbsentDays,
                        AllowanceAbsent = _SalaryProcessStaffModelRecord.AllowanceAbsent,
                        AdvanceAmount = _SalaryProcessStaffModelRecord.AdvanceAmount,
                        LoanAmount = _SalaryProcessStaffModelRecord.LoanAmount,
                        AdditionAmount = _SalaryProcessStaffModelRecord.AdditionAmount,
                        DeductionAmount = _SalaryProcessStaffModelRecord.DeductionAmount,

                        IncomeTaxAmount = _SalaryProcessStaffModelRecord.IncomeTaxAmount,

                        OvertimeMinutes = _SalaryProcessStaffModelRecord.OvertimeMinutes,
                        OvertimeHours = _SalaryProcessStaffModelRecord.OvertimeHours,
                        OvertimeDays = _SalaryProcessStaffModelRecord.OvertimeDays,
                        OvertimeRate = _SalaryProcessStaffModelRecord.OvertimeRate,
                        OvertimeActual = _SalaryProcessStaffModelRecord.OvertimeActual,
                        OvertimeActualAmount = _SalaryProcessStaffModelRecord.OvertimeActualAmount,

                        SalaryPerDay = _SalaryProcessStaffModelRecord.SalaryPerDay,
                        SalaryPerHour = _SalaryProcessStaffModelRecord.SalaryPerHour,
                        SalaryPerminute = _SalaryProcessStaffModelRecord.SalaryPerminute,

                        SalaryGrossPerDay = _SalaryProcessStaffModelRecord.SalaryGrossPerDay,
                        SalaryGrossPerHour = _SalaryProcessStaffModelRecord.SalaryGrossPerHour,
                        SalaryGrossPerMinute = _SalaryProcessStaffModelRecord.SalaryGrossPerMinute,

                        Amount = _SalaryProcessStaffModelRecord.Amount,
                        GrossAmount = _SalaryProcessStaffModelRecord.GrossAmount,
                        PayableAmount = _SalaryProcessStaffModelRecord.PayableAmount,

                        CompanyId = _SalaryProcessStaffModelRecord.CompanyId,
                        UserNameInsert = _CheckTimeTable.UserName,
                        InsertDate = DateTime.Now
                    });
                }

                await _context.StaffSalaries.AddRangeAsync(_SalaryStaffTable);
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