using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Bussiness.Payroll.Transaction;
using TWP_API_Payroll.Controllers.Payroll.Report;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Dashboard;
using TWP_API_Payroll.ViewModels.Payroll.Report;
using TWP_API_Payroll.ViewModels.Report;

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollDashBoardSevicesRepository
    {

        Task<ApiResponse> GetAttendanceMachineIpAsync(Guid GroupId);
        Task<ApiResponse> PostCheckInOutAsync(CheckInOutMachineListModel _CheckInOutMachineListModel);
        Task<ApiResponse> ProcessAttendanceAsync(CheckAttendanceBaseModel _CheckAttendanceBaseModel);

        Task<ApiResponse> GetAttendanceAsync(ClaimsPrincipal _User, string _TokenString, DateTime _AttendanceDate);
        Task<ApiResponse> GetInOutMapAsync(ClaimsPrincipal _User, DateTime _inOutDate);
        Task<ApiResponse> GetLocationAsync(ClaimsPrincipal _User, DashboardCreteriaViewModel _DashboardCreteriaViewModel);
    }
    public class PayrollDashBoardSevicesRepository : IPayrollDashBoardSevicesRepository
    {
        private readonly DataContext _context = null;
        private readonly AttendanceProcess _AttendanceProcess = null;
        private NotificationService _NotificationService = null;
        SecurityHelper _SecurityHelper = new SecurityHelper();

        vCheckTimeTable _vCheckTimeTable;
        public PayrollDashBoardSevicesRepository(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
            _AttendanceProcess = new AttendanceProcess();
            _NotificationService = new NotificationService();
        }

        public async Task<ApiResponse> GetAttendanceMachineIpAsync(Guid GroupId)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                // string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();

                // ApiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                // if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                // var _BranchInfo = (List<BranchViewModel>)ApiResponse.data;


                string _type = "I";
                //.Include(b => b.branch)
                var _Table = await _context.AttendanceMachines.Include(g => g.attendanceMachineGroup).Where(a => a.Active == true && a.Action != Enums.Operations.D.ToString() && a.AttendanceMachineGroupId == GroupId).ToListAsync();
                if (_Table == null)
                {

                    await _NotificationService.sendnotification("1", "W", DateTime.Now, "No machine registered", Enums.NotificationMessageCategory.AttendanceMachine.ToString());

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_Table.ToList().Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                List<AttendanceMachineInfo> _AttendanceMachineInfo = new List<AttendanceMachineInfo>();
                foreach (var item in _Table)
                {
                    // _AttendanceMachineInfo.Add(new AttendanceMachineInfo { Id = item.Id, Name = item.Name.Trim(), Ip = item.Ip.Trim(), Port = item.Port, BranchId = item.BranchId, BranchName = _BranchInfo.Where(x => x.BranchId == item.BranchId).FirstOrDefault().BranchName });
                    _AttendanceMachineInfo.Add(new AttendanceMachineInfo { Id = item.Id, Name = item.Name.Trim(), Ip = item.Ip.Trim(), Port = item.Port, AttendanceMachineGroupId = item.AttendanceMachineGroupId, AttendanceMachineGroupName = item.attendanceMachineGroup.Name });
                    await _NotificationService.sendnotification("1", _type, DateTime.Now, item.Name, Enums.NotificationMessageCategory.AttendanceMachine.ToString());
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _AttendanceMachineInfo;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();

                await _NotificationService.sendnotification("1", "E", DateTime.Now, innerexp, Enums.NotificationMessageCategory.AttendanceMachine.ToString());

                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = innerexp;
                return ApiResponse;
            }
        }

        public async Task<ApiResponse> PostCheckInOutAsync(CheckInOutMachineListModel _CheckInOutMachineListModel)
        {
            var ApiResponse = new ApiResponse();
            int rowcnt = 0;
            try
            {
                // string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();

                // ApiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                // if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                // var _BranchViewModel = (List<BranchViewModel>)ApiResponse.data;


                var _CheckInOutsMachine = _CheckInOutMachineListModel.CheckInOutMachineModels.OrderBy(o => o.UserId).ToList();
                var _CheckInOutsTable = await (from _CheckInOuts in _context.CheckInOuts where _CheckInOuts.Type == Enums.Operations.S.ToString() select new CheckInOutMachineModel { UserId = _CheckInOuts.MachineId, MachineId = _CheckInOuts.AttendanceMachineId.HasValue ? _CheckInOuts.AttendanceMachineId.Value : null, CheckTime = _CheckInOuts.CheckTime.ToString("yyyy-MM-dd HH:mm:ss") }).OrderBy(o => o.UserId).ToListAsync();
                var _CheckInOutDiffRecord = (from _cm in _CheckInOutsMachine join _ct in _CheckInOutsTable on new(_cm.UserId, _cm.CheckTime, _cm.MachineId) equals (_ct.UserId, _ct.CheckTime, _ct.MachineId) into g where g.Count() != 1 select _cm).ToList();
                List<AttendanceProcessTable> _AttendanceProcessTable = new List<AttendanceProcessTable>();
                CheckAttendanceBaseModel _CheckAttendanceBaseModel = new CheckAttendanceBaseModel();
                if (_CheckInOutDiffRecord.Count() > 0)
                {
                    foreach (var item in _CheckInOutDiffRecord)
                    {
                       
                        string _address = "";
                        if (item.Latitude > 0 && item.Longitude > 0)
                        {
                            var httpClient = new HttpClient();
                            var json = await httpClient.GetStringAsync("https://maps.google.com/maps/api/geocode/xml?key=AIzaSyCtDFEsamLKPvhrJtuCCZ_iFPzWy1kHpCs&latlng=" + item.Latitude + "," + item.Longitude + "&sensor=false");
                            var responseXml = new XmlDocument();
                            responseXml.LoadXml(json);
                            _address = responseXml.SelectSingleNode("//formatted_address").InnerText;
                        }



                        var _EmployeeTable = await (from _Employee in _context.Employees.Where(x => x.MachineId == item.UserId) select new { CompanyId = _Employee.CompanyId, EmployeeName = _Employee.Name + " " + _Employee.FatherName }).FirstOrDefaultAsync();
                        if (_EmployeeTable != null)
                        {
                            //     ApiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                            //     ApiResponse.data = "Invalid Employee";
                            //     return ApiResponse;
                            // }

                            var _CheckInOutDiff = new CheckInOut();
                            _CheckInOutDiff.Action = Enums.Operations.A.ToString();
                            _CheckInOutDiff.Approved = true;
                            _CheckInOutDiff.Type = Enums.Operations.S.ToString();
                            _CheckInOutDiff.CheckType = Enums.Operations.I.ToString();
                            _CheckInOutDiff.CheckTime = Convert.ToDateTime(item.CheckTime);
                            _CheckInOutDiff.Latitude = item.Latitude;
                            _CheckInOutDiff.Longitude = item.Longitude;
                            _CheckInOutDiff.Address = _address;
                            _CheckInOutDiff.Date = Convert.ToDateTime(Convert.ToDateTime(item.CheckTime).ToString("yyyy-MM-dd"));
                            _CheckInOutDiff.AttendanceMachineId = item.MachineId;
                            _CheckInOutDiff.MachineId = item.UserId;
                            _CheckInOutDiff.CompanyId = _EmployeeTable.CompanyId;// _BranchViewModel.Where(x => x.BranchId == _AttendanceMachine.BranchId).FirstOrDefault().CompanyId;

                            await _context.CheckInOuts.AddAsync(_CheckInOutDiff);
                            _context.SaveChanges();
                            _AttendanceProcessTable.Add(new AttendanceProcessTable { MachineId = _CheckInOutDiff.MachineId, Date = _CheckInOutDiff.Date });

                            _CheckAttendanceBaseModel.MachineId = _CheckInOutDiff.MachineId;
                            _CheckAttendanceBaseModel.DateFrom = Convert.ToDateTime(item.CheckTime);
                            _CheckAttendanceBaseModel.DateTo = Convert.ToDateTime(item.CheckTime);
                            await _AttendanceProcess.AttendanceProcessAsync(_CheckAttendanceBaseModel);

                            await _NotificationService.sendnotification("1", "", Convert.ToDateTime(item.CheckTime), _EmployeeTable.EmployeeName, Enums.NotificationMessageCategory.Attendance.ToString());
                            rowcnt += 1;
                        }
                    }
                }
                // var _AttendanceProcessTableDistinct = (from m in _AttendanceProcessTable group m by new { m.Date } into mygroup select mygroup.FirstOrDefault ()).Distinct (); //_AttendanceProcessTable.DistictBy(row => row.Text);// .GroupBy(x=>x.CheckTime).Select(s=>s.FirstOrDefault());
                // foreach (var item in _AttendanceProcessTableDistinct) {
                //     CheckAttendanceBaseModel _CheckAttendanceBaseModel = new CheckAttendanceBaseModel ();
                //     _CheckAttendanceBaseModel.MachineId = item.MachineId;
                //     _CheckAttendanceBaseModel.DateFrom = item.Date;
                //     _CheckAttendanceBaseModel.DateTo = item.Date;
                //     await _AttendanceProcess.AttendanceProcessAsync (_CheckAttendanceBaseModel);
                // }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = " Record insert " + rowcnt.ToString() + " out of " + _CheckInOutsMachine.Count().ToString();
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = e.Message.ToString() + innerexp;
                return ApiResponse;
            }
        }

        public async Task<ApiResponse> ProcessAttendanceAsync(CheckAttendanceBaseModel _CheckAttendanceBaseModel)
        {
            var ApiResponse = new ApiResponse();
            AttendanceProcess _AttendanceProcess = new AttendanceProcess();
            ApiResponse = await _AttendanceProcess.AttendanceProcessAsync(_CheckAttendanceBaseModel);
            return ApiResponse;
        }

        public async Task<ApiResponse> GetAttendanceAsync(ClaimsPrincipal _User, string _TokenString, DateTime _AttendanceDate)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                // ApiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                // if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                // var _BranchInfo = (List<BranchViewModel>)ApiResponse.data;

                //var _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                //var _userInformation = await _context.UserLoginAudits.Where(x => x.Key == _UserKey.ToString()).Include(u => u.ApplicationUsers).Include(b => b.Branches).Include(c => c.Companies).Include(y => y.FinancialYears).FirstOrDefaultAsync();

                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;


                PayrollReportCreteria _PayrollReportCreteria = new PayrollReportCreteria();
                _PayrollReportCreteria.DateFrom = _AttendanceDate;
                _PayrollReportCreteria.DateTo = _AttendanceDate;

                //Employee
                //a.branch.CompanyId == _userInformation.CompanyId &&
                var _Table = await _context.Employees.Where(a => a.Active == true && a.ResignationCheck == false && a.AttendanceExempt == false && a.DateofJoin <= _AttendanceDate).ToListAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_Table.ToList().Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                //Machine Information
                //.Include(b => b.branch).Where(m => m.branch.CompanyId == _userInformation.CompanyId)
                // _AttendanceMachinesInfo.branch.Name
                List<AttendanceMachineInfo> _AttendanceMachineInfo = new List<AttendanceMachineInfo>();
                //string _BranchName = "";
                var _AttendanceMachine = await _context.AttendanceMachines.ToListAsync();
                foreach (var _AttendanceMachineRecord in _AttendanceMachine)
                {
                    _AttendanceMachineInfo.Add(new AttendanceMachineInfo
                    {
                        Id = _AttendanceMachineRecord.Id,
                        Name = _AttendanceMachineRecord.Name.Trim(),
                        Ip = _AttendanceMachineRecord.Ip.Trim(),
                        Port = _AttendanceMachineRecord.Port,
                        // BranchId = _AttendanceMachineRecord.BranchId,
                        // BranchName = _BranchInfo.Where(x => x.BranchId == _AttendanceMachineRecord.BranchId).FirstOrDefault().BranchName,

                    });
                }

                // (from _AttendanceMachinesInfo in _context.AttendanceMachines select new AttendanceMachineInfo { Id = _AttendanceMachinesInfo.Id, Name = _AttendanceMachinesInfo.Name.Trim(), Ip = _AttendanceMachinesInfo.Ip.Trim(), Port = _AttendanceMachinesInfo.Port, BranchId = _AttendanceMachinesInfo.BranchId, BranchName = _BranchInfo.Where(x => x.BranchId == _AttendanceMachinesInfo.BranchId).FirstOrDefault().BranchName }).ToListAsync();
                //Attendance

                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);
                //  var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

                if (vCheckTimeViewModelBaseModel.vCheckTimeViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (vCheckTimeViewModelBaseModel.vCheckTimeViewModel.ToList().Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                var _TimeSheetViewModelBaseModel = new TimeSheetViewModelBaseModel();

                //Present
                var _TablePresent = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(x => x.InnTime != null && x.ResignationCheck == false)
                                     select new PresentViewModel
                                     {
                                         EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                         EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                         EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                         DesignationId = _vCheckTimeViewModel.DesignationId,
                                         DesignationName = _vCheckTimeViewModel.DesignationName,
                                         DepartmentId = _vCheckTimeViewModel.DepartmentId,
                                         DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                         AttendanceMachineName = _vCheckTimeViewModel.AttendanceMachineInnName,
                                         InnTime = _vCheckTimeViewModel.InnTime,
                                         OutTime = _vCheckTimeViewModel.OutTime,
                                     }).ToList();

                //Absent
                var _TableAbsent = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(x => x.AbsentCheck == true && x.ResignationCheck == false)
                                    select new AbsentViewModel
                                    {
                                        EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                        EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                        EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                        DesignationId = _vCheckTimeViewModel.DesignationId,
                                        DesignationName = _vCheckTimeViewModel.DesignationName,
                                        DepartmentId = _vCheckTimeViewModel.DepartmentId,
                                        DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                        Remarks = _vCheckTimeViewModel.AbsentAdjustmentCheck ? _vCheckTimeViewModel.Remarks : ""
                                    }).ToList();

                //Late
                var _TableLate = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(x => x.LateComing > 0 && x.ResignationCheck == false)
                                  select new LateViewModel
                                  {
                                      EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                      EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                      EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                      DesignationId = _vCheckTimeViewModel.DesignationId,
                                      DesignationName = _vCheckTimeViewModel.DesignationName,
                                      DepartmentId = _vCheckTimeViewModel.DepartmentId,
                                      DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                      InnTime = _vCheckTimeViewModel.InnTime,
                                      LateMinutes = _vCheckTimeViewModel.LateComing,
                                  }).ToList();

                var _DashboardViewModel = new DashboardViewModel();
                _DashboardViewModel.TotalStrength = _Table.Count;
                _DashboardViewModel.attendanceMachineInfos = _AttendanceMachineInfo;
                _DashboardViewModel.present = _TablePresent;
                _DashboardViewModel.absent = _TableAbsent;
                _DashboardViewModel.late = _TableLate;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _DashboardViewModel;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = e.Message.ToString() + innerexp;
                return ApiResponse;
            }
        }

        public async Task<ApiResponse> GetInOutMapAsync(ClaimsPrincipal _User, DateTime inOutDate)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CheckInOutTable = await (from _CheckInOut in _context.CheckInOuts.Where(a => a.Date == inOutDate && a.Latitude > 0)
                                              join _Employee in _context.Employees on _CheckInOut.MachineId equals _Employee.MachineId
                                              join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                              join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                              select new InOutMapViewModel
                                              {
                                                  EmployeeId = _Employee.Id,
                                                  EmployeeNo = _Employee.MachineId,
                                                  EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                                  DesignationId = _Employee.DesignationId,
                                                  DesignationName = _Designation.Name,
                                                  DepartmentId = _Employee.DepartmentId,
                                                  DepartmentName = _Department.Name,
                                              }).Distinct().ToListAsync();

                if (_CheckInOutTable == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CheckInOutTable.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CheckInOutTable;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = e.Message.ToString() + innerexp;
                return ApiResponse;
            }
        }

        public async Task<ApiResponse> GetLocationAsync(ClaimsPrincipal _User, DashboardCreteriaViewModel _DashboardCreteriaViewModel)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CheckInOutTable = await (from _CheckInOut in _context.CheckInOuts.Where(a => a.Date == _DashboardCreteriaViewModel.date && a.MachineId == _DashboardCreteriaViewModel.MachineId && a.Latitude > 0)
                                              join _Employee in _context.Employees on _CheckInOut.MachineId equals _Employee.MachineId
                                              join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                              join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                              select new InOutMapViewModel
                                              {
                                                  EmployeeId = _Employee.Id,
                                                  EmployeeNo = _Employee.MachineId,
                                                  EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                                  DesignationId = _Employee.DesignationId,
                                                  DesignationName = _Designation.Name,
                                                  DepartmentId = _Employee.DepartmentId,
                                                  DepartmentName = _Department.Name,
                                                  CheckTime = _CheckInOut.CheckTime,
                                                  CheckType = _CheckInOut.CheckType == Enums.Payroll.I.ToString() ? Enums.Payroll.Inn.ToString() : _CheckInOut.CheckType == Enums.Payroll.O.ToString() ? Enums.Payroll.Out.ToString() : "",
                                                  Approved = _CheckInOut.Approved,
                                                  Latitude = _CheckInOut.Latitude,
                                                  Longitude = _CheckInOut.Longitude,
                                                  Address = _CheckInOut.Address

                                              }).ToListAsync();

                if (_CheckInOutTable == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CheckInOutTable.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CheckInOutTable;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = e.Message.ToString() + innerexp;
                return ApiResponse;
            }
        }
    }
}