using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Bussiness.Payroll.Transaction
{
    public class AttendanceProcess
    {

        SecurityHelper _SecurityHelper = new SecurityHelper();

        //  private ErrorLog _ErrorLog = new ErrorLog ();
        public async Task<ApiResponse> AttendanceProcessAsync(CheckAttendanceBaseModel _CheckAttendanceBaseModel)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                // string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();


                // ApiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                // if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                // var _BranchViewModel = (List<BranchViewModel>)ApiResponse.data;

                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .Build();
                string _connectionString = configuration.GetConnectionString("Connection").ToString();

                var options = new DbContextOptionsBuilder<DataContext>().UseSqlServer(_connectionString).Options;
                _CheckAttendanceBaseModel.DateFrom = Convert.ToDateTime(Convert.ToDateTime(_CheckAttendanceBaseModel.DateFrom).ToString("yyyy/MM/dd"));
                _CheckAttendanceBaseModel.DateTo = Convert.ToDateTime(Convert.ToDateTime(_CheckAttendanceBaseModel.DateTo).ToString("yyyy/MM/dd"));

                using (var _context = new DataContext(options))
                {

                    var _MachineIdTable = await (from _CheckInOuts in _context.CheckInOuts.Where(x => x.Approved == true &&
                       (_CheckAttendanceBaseModel.MachineId == 0 ? true : x.MachineId == _CheckAttendanceBaseModel.MachineId) &&
                       x.Date >= _CheckAttendanceBaseModel.DateFrom && x.Date <= _CheckAttendanceBaseModel.DateTo)
                                                 join _Employee in _context.Employees on _CheckInOuts.MachineId equals _Employee.MachineId
                                                 select new { MachineId = _CheckInOuts.MachineId }).Distinct().ToListAsync();
                    if (_MachineIdTable == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Attendance not found";
                        return ApiResponse;

                    }
                    if (_MachineIdTable.Count() == 0)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Attendance not found";
                        return ApiResponse;
                    }
                    foreach (var _MachineId in _MachineIdTable)
                    {
                        _CheckAttendanceBaseModel.MachineId = _MachineId.MachineId;

                        var _EmployeeTable = await _context.Employees.Where(x => x.MachineId == _CheckAttendanceBaseModel.MachineId).FirstOrDefaultAsync();

                        if (_EmployeeTable == null)
                        {
                            ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                            ApiResponse.message = "Invalid Employee # " + _MachineId.MachineId.ToString();
                            return ApiResponse;
                        }
                        var _CheckInOutsTableRemove = await _context.CheckAttendances.Where(x => x.EmployeeId == _EmployeeTable.Id && x.Date >= _CheckAttendanceBaseModel.DateFrom && x.Date <= _CheckAttendanceBaseModel.DateTo).ToListAsync();
                        if (_CheckInOutsTableRemove.Count() > 0)
                        {
                            foreach (var item in _CheckInOutsTableRemove)
                            {
                                _context.CheckAttendances.Remove(item);
                                await _context.SaveChangesAsync();
                            }
                        }

                        var _CheckInOutsTable = await (from _CheckInOuts in _context.CheckInOuts.Where(x => x.Approved == true && x.MachineId == _CheckAttendanceBaseModel.MachineId && x.Date >= _CheckAttendanceBaseModel.DateFrom && x.Date <= _CheckAttendanceBaseModel.DateTo) select new { MachineId = _CheckInOuts.MachineId, Date = _CheckInOuts.Date }).Distinct().ToListAsync();
                        if (_CheckInOutsTable == null)
                        {
                            ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                            ApiResponse.message = "Record not found in CheckInOut Table";
                            return ApiResponse;
                        }

                        CheckAttendance _CheckAttendances = new CheckAttendance();
                        foreach (var item in _CheckInOutsTable)
                        {
                            var _RosterGroupsTable = await _context.RosterGroups.Where(rg => rg.RosterId == _EmployeeTable.RosterId && rg.Date <= item.Date).OrderByDescending(rgo => rgo.Date).FirstOrDefaultAsync();
                            DateTime _RosterInn = new DateTime();
                            DateTime _RosterOut = new DateTime();
                            bool _RosterCheck = false;
                            if (_RosterGroupsTable != null)
                            {

                                _RosterCheck = item.Date.DayOfWeek.ToString() == Enums.Days.Monday.ToString() ? _RosterGroupsTable.MondayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() ? _RosterGroupsTable.TuesdayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() ? _RosterGroupsTable.WednesdayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() ? _RosterGroupsTable.ThursdayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Friday.ToString() ? _RosterGroupsTable.FridayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() ? _RosterGroupsTable.SaturdayCheck : item.Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() ? _RosterGroupsTable.SundayCheck : false;
                                _RosterInn = item.Date.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroupsTable.MondayCheck ? _RosterGroupsTable.MondayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroupsTable.TuesdayCheck ? _RosterGroupsTable.TuesdayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroupsTable.WednesdayCheck ? _RosterGroupsTable.WednesdayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroupsTable.ThursdayCheck ? _RosterGroupsTable.ThursdayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroupsTable.FridayCheck ? _RosterGroupsTable.FridayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroupsTable.SaturdayCheck ? _RosterGroupsTable.SaturdayInn : item.Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroupsTable.SundayCheck ? _RosterGroupsTable.SundayInn : new DateTime();
                                _RosterOut = item.Date.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroupsTable.MondayCheck ? _RosterGroupsTable.MondayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroupsTable.TuesdayCheck ? _RosterGroupsTable.TuesdayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroupsTable.WednesdayCheck ? _RosterGroupsTable.WednesdayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroupsTable.ThursdayCheck ? _RosterGroupsTable.ThursdayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroupsTable.FridayCheck ? _RosterGroupsTable.FridayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroupsTable.SaturdayCheck ? _RosterGroupsTable.SaturdayOut : item.Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroupsTable.SundayCheck ? _RosterGroupsTable.SundayOut : new DateTime();
                            }
                            var _RosterDiff = DateTime.Compare(_RosterOut, _RosterInn);
                            DateTime _Checkinout_inn = new DateTime();
                            DateTime _Checkinout_out = new DateTime();
                            Guid? _MachineInn = new Guid(), _MachineOut = new Guid();
                            //   if (_RosterCheck)
                            //{
                            if (_RosterDiff < 0)
                            {
                                // var _Checkinout_inn_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == item.Date).OrderByDescending(d => d.CheckTime).FirstOrDefaultAsync());
                                // _Checkinout_inn = _Checkinout_inn_Table.CheckTime;
                                // _MachineInn = _Checkinout_inn_Table.AttendanceMachineId;

                                // var _Checkinout_out_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == item.Date).OrderBy(d => d.CheckTime).FirstOrDefaultAsync());
                                // _Checkinout_out = _Checkinout_out_Table.CheckTime;
                                // _MachineOut = _Checkinout_out_Table.AttendanceMachineId;

                                var _Checkinout_inn_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == item.Date).OrderByDescending(d => d.CheckTime).FirstOrDefaultAsync());
                                if (_Checkinout_inn_Table != null)
                                {
                                    _Checkinout_inn = _Checkinout_inn_Table.CheckTime;
                                    _MachineInn = _Checkinout_inn_Table.AttendanceMachineId;
                                    _MachineOut = _Checkinout_inn_Table.AttendanceMachineId;
                                }

                                DateTime _Dateout = item.Date.AddDays(1);
                                var _Checkinout_out_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == _Dateout).OrderBy(d => d.CheckTime).FirstOrDefaultAsync());
                                if (_Checkinout_out_Table != null)
                                {
                                    _Checkinout_out = _Checkinout_out_Table.CheckTime;
                                    _MachineOut = _Checkinout_out_Table.AttendanceMachineId;
                                }

                            }
                            else
                            {
                                var _Checkinout_inn_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == item.Date).OrderBy(d => d.CheckTime).FirstOrDefaultAsync());
                                if (_Checkinout_inn_Table != null)
                                {
                                    _Checkinout_inn = _Checkinout_inn_Table.CheckTime;
                                    _MachineInn = _Checkinout_inn_Table.AttendanceMachineId;
                                    _MachineOut = _Checkinout_inn_Table.AttendanceMachineId;
                                }

                                var _Checkinout_out_Table = (await _context.CheckInOuts.Where(x => x.MachineId == item.MachineId && x.Approved == true && x.Date == item.Date).OrderByDescending(d => d.CheckTime).FirstOrDefaultAsync());
                                if (_Checkinout_out_Table != null)
                                {
                                    _Checkinout_out = _Checkinout_out_Table.CheckTime;
                                    _MachineOut = _Checkinout_out_Table.AttendanceMachineId.HasValue ? _Checkinout_out_Table.AttendanceMachineId.Value : null;
                                }

                            }

                            _CheckAttendances.EmployeeId = _EmployeeTable.Id;
                            _CheckAttendances.Date = item.Date;
                            _CheckAttendances.Inn = _Checkinout_inn;
                            _CheckAttendances.Out = _Checkinout_out;
                            _CheckAttendances.AttendanceMachineIdInn = _MachineInn;
                            _CheckAttendances.AttendanceMachineIdOut = _MachineOut;
                            _CheckAttendances.Approved = true;
                            _CheckAttendances.CompanyId = _EmployeeTable.CompanyId;// _CheckAttendanceBaseModel. _BranchViewModel.Where(x => x.BranchId == _EmployeeTable.BranchId).FirstOrDefault().CompanyId;
                            _CheckAttendances.InsertDate = DateTime.Now;

                            _context.CheckAttendances.Add(_CheckAttendances);
                            await _context.SaveChangesAsync();
                        }

                        // }
                    }
                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    ApiResponse.message = "Attendance Process Complete";
                    return ApiResponse;
                }
            }
            catch (Exception e)
            {
                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                // string _ErrorId = await _ErrorLog.LogError ("", innerexp, Enums.ErrorType.Error.ToString (), _UserId);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = innerexp;
                return ApiResponse;
            }
        }

    }
}