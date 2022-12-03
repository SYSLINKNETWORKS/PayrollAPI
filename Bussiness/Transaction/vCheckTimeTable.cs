using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Report;

namespace TWP_API_Payroll.Controllers.Payroll.Report
{
    public class vCheckTimeTable
    {
        private readonly DataContext _context = null;

        public vCheckTimeTable(DataContext context)
        {
            _context = context;
        }
        public async Task<vCheckTimeViewModelBaseModel> vCheckTimeAsync(UserLoginInfoBaseModel _UserLoginInfoBaseModel, PayrollReportCreteria _PayrollReportCreteria)
        {
            ApiResponse apiResponse = new ApiResponse();



            vCheckTimeViewModelBaseModel _vCheckTimeViewModelBaseModel = new vCheckTimeViewModelBaseModel();




            _vCheckTimeViewModelBaseModel.CompanyId = _UserLoginInfoBaseModel.CompanyId;
            _vCheckTimeViewModelBaseModel.CompanyName = _UserLoginInfoBaseModel.CompanyName;
            _vCheckTimeViewModelBaseModel.BranchId = _UserLoginInfoBaseModel.BranchId;
            _vCheckTimeViewModelBaseModel.BranchName = _UserLoginInfoBaseModel.BranchName;
            _vCheckTimeViewModelBaseModel.UserId = _UserLoginInfoBaseModel.UserId;
            _vCheckTimeViewModelBaseModel.UserName = _UserLoginInfoBaseModel.UserName;
            _vCheckTimeViewModelBaseModel.DailyDate = _PayrollReportCreteria.DateFrom;

            int _Year = _PayrollReportCreteria.DateFrom.Year;
            int _Month = _PayrollReportCreteria.DateFrom.Month;
            DateTime DtFrom = new DateTime(_Year, _Month, 1);
            DateTime DtTo = _PayrollReportCreteria.DateTo.AddMonths(1).AddDays(-1);
            var _HolidayRecordTable = await _context.Holidays.Where(x => x.Date >= DtFrom && x.Date <= DtTo).ToListAsync();
            if (_HolidayRecordTable.Count() == 0)
            {
                return _vCheckTimeViewModelBaseModel;
            }

            var _vCheckTimeView = await (from _DailyDates in _context.DailyDates
                                         from _Employee in _context.Employees.Where(e => e.Active == true)
                                         join _Roster in _context.Rosters on _Employee.RosterId equals _Roster.Id
                                         join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                         join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                         join _EmployeeCategory in _context.EmployeeCategories on _Employee.EmployeeCategoryId equals _EmployeeCategory.Id
                                         from _checkattendance in _context.CheckAttendances.Where(ct => ct.Date == _DailyDates.Date && ct.EmployeeId == _Employee.Id).DefaultIfEmpty()
                                         from _Holiday in _context.Holidays.Where(h => h.Date == _DailyDates.Date).DefaultIfEmpty()
                                         from _Absents in _context.Absents.Where(a => a.EmployeeId == _Employee.Id && a.Date == _DailyDates.Date).DefaultIfEmpty()
                                             //_Employee.branch.CompanyId == _UserLoginInfoBaseModel.CompanyId && 
                                         where _Employee.Active == true && _Employee.AttendanceExempt == false && _DailyDates.Date >= _PayrollReportCreteria.DateFrom && _DailyDates.Date <= _PayrollReportCreteria.DateTo &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
                                         (string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                         select new vCheckTimeViewModel
                                         {
                                             EmployeeId = _Employee.Id,
                                             EmployeeNo = _Employee.MachineId,
                                             EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                             EmployeeCategory = _EmployeeCategory.Name,
                                             OfficeWorker = _Employee.OfficeWorker,
                                             DesignationId = _Designation.Id,
                                             DesignationName = _Designation.Name,
                                             DepartmentId = _Department.Id,
                                             DepartmentName = _Department.Name,
                                             DailyDate = _DailyDates.Date,
                                             RosterId = _Employee.RosterId,
                                             RosterName = _Roster.Name,
                                             DateOfJoin = _Employee.DateofJoin,
                                             DateOfResign = _Employee.ResignationDate,
                                             ResignationCheck = _Employee.ResignationCheck,
                                             AttendanceExemptCheck = _Employee.AttendanceExempt,
                                             //HolidayCheck = _Holiday.HolidayCheck == null?false : _Holiday.HolidayCheck,
                                             HolidayCheck = _Holiday.HolidayCheck,
                                             OverTimeCheck = _Employee.OverTime,
                                             OverTimeHolidayCheck = _Employee.OverTimeHoliday,
                                             LateCheck = _Employee.LateDeduction,
                                             CheckDate = _checkattendance.Date,
                                             OutDate = _checkattendance.Out,
                                             InnTime = _checkattendance.Inn,
                                             OutTime = _checkattendance.Inn != _checkattendance.Out ? _checkattendance.Out : null,
                                             AttendanceMachineInnId = _checkattendance.attendanceMachineInn.Id,
                                             AttendanceMachineInnName = _checkattendance.attendanceMachineInn.Name == null ? "" : _checkattendance.attendanceMachineInn.Name,
                                             AttendanceMachineOutId = _checkattendance.attendanceMachineOut.Id,
                                             AttendanceMachineOutName = _checkattendance.attendanceMachineOut.Name == null ? "" : _checkattendance.attendanceMachineOut.Name,
                                             // WorkingHours = _checkattendance.Out == null ? 0 : Math.Round ((Convert.ToDateTime (_checkattendance.Out).Subtract (_checkattendance.Inn)).TotalHours, 2),
                                             AbsentCheck = String.IsNullOrEmpty(_Holiday.Remarks) && _checkattendance.Date == null ? true : false,
                                             AbsentAdjustmentCheck = _Absents.Approved ? true : false,
                                             AbsentAdjustmentCategory = _Absents.Approved ? _Absents.ApprovedAdjustType : "",
                                             Remarks = !String.IsNullOrEmpty(_Holiday.Remarks) ? _Holiday.Remarks : _Absents.Approved ? _Absents.ApprovedAdjustType : _checkattendance.Date == null ? Enums.Payroll.Absent.ToString() : "",
                                             //:_Employee.DateofJoin > _PayrollReportCreteria.DateFrom ? "Doj"
                                         }).ToListAsync();

            if (_vCheckTimeView == null)
            {
                return _vCheckTimeViewModelBaseModel;
                // ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();;
                // ApiResponse.message = "Record Not Found";
                // return ApiResponse;
            }

            if (_vCheckTimeView.ToList().Count == 0)
            {
                return _vCheckTimeViewModelBaseModel;
            }

            foreach (var _Record in _vCheckTimeView)
            {

                var _RosterGroups = await (_context.RosterGroups).Where(rg => rg.RosterId == _Record.RosterId && rg.Date <= _Record.DailyDate).OrderByDescending(rgo => rgo.Date).FirstOrDefaultAsync();
                if (_RosterGroups != null)
                {
                    bool _RosterCheck = _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Monday.ToString() ? _RosterGroups.MondayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() ? _RosterGroups.TuesdayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() ? _RosterGroups.WednesdayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() ? _RosterGroups.ThursdayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Friday.ToString() ? _RosterGroups.FridayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() ? _RosterGroups.SaturdayCheck : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() ? _RosterGroups.SundayCheck : false;


                    _Record.DayOff = _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && !_RosterGroups.MondayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && !_RosterGroups.TuesdayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && !_RosterGroups.WednesdayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && !_RosterGroups.ThursdayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && !_RosterGroups.FridayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && !_RosterGroups.SaturdayCheck ? true : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && !_RosterGroups.SundayCheck ? true : false;
                    _Record.RosterInn = _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroups.MondayCheck ? _RosterGroups.MondayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroups.TuesdayCheck ? _RosterGroups.TuesdayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroups.WednesdayCheck ? _RosterGroups.WednesdayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroups.ThursdayCheck ? _RosterGroups.ThursdayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroups.FridayCheck ? _RosterGroups.FridayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroups.SaturdayCheck ? _RosterGroups.SaturdayInn : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroups.SundayCheck ? _RosterGroups.SundayInn : null;
                    _Record.RosterOut = _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroups.MondayCheck ? _RosterGroups.MondayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroups.TuesdayCheck ? _RosterGroups.TuesdayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroups.WednesdayCheck ? _RosterGroups.WednesdayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroups.ThursdayCheck ? _RosterGroups.ThursdayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroups.FridayCheck ? _RosterGroups.FridayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroups.SaturdayCheck ? _RosterGroups.SaturdayOut : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroups.SundayCheck ? _RosterGroups.SundayOut : null;
                    _Record.RosterLate = _RosterGroups.Late;
                    _Record.RosterEarly = _RosterGroups.EarlyGoing;
                    _Record.RosterOverTime = _RosterGroups.OverTime;
                    _Record.WorkingHours = _RosterGroups.WorkingHours;
                    _Record.RosterWorkingHours = _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Monday.ToString() ? _RosterGroups.MondayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() ? _RosterGroups.TuesdayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() ? _RosterGroups.WednesdayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() ? _RosterGroups.ThursdayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Friday.ToString() ? _RosterGroups.FridayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() ? _RosterGroups.SaturdayWorkingHours : _Record.DailyDate.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() ? _RosterGroups.SundayWorkingHours : 0;
                    _Record.RosterWorkingHoursMorning = _RosterGroups.MorningWorkingHours;
                    _Record.RosterWorkingHoursEvening = _RosterGroups.EveningWorkingHours;
                    _Record.AbsentCheck = !_RosterCheck ? _RosterCheck : _Record.AbsentCheck;
                    _Record.Remarks = !string.IsNullOrEmpty(_Record.AbsentAdjustmentCategory) ? _Record.AbsentAdjustmentCategory == Enums.Payroll.A.ToString() ? Enums.Payroll.AnnualLeave.ToString() : _Record.AbsentAdjustmentCategory == Enums.Payroll.C.ToString() ? Enums.Payroll.CasualLeave.ToString() : "Not Ajusted" : !_RosterCheck ? _Record.DailyDate.DayOfWeek.ToString() : _Record.Remarks;
                }
                _Record.HolidayGazette = !_Record.DayOff ? _Record.HolidayCheck : false;

            }
            //Roster InnOut
            foreach (var _Record in _vCheckTimeView)
            {
                _Record.RosterInn = _Record.InnTime.HasValue && _Record.RosterInn.HasValue ? Convert.ToDateTime(_Record.InnTime.Value.ToString("yyyy-MM-dd") + " " + _Record.RosterInn.Value.ToString("HH:mm")) : null;
                _Record.RosterOut = _Record.OutTime.HasValue && _Record.RosterOut.HasValue ? Convert.ToDateTime(_Record.OutTime.Value.ToString("yyyy-MM-dd") + " " + _Record.RosterOut.Value.ToString("HH:mm")) : null;

            }
            //Late Coming
            foreach (var _Record in _vCheckTimeView)
            {
                _Record.LateComing = _Record.LateCheck && _Record.HolidayCheck == false && _Record.InnTime.HasValue && _Record.RosterInn.HasValue ? Convert.ToInt32(Math.Floor(_Record.InnTime.Value.Subtract(_Record.RosterInn.Value.AddMinutes(_Record.RosterLate)).TotalMinutes)) > 0 ? Convert.ToInt32(Math.Floor(_Record.InnTime.Value.Subtract(_Record.RosterInn.Value).TotalMinutes)) : 0 : 0;
                _Record.RosterWorkingHours = _Record.HolidayCheck ? 0 : _Record.RosterWorkingHours;
                _Record.OverTime = _Record.HolidayCheck && _Record.InnTime.HasValue && _Record.OutTime.HasValue ? Convert.ToInt32(Math.Floor(_Record.OutTime.Value.Subtract(_Record.InnTime.Value).TotalMinutes)) : _Record.OutTime.HasValue && _Record.RosterOut.HasValue ? Convert.ToInt32(Math.Floor(_Record.OutTime.Value.Subtract(_Record.RosterOut.Value.AddMinutes(_Record.RosterOverTime)).TotalMinutes)) > 0 ? Convert.ToInt32(Math.Floor(_Record.OutTime.Value.Subtract(_Record.RosterOut.Value).TotalMinutes)) : 0 : 0;
                _Record.EarlyGoing = !_Record.HolidayCheck && _Record.InnTime.HasValue && _Record.OutTime.HasValue && _Record.OfficeWorker == Enums.Payroll.Worker.ToString() ? Convert.ToInt32(Math.Floor(_Record.RosterOut.Value.Subtract(_Record.OutTime.Value).TotalMinutes)) > 0 ? Convert.ToInt32(Math.Floor(_Record.RosterOut.Value.Subtract(_Record.OutTime.Value).TotalMinutes)) : 0 : 0;

            }
            //Overtime & Early Going
            foreach (var _Record in _vCheckTimeView)
            {
                double _LateMins = 0;
                if (_Record.LateCheck)
                {
                    if (((_Record.LateComing / 30) - (Math.Floor(_Record.LateComing / 30))) > 0.3)
                    {
                        _LateMins = (Math.Floor((_Record.LateComing - _Record.RosterLate) / 30) * 30) + 30;
                    }
                    else if (((_Record.LateComing / 30) - (Math.Floor(_Record.LateComing / 30))) > 0)
                    {
                        _LateMins = (Math.Floor(_Record.LateComing / 30) * 30);
                    }
                    else if (((_Record.LateComing / 30) - (Math.Floor(_Record.LateComing / 30))) == 0)
                    {
                        _LateMins = _Record.LateComing;
                    }

                }
                _Record.EarlyOverTime = 0;
                _Record.LateComing = _LateMins;
                var OvertimeMinutes = (Math.Floor(_Record.OverTime / 30)) > 0 ? (Math.Floor(_Record.OverTime / 30)) * 30 : 0;
                var NightOvertimeTable = await _context.NightOverTimes.Where(x => x.Action != Enums.Operations.D.ToString() && x.EmployeeId == _Record.EmployeeId && x.Date == _Record.DailyDate && x.Approved == true).ToListAsync();
                var NightOvertimeMinute = NightOvertimeTable.Sum(x => x.OverTime) * 60;
                // var OvertimeMinutesAllow = 0;
                // OvertimeMinutesAllow = _Record.DayOff && _Record.OverTimeHolidayCheck?true : false;
                // OvertimeMinutesAllow = _Record.HolidayCheck && _Record.OverTimeHolidayCheck?true : false;
                //                _Record.OverTime = _Record.DayOff && _Record.OverTimeHolidayCheck?OvertimeMinutes : _Record.HolidayCheck && _Record.OverTimeHolidayCheck?OvertimeMinutes: !_Record.DayOff?OvertimeMinutes : 0;

                _Record.OverTime = !_Record.HolidayCheck ? OvertimeMinutes : _Record.HolidayCheck && _Record.OverTimeHolidayCheck ? OvertimeMinutes : 0; // _Record.HolidayCheck && _Record.OverTimeHolidayCheck?OvertimeMinutes : !_Record.HolidayCheck ?OvertimeMinutes: 0;
                _Record.EarlyGoing = _Record.OfficeWorker == Enums.Payroll.Worker.ToString() && (Math.Floor(_Record.EarlyGoing / 30)) > 0 ? (Math.Floor(_Record.EarlyGoing / 30)) * 30 : 0;
                _Record.OverTimeNight = NightOvertimeMinute;
            }
            //Total OverTime
            foreach (var _Record in _vCheckTimeView)
            {
                _Record.TotalOverTime = (_Record.OverTime + _Record.EarlyOverTime + _Record.OverTimeNight) - (_Record.LateComing + _Record.EarlyGoing);
            }

            //Sandwitch 
            foreach (var _Record in _vCheckTimeView.Where(x => x.Remarks == Enums.Payroll.Absent.ToString()))
            {

                var _SandwitchBeforeTable = _vCheckTimeView.Where(x => x.EmployeeId == _Record.EmployeeId && (x.DayOff == true || x.HolidayGazette == true) && x.SandwitchAfter == false && x.DailyDate >= _Record.DateOfJoin && x.DailyDate <= (_Record.ResignationCheck ? _Record.DateOfResign : _PayrollReportCreteria.DateTo) && x.DailyDate == _Record.DailyDate.AddDays(-1) && _Record.Remarks == Enums.Payroll.Absent.ToString()).FirstOrDefault();
                var _SandwitchAfterTable = _vCheckTimeView.Where(x => x.EmployeeId == _Record.EmployeeId && (x.DayOff == true || x.HolidayGazette == true) && x.SandwitchAfter == false && x.SandwitchBefore == false && x.DailyDate >= _Record.DateOfJoin && x.DailyDate <= (_Record.ResignationCheck ? _Record.DateOfResign : _PayrollReportCreteria.DateTo) && x.DailyDate == _Record.DailyDate.AddDays(1) && _Record.Remarks == Enums.Payroll.Absent.ToString()).FirstOrDefault();
                if (_SandwitchBeforeTable != null) { _SandwitchBeforeTable.SandwitchBefore = true; }
                if (_SandwitchAfterTable != null) { _SandwitchAfterTable.SandwitchAfter = true; }
            }

            //Sandwitch Gazette Holiday Before
            foreach (var _Record in _vCheckTimeView.Where(x => x.DayOff == false && x.HolidayCheck == true && x.SandwitchBefore == true && x.SandwitchAfter == false))
            {
                var _DtHoliday = _Record.DailyDate.AddDays(-1);
                if (_HolidayRecordTable.Where(h => h.Date == _DtHoliday && h.HolidayCheck == true).FirstOrDefault() != null)
                {
                  //  _vCheckTimeView.Where(x => x.DailyDate == _DtHoliday && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault().SandwitchBefore = true;
                  var table1 = _vCheckTimeView.Where(x => x.DailyDate == _DtHoliday && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault();
                    bool val = table1 != null ? true : false;
                    if (val)
                    {
                        table1.SandwitchBefore = val;
                    }
                }
                else { break; }
            }

            //Sandwitch Gazette Holiday After
            foreach (var _Record in _vCheckTimeView.Where(x => x.DayOff == false && x.HolidayCheck == true && x.SandwitchBefore == true && x.SandwitchAfter == false))
            {
                var _DtHoliday = _Record.DailyDate.AddDays(1);
                if (_HolidayRecordTable.Where(h => h.Date == _DtHoliday && h.HolidayCheck == true).FirstOrDefault() != null)
                {
                    _vCheckTimeView.Where(x => x.DailyDate == _DtHoliday && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault().SandwitchBefore = true;
                }
                else { break; }
            }
            // var _HolidayTable = (from _vCheckTimeTable in _vCheckTimeView where _vCheckTimeTable.DayOff == false && _vCheckTimeTable.HolidayCheck == true select _vCheckTimeTable.DailyDate)
            //     .Distinct ().ToList ();

            // //Sandwitch Gazette Holiday Before
            // foreach (var _Record in _vCheckTimeView.Where (x => x.DayOff == false && x.HolidayCheck == true && x.SandwitchBefore == true && x.SandwitchAfter == false)) {
            //     //                var _HolidayTableBefore = _vCheckTimeView.Where (x => x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).ToList ();
            //     //                foreach (var _HolidayRecord in _HolidayTableBefore) {
            //     //                    var _SandwitchBeforeTable = _vCheckTimeView.Where (x => x.DailyDate == _HolidayRecord.DailyDate && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault ().SandwitchBefore = true;
            //     //               }
            //     // foreach (var _HolidayTableRecord in _HolidayTable) {
            //     // DateTime _Date1=new DateTime().sub

            //     DateTime _DtHoliday = _Record.DailyDate.AddDays (-1);
            //     foreach (var _HolidayRecord in _HolidayTable) {
            //         if (_DtHoliday == _HolidayRecord) {
            //             _vCheckTimeView.Where (x => x.DailyDate == _DtHoliday && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault ().SandwitchBefore = true;
            //         }
            //         _DtHoliday = _HolidayRecord.AddDays (-1);
            //     }

            //     // }
            // }

            // //Sandwitch Gazette Holiday After
            // foreach (var _Record in _vCheckTimeView.Where (x => x.DayOff == false && x.HolidayCheck == true && x.SandwitchBefore == false && x.SandwitchAfter == true)) {

            //     // var _HolidayTableAfter = _vCheckTimeView.Where (x => x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).ToList ();
            //     // foreach (var _HolidayRecord in _HolidayTableAfter) {
            //     DateTime _DtHoliday = _Record.DailyDate.AddDays (1);
            //     foreach (var _HolidayRecord in _HolidayTable) {
            //         if (_DtHoliday == _HolidayRecord) {
            //             var _SandwitchAfterTable = _vCheckTimeView.Where (x => x.DailyDate == _DtHoliday && x.DayOff == false && x.HolidayCheck == true && x.EmployeeId == _Record.EmployeeId).FirstOrDefault ().SandwitchAfter = true;
            //         }
            //         _DtHoliday = _HolidayRecord.AddDays (1);
            //     }

            // }
            //Sandwitch before and after true
            foreach (var _Record in _vCheckTimeView.Where(x => x.SandwitchBefore == true && x.SandwitchAfter == true))
            {
                _Record.SandwitchAfter = false;

            }

            var _SandwitchBeforeTable1 = _vCheckTimeView.Where(x => x.SandwitchBefore == true).ToList();
            var _SandwitchAfterTable1 = _vCheckTimeView.Where(x => x.SandwitchAfter == true).ToList();

            _vCheckTimeViewModelBaseModel.vCheckTimeViewModel = _vCheckTimeView;

            return _vCheckTimeViewModelBaseModel;
        }

    }
}