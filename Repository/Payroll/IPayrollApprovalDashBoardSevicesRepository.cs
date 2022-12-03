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
    public interface IPayrollApprovalDashBoardSevicesRepository
    {

        Task<ApiResponse> GetInOutEditorApprovalLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User);
        Task<ApiResponse> GetNightOverTimeLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User);
        Task<ApiResponse> UpdateNightOverTimeLovAsync(UpdateNightOverTimeDashboardViewModel _request, ClaimsPrincipal _User);
        Task<ApiResponse> GetAdvanceLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User);
        Task<ApiResponse> UpdateAdvanceLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User);

        Task<ApiResponse> GetLoanLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User);
        Task<ApiResponse> UpdateLoanLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User);
        Task<ApiResponse> GetEmployeeLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User);
        Task<ApiResponse> UpdateEmployeeLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User);


        // Task<ApiResponse> GetAttendanceMachineIpAsync(Guid GroupId);
        // Task<ApiResponse> PostCheckInOutAsync(CheckInOutMachineListModel _CheckInOutMachineListModel);
        // Task<ApiResponse> ProcessAttendanceAsync(CheckAttendanceBaseModel _CheckAttendanceBaseModel);

        // Task<ApiResponse> GetAttendanceAsync(ClaimsPrincipal _User, string _TokenString, DateTime _AttendanceDate);
        // Task<ApiResponse> GetInOutMapAsync(ClaimsPrincipal _User, DateTime _inOutDate);
        // Task<ApiResponse> GetLocationAsync(ClaimsPrincipal _User, DashboardCreteriaViewModel _DashboardCreteriaViewModel);
    }
    public class PayrollApprovalDashBoardSevicesRepository : IPayrollApprovalDashBoardSevicesRepository
    {
        private readonly DataContext _context = null;
        private readonly AttendanceProcess _AttendanceProcess = null;
        private NotificationService _NotificationService = null;
        SecurityHelper _SecurityHelper = new SecurityHelper();

        vCheckTimeTable _vCheckTimeTable;
        public PayrollApprovalDashBoardSevicesRepository(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
            _AttendanceProcess = new AttendanceProcess();
            _NotificationService = new NotificationService();
        }
        public async Task<ApiResponse> GetInOutEditorApprovalLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_DashboardFilterViewModel.MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Filter = false;
                DateTime _DateFrom = DateTime.Now;
                DateTime _DateTo = DateTime.Now; ;

                if ((!string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom)))
                {
                    _DateFrom = Convert.ToDateTime(_DashboardFilterViewModel.DateFrom);
                    _DateTo = string.IsNullOrEmpty(_DashboardFilterViewModel.DateTo) ? Convert.ToDateTime(_DashboardFilterViewModel.DateFrom) : Convert.ToDateTime(_DashboardFilterViewModel.DateTo);
                    _Filter = true;
                }
                if (!string.IsNullOrEmpty(_DashboardFilterViewModel.No)) { _Filter = true; }




                var _InOutEditorApprovalDashboardViewModel = await (from _CheckInOutTable in _context.CheckInOuts
                                                                    join _EmployeeTable in _context.Employees on _CheckInOutTable.MachineId equals _EmployeeTable.MachineId
                                                                    where (_CheckInOutTable.Action != Enums.Operations.D.ToString() &&
                                         //&& _CheckInOutTable.Date >= _DateFrom && _CheckInOutTable.Date <= _DateTo &&
                                         _CheckInOutTable.Type == Enums.Operations.U.ToString()
                                           && (string.IsNullOrEmpty(_DashboardFilterViewModel.No) ? true : (_EmployeeTable.MachineId.ToString() == _DashboardFilterViewModel.No))
                                          && (string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom) ? true : (_CheckInOutTable.Date >= _DateFrom && _CheckInOutTable.Date <= _DateTo))
                                          && (_Filter ? true : ((_CheckInOutTable.Approved == false)))
                                         )
                                                                    select new InOutEditorApprovalDashboardViewModel
                                                                    {
                                                                        MachineId = _CheckInOutTable.MachineId,
                                                                        EmployeeName = _EmployeeTable.Name.Trim() + ' ' + _EmployeeTable.FatherName.Trim(),
                                                                        Date = Convert.ToDateTime(Convert.ToDateTime(_CheckInOutTable.CheckTime).ToString("yyyy-MM-dd")),
                                                                        CheckTime = _CheckInOutTable.CheckTime,
                                                                        Approved = _CheckInOutTable.Approved,
                                                                    }).OrderByDescending(o => o.CheckTime).ToListAsync();

                if (_InOutEditorApprovalDashboardViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_InOutEditorApprovalDashboardViewModel.Count() == 0)
                {

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorApprovalDashboardViewModel;
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
        public async Task<ApiResponse> GetNightOverTimeLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_DashboardFilterViewModel.MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Filter = false;
                DateTime _DateFrom = DateTime.Now;
                DateTime _DateTo = DateTime.Now; ;

                if ((!string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom)))
                {
                    _DateFrom = Convert.ToDateTime(_DashboardFilterViewModel.DateFrom);
                    _DateTo = string.IsNullOrEmpty(_DashboardFilterViewModel.DateTo) ? Convert.ToDateTime(_DashboardFilterViewModel.DateFrom) : Convert.ToDateTime(_DashboardFilterViewModel.DateTo);
                    _Filter = true;
                }
                if (!string.IsNullOrEmpty(_DashboardFilterViewModel.No)) { _Filter = true; }




                var _InOutEditorApprovalDashboardViewModel = await (from _NightOverTimesTable in _context.NightOverTimes
                                                                    join _EmployeeTable in _context.Employees on _NightOverTimesTable.EmployeeId equals _EmployeeTable.Id
                                                                    where (_NightOverTimesTable.Action != Enums.Operations.D.ToString()
                                           && (string.IsNullOrEmpty(_DashboardFilterViewModel.No) ? true : (_EmployeeTable.MachineId.ToString() == _DashboardFilterViewModel.No))
                                          && (string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom) ? true : (_NightOverTimesTable.Date >= _DateFrom && _NightOverTimesTable.Date <= _DateTo))
                                          && (_Filter ? true : ((_NightOverTimesTable.Approved == false)))
                                         )
                                                                    select new NightOverTimeDashboardViewModel
                                                                    {
                                                                        EmployeeId = _EmployeeTable.Id,
                                                                        MachineId = _EmployeeTable.MachineId,
                                                                        EmployeeName = _EmployeeTable.Name.Trim() + ' ' + _EmployeeTable.FatherName.Trim(),
                                                                        Date = _NightOverTimesTable.Date,
                                                                        Hours = _NightOverTimesTable.OverTime,
                                                                        Approved = _NightOverTimesTable.Approved,
                                                                    }).OrderByDescending(o => o.Date).ToListAsync();

                if (_InOutEditorApprovalDashboardViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_InOutEditorApprovalDashboardViewModel.Count() == 0)
                {

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorApprovalDashboardViewModel;
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

        public async Task<ApiResponse> UpdateNightOverTimeLovAsync(UpdateNightOverTimeDashboardViewModel _request, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse apiResponse = new ApiResponse();
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
                DateTime _Date = Convert.ToDateTime(_request.Date);

                var _Table = await _context.NightOverTimes.Include(m => m.employee).Where(x => x.EmployeeId == _request.Id && x.Date == _Date).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                bool _Approved = _request.Approved == "1" ? true : false;
                if (_Table.Approved != _Approved)
                {
                    _Table.Approved = _Approved;
                    _Table.OverTime = _request.Hours;
                    _Table.UserNameApproved = _Approved ? _UserName : "";
                    _Table.DateApproved = DateTime.Now;
                }

                _context.NightOverTimes.Update(_Table);
                await _context.SaveChangesAsync();

                string _status = _Approved ? Enums.NotificationStatus.Approved.ToString() : "";
                if (!string.IsNullOrEmpty(_status))
                {
                    string _msg = "Night over time for employee " + _Table.employee.Name.ToString() + " " + _status;
                    await _NotificationService.sendnotification("1", "", Convert.ToDateTime(_Table.DateApproved), _msg, Enums.NotificationMessageCategory.NightOverTime.ToString());
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Record Updated";
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

        //Advance
        public async Task<ApiResponse> GetAdvanceLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_DashboardFilterViewModel.MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Filter = false;
                DateTime _DateFrom = DateTime.Now;
                DateTime _DateTo = DateTime.Now; ;

                if ((!string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom)))
                {
                    _DateFrom = Convert.ToDateTime(_DashboardFilterViewModel.DateFrom);
                    _DateTo = string.IsNullOrEmpty(_DashboardFilterViewModel.DateTo) ? Convert.ToDateTime(_DashboardFilterViewModel.DateFrom) : Convert.ToDateTime(_DashboardFilterViewModel.DateTo);
                    _Filter = true;
                }
                if (!string.IsNullOrEmpty(_DashboardFilterViewModel.No)) { _Filter = true; }




                var _InOutEditorApprovalDashboardViewModel = await (from _AdvancesTable in _context.Advances
                                                                    join _EmployeeTable in _context.Employees on _AdvancesTable.EmployeeId equals _EmployeeTable.Id
                                                                    join _Department in _context.Departments on _EmployeeTable.DepartmentId equals _Department.Id
                                                                    join _Designation in _context.Designations on _EmployeeTable.DesignationId equals _Designation.Id
                                                                    where (_AdvancesTable.Action != Enums.Operations.D.ToString()
                                           && (string.IsNullOrEmpty(_DashboardFilterViewModel.No) ? true : (_EmployeeTable.MachineId.ToString() == _DashboardFilterViewModel.No))
                                          && (string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom) ? true : (_AdvancesTable.Date >= _DateFrom && _AdvancesTable.Date <= _DateTo))
                                          && (_Filter ? true : ((_AdvancesTable.Approved == false)))
                                         )
                                                                    select new AdvanceDashboardViewModel
                                                                    {
                                                                        Id = _AdvancesTable.Id,
                                                                        MachineId = _EmployeeTable.MachineId,
                                                                        EmployeeName = _EmployeeTable.Name.Trim() + ' ' + _EmployeeTable.FatherName.Trim(),
                                                                        DepartmentName = _Department.Name,
                                                                        DesignationName = _Designation.Name,
                                                                        Date = _AdvancesTable.Date,
                                                                        Amount = _AdvancesTable.Amount,
                                                                        Approved = _AdvancesTable.Approved,
                                                                    }).OrderByDescending(o => o.Date).ToListAsync();

                if (_InOutEditorApprovalDashboardViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_InOutEditorApprovalDashboardViewModel.Count() == 0)
                {

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorApprovalDashboardViewModel;
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

        public async Task<ApiResponse> UpdateAdvanceLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse apiResponse = new ApiResponse();
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                var _Table = await _context.Advances.Include(m => m.Employee).Where(x => x.Id == _request.Id).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                bool _Approved = _request.Approved == "1" ? true : false;
                if (_Table.Approved != _Approved)
                {
                    _Table.Approved = _Approved;
                    _Table.UserNameApproved = _Approved ? _UserName : "";
                    _Table.DateApproved = DateTime.Now;
                }

                _context.Advances.Update(_Table);
                await _context.SaveChangesAsync();

                string _status = _Approved ? Enums.NotificationStatus.Approved.ToString() : "";
                if (!string.IsNullOrEmpty(_status))
                {
                    string _msg = "Advance for employee " + _Table.Employee.Name.ToString() + " " + _status;
                    await _NotificationService.sendnotification("1", "", Convert.ToDateTime(_Table.DateApproved), _msg, Enums.NotificationMessageCategory.Advance.ToString());
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Record Updated";
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

        //Loan
        public async Task<ApiResponse> GetLoanLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_DashboardFilterViewModel.MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Filter = false;
                DateTime _DateFrom = DateTime.Now;
                DateTime _DateTo = DateTime.Now; ;

                if ((!string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom)))
                {
                    _DateFrom = Convert.ToDateTime(_DashboardFilterViewModel.DateFrom);
                    _DateTo = string.IsNullOrEmpty(_DashboardFilterViewModel.DateTo) ? Convert.ToDateTime(_DashboardFilterViewModel.DateFrom) : Convert.ToDateTime(_DashboardFilterViewModel.DateTo);
                    _Filter = true;
                }
                if (!string.IsNullOrEmpty(_DashboardFilterViewModel.No)) { _Filter = true; }




                var _InOutEditorApprovalDashboardViewModel = await (from _LoanIssueTable in _context.LoanIssues
                                                                    join _EmployeeTable in _context.Employees on _LoanIssueTable.EmployeeId equals _EmployeeTable.Id
                                                                    join _Department in _context.Departments on _EmployeeTable.DepartmentId equals _Department.Id
                                                                    join _Designation in _context.Designations on _EmployeeTable.DesignationId equals _Designation.Id
                                                                    where (_LoanIssueTable.Action != Enums.Operations.D.ToString()
                                           && (string.IsNullOrEmpty(_DashboardFilterViewModel.No) ? true : (_EmployeeTable.MachineId.ToString() == _DashboardFilterViewModel.No))
                                          && (string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom) ? true : (_LoanIssueTable.Date >= _DateFrom && _LoanIssueTable.Date <= _DateTo))
                                          && (_Filter ? true : ((_LoanIssueTable.Approved == false)))
                                         )
                                                                    select new LoanDashboardViewModel
                                                                    {
                                                                        Id = _LoanIssueTable.Id,
                                                                        MachineId = _EmployeeTable.MachineId,
                                                                        EmployeeName = _EmployeeTable.Name.Trim() + ' ' + _EmployeeTable.FatherName.Trim(),
                                                                        DesignationName = _Designation.Name,
                                                                        DepartmentName = _Department.Name,
                                                                        Date = _LoanIssueTable.Date,
                                                                        Amount = _LoanIssueTable.Amount,
                                                                        Approved = _LoanIssueTable.Approved,
                                                                    }).OrderByDescending(o => o.Date).ToListAsync();

                if (_InOutEditorApprovalDashboardViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_InOutEditorApprovalDashboardViewModel.Count() == 0)
                {

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorApprovalDashboardViewModel;
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

        public async Task<ApiResponse> UpdateLoanLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse apiResponse = new ApiResponse();
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                var _Table = await _context.LoanIssues.Include(m => m.Employee).Where(x => x.Id == _request.Id).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                bool _Approved = _request.Approved == "1" ? true : false;
                if (_Table.Approved != _Approved)
                {
                    _Table.Approved = _Approved;
                    _Table.UserNameApproved = _Approved ? _UserName : "";
                    _Table.DateApproved = DateTime.Now;
                }

                _context.LoanIssues.Update(_Table);
                await _context.SaveChangesAsync();

                string _status = _Approved ? Enums.NotificationStatus.Approved.ToString() : "";
                if (!string.IsNullOrEmpty(_status))
                {
                    string _msg = "Loan for employee " + _Table.Employee.Name.ToString() + " " + _status;
                    await _NotificationService.sendnotification("1", "", Convert.ToDateTime(_Table.DateApproved), _msg, Enums.NotificationMessageCategory.Loan.ToString());
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Record Updated";
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

        //Employee
        public async Task<ApiResponse> GetEmployeeLovAsync(DashboardFilterViewModel _DashboardFilterViewModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_DashboardFilterViewModel.MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Filter = false;
                DateTime _DateFrom = DateTime.Now;
                DateTime _DateTo = DateTime.Now; ;

                if ((!string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom)))
                {
                    _DateFrom = Convert.ToDateTime(_DashboardFilterViewModel.DateFrom);
                    _DateTo = string.IsNullOrEmpty(_DashboardFilterViewModel.DateTo) ? Convert.ToDateTime(_DashboardFilterViewModel.DateFrom) : Convert.ToDateTime(_DashboardFilterViewModel.DateTo);
                    _Filter = true;
                }
                if (!string.IsNullOrEmpty(_DashboardFilterViewModel.No)) { _Filter = true; }




                var _InOutEditorApprovalDashboardViewModel = await (from _EmployeeTable in _context.Employees
                                                                    join _Department in _context.Departments on _EmployeeTable.DepartmentId equals _Department.Id
                                                                    join _Designation in _context.Designations on _EmployeeTable.DesignationId equals _Designation.Id
                                                                    where (_EmployeeTable.Action != Enums.Operations.D.ToString()
                                           && (string.IsNullOrEmpty(_DashboardFilterViewModel.No) ? true : (_EmployeeTable.MachineId.ToString() == _DashboardFilterViewModel.No))
                                          && (string.IsNullOrEmpty(_DashboardFilterViewModel.DateFrom) ? true : (_EmployeeTable.DateofJoin >= _DateFrom && _EmployeeTable.DateofJoin <= _DateTo))
                                          && (_Filter ? true : ((_EmployeeTable.Approved == false)))
                                         )
                                                                    select new EmployeeDashboardViewModel
                                                                    {
                                                                        Id = _EmployeeTable.Id,
                                                                        MachineId = _EmployeeTable.MachineId,
                                                                        EmployeeName = _EmployeeTable.Name.Trim() + ' ' + _EmployeeTable.FatherName.Trim(),
                                                                        DepartmentName = _Department.Name,
                                                                        DesignationName = _Designation.Name,
                                                                        Date = _EmployeeTable.DateofJoin,
                                                                        Approved = _EmployeeTable.Approved,
                                                                    }).OrderByDescending(o => o.Date).ToListAsync();

                if (_InOutEditorApprovalDashboardViewModel == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_InOutEditorApprovalDashboardViewModel.Count() == 0)
                {

                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorApprovalDashboardViewModel;
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

        public async Task<ApiResponse> UpdateEmployeeLovAsync(UpdateDashboardViewModel _request, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse apiResponse = new ApiResponse();
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                var _Table = await _context.Employees.Where(x => x.Id == _request.Id).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                bool _Approved = _request.Approved == "1" ? true : false;
                if (_Table.Approved != _Approved)
                {
                    _Table.Approved = _Approved;
                    _Table.UserNameApproved = _Approved ? _UserName : "";
                    _Table.DateApproved = DateTime.Now;
                }

                _context.Employees.Update(_Table);
                await _context.SaveChangesAsync();

                string _status = _Approved ? Enums.NotificationStatus.Approved.ToString() : "";
                if (!string.IsNullOrEmpty(_status))
                {
                    string _msg = "employee " + _Table.Name.ToString() + " " + _status;
                    await _NotificationService.sendnotification("1", "", Convert.ToDateTime(_Table.DateApproved), _msg, Enums.NotificationMessageCategory.Employee.ToString());
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Record Updated";
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