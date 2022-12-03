using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Controllers.Payroll.Report;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Report;
using TWP_API_Payroll.ViewModels.Report;

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollReportSevicesRepository
    {

        Task<ApiResponse> GetSingleDayAttandanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetDailyAttandanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetTimeSheetAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetOverTimeAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetAbsentAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);

        Task<ApiResponse> GetEmployeeProfileAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetEmployeeProfileDocumentAsync(string _TokenString, Guid _EmployeeId);

        Task<ApiResponse> GetInOutEditorAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);

        Task<ApiResponse> GetAdvanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);

        Task<ApiResponse> GetLoanAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);

        Task<ApiResponse> GetLoanReceiveAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetLeaveAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);

        Task<ApiResponse> GetSalaryIncrementAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetSalaryRegisterWorkerAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetSalaryRegisterStaffAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetSalaryPaySlipAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
        Task<ApiResponse> GetCurrentSalariesAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria);
    }
    public class PayrollReportSevicesRepository : IPayrollReportSevicesRepository
    {
        private readonly DataContext _context = null;
        SecurityHelper _SecurityHelper = new SecurityHelper();

        DateTime _date = Convert.ToDateTime(DateTime.Now.ToString("dd-MMM-yyyy"));
        vCheckTimeTable _vCheckTimeTable;
        public PayrollReportSevicesRepository(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
        }

        public async Task<ApiResponse> GetSingleDayAttandanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {


                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;


                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

                SingleDayAttandanceBaseModel _SingleDayAttandanceBaseModel = new SingleDayAttandanceBaseModel();

                _SingleDayAttandanceBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                _SingleDayAttandanceBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                _SingleDayAttandanceBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;
                _SingleDayAttandanceBaseModel.DailyDate = _PayrollReportCreteria.DateFrom;
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

                var _TableSingleDayAttandance = from _TableVCheckTime1 in vCheckTimeViewModelBaseModel.vCheckTimeViewModel
                                                join _Employee in _context.Employees on _TableVCheckTime1.EmployeeId equals _Employee.Id
                                                select new SingleDayAttandance
                                                {
                                                    EmployeeId = _Employee.Id,
                                                    EmployeeNo = _Employee.MachineId,
                                                    EmployeeTemporaryPermanent = _Employee.TemporaryPermanent,
                                                    EmployeeName = _Employee.Name,
                                                    DepartmentName = _TableVCheckTime1.DepartmentName,
                                                    DesignationName = _TableVCheckTime1.DesignationName,
                                                    EmployeeCategory = _TableVCheckTime1.EmployeeCategory,
                                                    DailyDate = _TableVCheckTime1.DailyDate,
                                                    OutDate = _TableVCheckTime1.OutDate,
                                                    InnTime = _TableVCheckTime1.InnTime,
                                                    OutTime = _TableVCheckTime1.OutTime,
                                                    Remarks = _TableVCheckTime1.Remarks

                                                };

                _SingleDayAttandanceBaseModel.SingleDayAttandances = _TableSingleDayAttandance.ToList();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _SingleDayAttandanceBaseModel;
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
        public async Task<ApiResponse> GetDailyAttandanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;


                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

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
                var _DailyAttendanceBaseModel = new DailyAttendanceBaseModel();
                _DailyAttendanceBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                _DailyAttendanceBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                _DailyAttendanceBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _DailyAttendanceBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                _DailyAttendanceBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;

                var _DailyAttendaceView = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel
                                           select new DailyAttendanceLists
                                           {
                                               EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                               EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                               EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                               DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                               DesignationName = _vCheckTimeViewModel.DesignationName,
                                               DailyDate = _vCheckTimeViewModel.DailyDate,
                                               RosterId = _vCheckTimeViewModel.RosterId,
                                               RosterName = _vCheckTimeViewModel.RosterName,
                                               RosterInn = _vCheckTimeViewModel.RosterInn,
                                               RosterOut = _vCheckTimeViewModel.RosterOut,
                                               InnTime = _vCheckTimeViewModel.InnTime,
                                               OutDate = _vCheckTimeViewModel.OutDate,
                                               OutTime = _vCheckTimeViewModel.OutTime,
                                               WorkingHours = _vCheckTimeViewModel.WorkingHours,
                                               LateComingMin = _vCheckTimeViewModel.LateComing,
                                               LateComingHr = (_vCheckTimeViewModel.LateComing / 60),
                                               OverTimeMin = _vCheckTimeViewModel.OverTime,
                                               OverTimehr = (_vCheckTimeViewModel.OverTime / 60),
                                               OverTimeNightMin = _vCheckTimeViewModel.OverTimeNight,
                                               OverTimeNighthr = (_vCheckTimeViewModel.OverTimeNight / 60),
                                               TotalOverTime = _vCheckTimeViewModel.TotalOverTime,
                                               Remarks = _vCheckTimeViewModel.Remarks
                                           }).Where(x => x.Remarks != Enums.Payroll.Absent.ToString()).ToList();

                _DailyAttendanceBaseModel.DailyAttendanceLists = _DailyAttendaceView;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _DailyAttendanceBaseModel;
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
        public async Task<ApiResponse> GetTimeSheetAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

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
                _TimeSheetViewModelBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                _TimeSheetViewModelBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                _TimeSheetViewModelBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _TimeSheetViewModelBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                _TimeSheetViewModelBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;

                var _TimeSheetView = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel
                                      select new TimeSheetViewModel
                                      {
                                          EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                          EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                          EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                          EmployeeCategory = _vCheckTimeViewModel.EmployeeCategory,
                                          DesignationName = _vCheckTimeViewModel.DesignationName,
                                          DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                          DailyDate = _vCheckTimeViewModel.DailyDate,
                                          RosterId = _vCheckTimeViewModel.RosterId,
                                          RosterName = _vCheckTimeViewModel.RosterName,
                                          RosterInn = _vCheckTimeViewModel.RosterInn,
                                          RosterOut = _vCheckTimeViewModel.RosterOut,
                                          RosterEarly = _vCheckTimeViewModel.RosterEarly,
                                          RosterLate = _vCheckTimeViewModel.RosterLate,
                                          RosterOverTime = _vCheckTimeViewModel.RosterOverTime,
                                          HolidayCheck = _vCheckTimeViewModel.HolidayCheck,
                                          OverTimeCheck = _vCheckTimeViewModel.OverTimeCheck,
                                          LateCheck = _vCheckTimeViewModel.LateCheck,
                                          InnTime = _vCheckTimeViewModel.InnTime,
                                          OutTime = _vCheckTimeViewModel.OutTime,
                                          RosterWorkingHours = _vCheckTimeViewModel.RosterWorkingHours,
                                          WorkingHours = _vCheckTimeViewModel.WorkingHours,
                                          AbsentAdjustmentCategory = _vCheckTimeViewModel.AbsentAdjustmentCategory,
                                          LateComing = _vCheckTimeViewModel.LateComing,
                                          OverTime = _vCheckTimeViewModel.OverTime,
                                          OverTimeNight = _vCheckTimeViewModel.OverTimeNight,
                                          EarlyGoing = _vCheckTimeViewModel.EarlyGoing,
                                          EarlyOverTime = _vCheckTimeViewModel.EarlyOverTime,
                                          TotalOverTime = _vCheckTimeViewModel.TotalOverTime,
                                          AbsentCheck = _vCheckTimeViewModel.AbsentCheck,
                                          Remarks = _vCheckTimeViewModel.Remarks
                                      }).ToList();

                //                var _TimeSheetView = new TimeSheetViewModel ();

                _TimeSheetViewModelBaseModel.TimeSheetViewModels = _TimeSheetView;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _TimeSheetViewModelBaseModel;
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

        public async Task<ApiResponse> GetOverTimeAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;


                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

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
                _TimeSheetViewModelBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                _TimeSheetViewModelBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                _TimeSheetViewModelBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _TimeSheetViewModelBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                _TimeSheetViewModelBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;

                var _TimeSheetView = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel
                                      select new TimeSheetViewModel
                                      {
                                          EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                          EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                          EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                          EmployeeCategory = _vCheckTimeViewModel.EmployeeCategory,
                                          DesignationName = _vCheckTimeViewModel.DesignationName,
                                          DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                          DailyDate = _vCheckTimeViewModel.DailyDate,
                                          RosterId = _vCheckTimeViewModel.RosterId,
                                          RosterName = _vCheckTimeViewModel.RosterName,
                                          RosterInn = _vCheckTimeViewModel.RosterInn,
                                          RosterOut = _vCheckTimeViewModel.RosterOut,
                                          RosterEarly = _vCheckTimeViewModel.RosterEarly,
                                          RosterLate = _vCheckTimeViewModel.RosterLate,
                                          RosterOverTime = _vCheckTimeViewModel.RosterOverTime,
                                          HolidayCheck = _vCheckTimeViewModel.HolidayCheck,
                                          OverTimeCheck = _vCheckTimeViewModel.OverTimeCheck,
                                          LateCheck = _vCheckTimeViewModel.LateCheck,
                                          InnTime = _vCheckTimeViewModel.InnTime,
                                          OutTime = _vCheckTimeViewModel.OutTime,
                                          RosterWorkingHours = _vCheckTimeViewModel.RosterWorkingHours,
                                          WorkingHours = _vCheckTimeViewModel.WorkingHours,
                                          AbsentAdjustmentCategory = _vCheckTimeViewModel.AbsentAdjustmentCategory,
                                          LateComing = _vCheckTimeViewModel.LateComing,
                                          OverTime = _vCheckTimeViewModel.OverTime,
                                          OverTimeNight = _vCheckTimeViewModel.OverTimeNight,
                                          EarlyGoing = _vCheckTimeViewModel.EarlyGoing,
                                          EarlyOverTime = _vCheckTimeViewModel.EarlyOverTime,
                                          TotalOverTime = _vCheckTimeViewModel.TotalOverTime,
                                          AbsentCheck = _vCheckTimeViewModel.AbsentCheck,
                                          Remarks = _vCheckTimeViewModel.Remarks
                                      }).Where(x => x.TotalOverTime != 0).ToList();

                _TimeSheetViewModelBaseModel.TimeSheetViewModels = _TimeSheetView;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _TimeSheetViewModelBaseModel;
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

        public async Task<ApiResponse> GetAbsentAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;


                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

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
                // var _TimeSheetViewModelBaseModel = new TimeSheetViewModelBaseModel ();
                // _TimeSheetViewModelBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                // _TimeSheetViewModelBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                // _TimeSheetViewModelBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                // _TimeSheetViewModelBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                // _TimeSheetViewModelBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;

                var _AbsentEmployeeView = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(x => x.AbsentCheck == true)
                                           join _Employee in _context.Employees on _vCheckTimeViewModel.EmployeeId equals _Employee.Id
                                           join _EmployeesCatory in _context.EmployeeCategories on _Employee.EmployeeCategoryId equals _EmployeesCatory.Id
                                           select new AbsentReportList
                                           {
                                               EmployeeId = _Employee.Id,
                                               EmployeeNo = _Employee.MachineId,
                                               EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                               EmployeeCategory = _EmployeesCatory.Name,
                                               Date = _vCheckTimeViewModel.DailyDate,
                                               Remarks = _vCheckTimeViewModel.Remarks, // ApprovedAdjustType == "A" ? Enums.Payroll.AnnualLeave.ToString () : Enums.Payroll.Absent.ToString (),


                                           }).ToList();

                var _AbsentReportBaseModel = new AbsentReportBaseModel();
                _AbsentReportBaseModel.CompanyName = _userInformation.CompanyName;
                _AbsentReportBaseModel.BranchName = _userInformation.BranchName;
                _AbsentReportBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _AbsentReportBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                _AbsentReportBaseModel.AbsentReportList = _AbsentEmployeeView;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _AbsentReportBaseModel;
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

        //Inout Editor
        public async Task<ApiResponse> GetInOutEditorAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                InOutEditorSummaryBaseModel _InOutEditorSummaryBaseModel = new InOutEditorSummaryBaseModel();

                _InOutEditorSummaryBaseModel.CompanyName = _userInformation.CompanyName;
                _InOutEditorSummaryBaseModel.BranchName = _userInformation.BranchName;
                _InOutEditorSummaryBaseModel.UserName = _userInformation.UserName;
                _InOutEditorSummaryBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _InOutEditorSummaryBaseModel.DateTo = _PayrollReportCreteria.DateTo;

                var _TableInOutEditor = from _TableCheckInout in _context.CheckInOuts
                                        join _InOutCategory in _context.InOutCategories on _TableCheckInout.InOutCategoryId equals _InOutCategory.Id
                                        join _Employee in _context.Employees on _TableCheckInout.MachineId equals _Employee.MachineId
                                        join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                        join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id

                                        where _TableCheckInout.Type == Enums.Operations.U.ToString() && _TableCheckInout.Date >= _PayrollReportCreteria.DateFrom && _TableCheckInout.Date <= _PayrollReportCreteria.DateTo &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
                                            (string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))

                                        select new InOutEditorSummary
                                        {
                                            EmployeeId = _Employee.Id,
                                            EmployeeNo = _Employee.MachineId,
                                            EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                            DepartmentName = _Department.Name,
                                            DesignationName = _Designation.Name,
                                            Date = _TableCheckInout.Date,
                                            InoutCategoryId = _TableCheckInout.InOutCategoryId,
                                            InoutCategoryName = _InOutCategory.Name,
                                            CheckTime = _TableCheckInout.CheckTime,
                                            CheckType = _TableCheckInout.CheckType,
                                            Approved = _TableCheckInout.Approved,
                                        };

                _InOutEditorSummaryBaseModel.InOutEditorSummaryLists = _TableInOutEditor.ToList();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _InOutEditorSummaryBaseModel;
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

        // Advanced
        public async Task<ApiResponse> GetAdvanceAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                AdvanceReportBaseModel _AdvanceBaseModel = new AdvanceReportBaseModel();

                _AdvanceBaseModel.CompanyName = _userInformation.CompanyName;
                _AdvanceBaseModel.BranchName = _userInformation.BranchName;
                _AdvanceBaseModel.UserName = _userInformation.UserName;
                _AdvanceBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _AdvanceBaseModel.DateTo = _PayrollReportCreteria.DateTo;

                var _TableAdvance = from _Employee in _context.Employees
                                    join _Advance in _context.Advances on _Employee.Id equals _Advance.EmployeeId
                                    join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                    join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id

                                    where _Advance.Date >= _PayrollReportCreteria.DateFrom && _Advance.Date <= _PayrollReportCreteria.DateTo &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.Id) ? (true) : (_Advance.Id == new Guid(_PayrollReportCreteria.Id))) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))

                                    select new AdvanceReport
                                    {

                                        EmployeeId = _Employee.Id,
                                        EmployeeNo = _Employee.MachineId,
                                        EmployeeTemporaryPermanent = _Employee.TemporaryPermanent,
                                        EmployeeName = _Employee.Name + ' ' + _Employee.FatherName,
                                        DepartmentName = _Department.Name,
                                        DesignationName = _Designation.Name,
                                        Date = _Advance.Date,
                                        Amount = _Advance.Amount,
                                    };

                _AdvanceBaseModel.AdvancesList = _TableAdvance.ToList();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _AdvanceBaseModel;
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
        public async Task<ApiResponse> GetLoanAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                LoanSummaryBaseModel _LoanSummaryBaseModel = new LoanSummaryBaseModel();
                _LoanSummaryBaseModel.LoanSummaryLists = new List<LoanSummaryList>();

                _LoanSummaryBaseModel.CompanyName = _userInformation.CompanyName;
                _LoanSummaryBaseModel.BranchName = _userInformation.BranchName;
                _LoanSummaryBaseModel.UserName = _userInformation.UserName;
                _LoanSummaryBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _LoanSummaryBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                _LoanSummaryBaseModel.DateAsOn = _PayrollReportCreteria.DateAsOn;

                // bool _active = false;
                // if (_PayrollReportCreteria.Active == "1") { _active = true; }
                // else if (_PayrollReportCreteria.Active == "2") { _active = false; }



                var _TableLoan = await (from _Employee in _context.Employees
                                        join _LoanIssues in _context.LoanIssues on _Employee.Id equals _LoanIssues.EmployeeId
                                        join _LoanCategories in _context.LoanCategories on _LoanIssues.LoanCategoryId equals _LoanCategories.Id
                                        join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                        join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                        where _LoanIssues.Date <= _PayrollReportCreteria.DateAsOn &&
                                        (string.IsNullOrEmpty(_PayrollReportCreteria.Id) ? (true) : (_LoanIssues.Id == new Guid(_PayrollReportCreteria.Id))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                        //(string.IsNullOrEmpty(_PayrollReportCreteria.Active) ? (true) :(_Employee.Active == Convert.ToBoolean(_PayrollReportCreteria.Active)))
                                        select new LoanSummaryList
                                        {
                                            Id = _LoanIssues.Id,
                                            EmployeeId = _Employee.Id,
                                            EmployeeNo = _Employee.MachineId,
                                            EmployeeTemporaryPermanent = _Employee.TemporaryPermanent,
                                            EmployeeName = _Employee.Name + ' ' + _Employee.FatherName,
                                            LoanCatgoryId = _LoanIssues.LoanCategoryId,
                                            LoanCatgoryName = _LoanCategories.Name,
                                            DepartmentName = _Department.Name,
                                            DesignationName = _Designation.Name,
                                            Date = _LoanIssues.Date,
                                            NoofInstalment = _LoanIssues.NoOfInstalment,
                                            InstalmentAmount = _LoanIssues.InstalmentAmount,
                                            Amount = _LoanIssues.Amount,
                                            Receiving = 0,
                                            Balance = _LoanIssues.Amount,
                                            Status = Enums.Status.InProcess.ToString(),
                                        }).ToListAsync();

                if (_TableLoan == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                if (_TableLoan.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_TableLoan.Count() > 0)
                {
                    foreach (var _TableLoanRecord in _TableLoan)
                    {
                        var _TableLoanReceived = await _context.LoanReceives.Where(a => a.LoanIssueId == _TableLoanRecord.Id).SumAsync(s => s.Amount);
                        _TableLoanRecord.Receiving = _TableLoanReceived;
                        _TableLoanRecord.Balance = _TableLoanRecord.Amount - _TableLoanReceived;
                        _TableLoanRecord.Status = (_TableLoanRecord.Amount - _TableLoanReceived) == 0 ? Enums.Status.Complete.ToString() : Enums.Status.InProcess.ToString();
                    }
                }
                var _TableLoanFilter = _TableLoan;
                if (_PayrollReportCreteria.LoanStatus == "1")
                {
                    _TableLoanFilter = _TableLoan.Where(x => x.Balance > 0).ToList();
                }
                else if (_PayrollReportCreteria.LoanStatus == "2")
                {
                    _TableLoanFilter = _TableLoan.Where(x => x.Balance == 0).ToList();
                }
                _LoanSummaryBaseModel.LoanSummaryLists.AddRange(_TableLoanFilter);

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _LoanSummaryBaseModel;
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

        //Loan Receive
        public async Task<ApiResponse> GetLoanReceiveAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                LoanReceiveSummaryBaseModel _LoanReceiveSummaryBaseModel = new LoanReceiveSummaryBaseModel();

                _LoanReceiveSummaryBaseModel.CompanyName = _userInformation.CompanyName;
                _LoanReceiveSummaryBaseModel.BranchName = _userInformation.BranchName;
                _LoanReceiveSummaryBaseModel.UserName = _userInformation.UserName;
                _LoanReceiveSummaryBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                _LoanReceiveSummaryBaseModel.DateTo = _PayrollReportCreteria.DateTo;

                // var _TableLoan = await _context.LoanIssues.Include (e => e.Employee).Include (d => d.Employee.designation).Include (dep => dep.Employee.department).Include (c => c.Employee.employeeCategory).Include (lc => lc.LoanCategory).Where (x => x.Date >= _PayrollReportCreteria.DateFrom && x.Date <= _PayrollReportCreteria.DateTo).ToListAsync ();
                var _TableLoanReceived = await (from _Employee in _context.Employees
                                                join _LoanIssues in _context.LoanIssues on _Employee.Id equals _LoanIssues.EmployeeId
                                                join _LoanCategories in _context.LoanCategories on _LoanIssues.LoanCategoryId equals _LoanCategories.Id
                                                join _LoanReceives in _context.LoanReceives on _LoanIssues.Id equals _LoanReceives.LoanIssueId
                                                join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                                join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                                where _LoanReceives.Date >= _PayrollReportCreteria.DateFrom && _LoanReceives.Date <= _PayrollReportCreteria.DateTo &&

(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))

                                                select new LoanReceiveSummaryList
                                                {
                                                    EmployeeId = _Employee.Id,
                                                    EmployeeNo = _Employee.MachineId,
                                                    EmployeeTemporaryPermanent = _Employee.TemporaryPermanent,
                                                    EmployeeName = _Employee.Name + ' ' + _Employee.FatherName,
                                                    DepartmentName = _Department.Name,
                                                    DesignationName = _Designation.Name,
                                                    LoanCatgoryId = _LoanIssues.LoanCategoryId,
                                                    LoanCatgoryName = _LoanCategories.Name,
                                                    Date = _LoanReceives.Date,
                                                    IssueDate = _LoanIssues.Date,
                                                    CashBank = _LoanReceives.CheaqueCash,
                                                    Amount = _LoanIssues.Amount,
                                                    Receiving = _LoanReceives.Amount,
                                                }).ToListAsync();
                if (_TableLoanReceived == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                if (_TableLoanReceived.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                _LoanReceiveSummaryBaseModel.LoanReceiveSummaryLists = new List<LoanReceiveSummaryList>();

                _LoanReceiveSummaryBaseModel.LoanReceiveSummaryLists.AddRange(_TableLoanReceived);

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _LoanReceiveSummaryBaseModel;
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

        // Employee Profile
        public async Task<ApiResponse> GetEmployeeProfileAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                EmployeeProfileSummaryBaseModel _EmployeeProfileSummaryBaseModel = new EmployeeProfileSummaryBaseModel();
                _EmployeeProfileSummaryBaseModel.EmployeeProfileSummaryLists = new List<EmployeeProfileSummaryList>();

                DateTime _Date = DateTime.Now;

                _EmployeeProfileSummaryBaseModel.CompanyName = _userInformation.CompanyName;
                _EmployeeProfileSummaryBaseModel.BranchName = _userInformation.BranchName;
                _EmployeeProfileSummaryBaseModel.UserName = _userInformation.UserName;
                _EmployeeProfileSummaryBaseModel.DailyDate = _PayrollReportCreteria.DateFrom;
                var _Active = _PayrollReportCreteria.Active == "1" ? true : false;
                if (_PayrollReportCreteria.Active == "2") { _PayrollReportCreteria.Active = ""; }

                var _TableEmployee = await (from _Employee in _context.Employees
                                            join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                            join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                            join _Roster in _context.Rosters on _Employee.RosterId equals _Roster.Id
                                            where _Employee.Action != Enums.Operations.D.ToString() &&

(string.IsNullOrEmpty(_PayrollReportCreteria.Id) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.Id))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Active) ? (true) : (_Employee.Active == _Active)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))

                                            select new EmployeeProfileSummaryList
                                            {
                                                // Personal
                                                EmployeeId = _Employee.Id,
                                                EmployeeNo = _Employee.MachineId,
                                                EmployeeName = _Employee.Name,
                                                FatherName = _Employee.FatherName,
                                                CNIC = _Employee.CNIC,
                                                CNICExpire = _Employee.CNICExpire,
                                                CNICExpireDays = _Employee.CNICExpire.Subtract(_Date).TotalDays,
                                                NTN = _Employee.NTN,
                                                AddressPermanent = _Employee.AddressPermanent,
                                                PresentAddress = _Employee.Address,
                                                Phone = _Employee.Phone,
                                                Mobile = _Employee.Mobile,
                                                DateofBirth = _Employee.DateofBirth,
                                                Gender = _Employee.Gender,
                                                Married = _Employee.Married,
                                                //Employee Detail
                                                DesignationName = _Employee.designation.Name,
                                                DepartmentName = _Employee.department.Name,
                                                DateofJoin = _Employee.DateofJoin,
                                                RosterName = _Roster.Name,
                                                Remarks = _Employee.Remarks,

                                                // GrossSalary = _Salary.CurrentAmount,
                                                // BasicSalary = Convert.ToDouble (_Salary.CurrentAmount * 0.6),
                                                // Allowance = Convert.ToDouble (_Salary.CurrentAmount * 0.4),
                                                ReferenceOneName = _Employee.ReferenceOne,
                                                ReferenceOneCNIC = _Employee.ReferenceCNICOne,
                                                ReferenceOneContact = _Employee.ReferenceContactOne,
                                                ReferenceOneAddress = _Employee.ReferenceAddressOne,
                                                ReferenceTwoName = _Employee.ReferenceTwo,
                                                ReferenceTwoCNIC = _Employee.ReferenceCNICTwo,
                                                ReferenceTwoContact = _Employee.ReferenceContactTwo,
                                                ReferenceTwoAddress = _Employee.ReferenceAddressTwo,
                                                AnnualLeaveId = _Employee.AnnualLeavesId,
                                                // AnnualLeave = _AnnualLeave,
                                                // SickLeave = _SickLeave,
                                                // CasualLeave = _CasulaLeave,
                                            }).ToListAsync();

                if (_TableEmployee == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                if (_TableEmployee.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                foreach (var _TableEmployeeRecord in _TableEmployee)
                {
                    var _Salary = await _context.Salaries.Where(a => a.EmployeeId == _TableEmployeeRecord.EmployeeId).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
                    var _AnnualLeave = await _context.AnnualLeaves.Where(x => x.Id == _TableEmployeeRecord.AnnualLeaveId).FirstOrDefaultAsync();
                    var _ImageCount = await _context.EmployeeImages.Where(x => x.EmployeeId == _TableEmployeeRecord.EmployeeId).CountAsync();
                    _TableEmployeeRecord.AttachDocuments = _ImageCount;
                    if (_Salary != null)
                    {
                        _TableEmployeeRecord.GrossSalary = _Salary.CurrentAmount;
                        _TableEmployeeRecord.BasicSalary = Convert.ToDouble(_Salary.CurrentAmount * 0.6);
                        _TableEmployeeRecord.Allowance = Convert.ToDouble(_Salary.CurrentAmount * 0.4);
                    }
                    if (_AnnualLeave != null)
                    {
                        _TableEmployeeRecord.AnnualLeave = _AnnualLeave.AnnualLeaveDays;
                        _TableEmployeeRecord.SickLeave = _AnnualLeave.SickLeaveDays;
                        _TableEmployeeRecord.CasualLeave = _AnnualLeave.CasualLeaveDays;
                    }

                }
                //                List<EmployeeProfileSummaryList> _EmployeeProfileSummaryList = new List<EmployeeProfileSummaryList> ();

                _EmployeeProfileSummaryBaseModel.EmployeeProfileSummaryLists.AddRange(_TableEmployee);

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _EmployeeProfileSummaryBaseModel;
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

        // Employee Profile
        public async Task<ApiResponse> GetEmployeeProfileDocumentAsync(string _TokenString, Guid _EmployeeId)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                var _TableEmployee = await _context.EmployeeImages.Include(p => p.employee).Where(x => x.EmployeeId == _EmployeeId).ToListAsync();
                if (_TableEmployee == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                if (_TableEmployee.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                EmployeeProfileDocumentReportBaseModel _EmployeeProfileDocumentReportBaseModel = new EmployeeProfileDocumentReportBaseModel();
                _EmployeeProfileDocumentReportBaseModel.EmployeeProfileDocumentReportLists = new List<EmployeeProfileDocumentReportList>();

                DateTime _Date = DateTime.Now;

                _EmployeeProfileDocumentReportBaseModel.CompanyName = _userInformation.CompanyName;
                _EmployeeProfileDocumentReportBaseModel.BranchName = _userInformation.BranchName;
                _EmployeeProfileDocumentReportBaseModel.UserName = _userInformation.UserName;
                _EmployeeProfileDocumentReportBaseModel.EmployeeId = _TableEmployee.FirstOrDefault().EmployeeId;
                _EmployeeProfileDocumentReportBaseModel.EmployeeName = _TableEmployee.FirstOrDefault().employee.Name + " " + _TableEmployee.FirstOrDefault().employee.FatherName;
                _EmployeeProfileDocumentReportBaseModel.EmployeeNo = _TableEmployee.FirstOrDefault().employee.MachineId;
                foreach (var _TableEmployeeRecord in _TableEmployee)
                {
                    _EmployeeProfileDocumentReportBaseModel.EmployeeProfileDocumentReportLists.Add(new EmployeeProfileDocumentReportList { ImageBytes = _TableEmployeeRecord.ImageBytes, ImageName = _TableEmployeeRecord.ImageName });

                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _EmployeeProfileDocumentReportBaseModel;
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

        //Leave Report
        public async Task<ApiResponse> GetLeaveAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;
                _PayrollReportCreteria.DateFrom = new DateTime(_PayrollReportCreteria.DateFrom.Year, 1, 1);

                var _LeaveSummaryBaseModel = new LeaveSummaryBaseModel();
                _LeaveSummaryBaseModel.CompanyName = _userInformation.CompanyName;
                _LeaveSummaryBaseModel.BranchName = _userInformation.BranchName;
                _LeaveSummaryBaseModel.DailyDate = _PayrollReportCreteria.DateTo;

                var _AnnualLeaveTable = await (from _Employee in _context.Employees
                                               join _AnnualLeave in _context.AnnualLeaves on _Employee.AnnualLeavesId equals _AnnualLeave.Id
                                               where _Employee.Active == true && _Employee.Action != Enums.Operations.D.ToString() &&
(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                               select new LeaveSummaryList
                                               {
                                                   EmployeeId = _Employee.Id,
                                                   EmployeeNo = _Employee.MachineId,
                                                   EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                                   AnnualLeave = _AnnualLeave.AnnualLeaveDays,
                                                   balAnnualLeave = _AnnualLeave.AnnualLeaveDays,

                                                   SickLeave = _AnnualLeave.SickLeaveDays,
                                                   balSickLeave = _AnnualLeave.SickLeaveDays, // - Convert.ToInt32 (_AnnualLeave.SickLeaveAllow),

                                                   CasualLeave = _AnnualLeave.CasualLeaveDays,
                                                   balCasualLeave = _AnnualLeave.CasualLeaveDays, // - Convert.ToInt32 (_AnnualLeave.CasualLeaveAllow),
                                               }).ToListAsync();

                foreach (var _AnnualLeaveTableRecord in _AnnualLeaveTable)
                {
                    //                    var _AbsentApproval = await _context.Absents.Where (s => s.Date >= _PayrollReportCreteria.DateFrom && s.Date <= _PayrollReportCreteria.DateTo && s.EmployeeId == _AnnualLeaveTableRecord.EmployeeId && s.Approved == true && s.ApprovedAdjust == true).ToListAsync ();
                    var _AbsentApproval = await _context.AnnualLeaveAdjustments.Where(s => s.Date >= _PayrollReportCreteria.DateFrom && s.Date <= _PayrollReportCreteria.DateTo && s.EmployeeId == _AnnualLeaveTableRecord.EmployeeId).ToListAsync();
                    var _AnnualLeaveAvail = 0;
                    var _SickLeaveAvail = 0;
                    var _CasualLeaveAvail = 0;
                    if (_AbsentApproval != null)
                    {
                        _AnnualLeaveAvail = _AbsentApproval.Where(s => s.ApprovedAdjustType == Enums.Payroll.A.ToString()).Sum(s => s.LeaveAdjust);
                        _SickLeaveAvail = _AbsentApproval.Where(s => s.ApprovedAdjustType == Enums.Payroll.S.ToString()).Sum(s => s.LeaveAdjust);
                        _CasualLeaveAvail = _AbsentApproval.Where(s => s.ApprovedAdjustType == Enums.Payroll.C.ToString()).Sum(s => s.LeaveAdjust);
                    }
                    _AnnualLeaveTableRecord.AnnualLeaveAvail = _AnnualLeaveAvail;
                    _AnnualLeaveTableRecord.balAnnualLeave = _AnnualLeaveTableRecord.AnnualLeave - _AnnualLeaveAvail;

                    _AnnualLeaveTableRecord.SickLeaveAvail = _SickLeaveAvail;
                    _AnnualLeaveTableRecord.balSickLeave = _AnnualLeaveTableRecord.SickLeave - _SickLeaveAvail;

                    _AnnualLeaveTableRecord.CasualLeaveAvail = _CasualLeaveAvail;
                    _AnnualLeaveTableRecord.balCasualLeave = _AnnualLeaveTableRecord.CasualLeave - _CasualLeaveAvail;
                }
                _LeaveSummaryBaseModel.LeaveSummaryList = _AnnualLeaveTable;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _LeaveSummaryBaseModel;
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

        //Salary Increament Report
        public async Task<ApiResponse> GetSalaryIncrementAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                var _SalaryIncrementReportBaseModel = new SalaryIncrementReportBaseModel();
                _SalaryIncrementReportBaseModel.CompanyName = _userInformation.CompanyName;
                _SalaryIncrementReportBaseModel.BranchName = _userInformation.BranchName;
                _SalaryIncrementReportBaseModel.DailyDate = _PayrollReportCreteria.DateFrom;
                //_AbsentReportBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;

                var _SalaryTable = await (from _Employee in _context.Employees
                                          join _Salary in _context.Salaries on _Employee.Id equals _Salary.EmployeeId
                                          join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                          join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                          where _Employee.Active == true && _Employee.Action != Enums.Operations.D.ToString() && _Salary.Action != Enums.Operations.D.ToString() && _Salary.Date >= _PayrollReportCreteria.DateFrom && _Salary.Date <= _PayrollReportCreteria.DateTo &&
(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                          select new SalaryIncrementReportList
                                          {
                                              Date = _Salary.Date,
                                              EmployeeId = _Employee.Id,
                                              EmployeeNo = _Employee.MachineId,
                                              EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                              DepartmentName = _Department.Name,
                                              DesignationName = _Designation.Name,
                                              PreviousSalary = _Salary.PreviousAmount,
                                              IncrementPercentage = _Salary.PreviousAmount == _Salary.CurrentAmount ? 0 : _Salary.IncreamentPercentage,
                                              IncrementAmount = _Salary.PreviousAmount == _Salary.CurrentAmount ? 0 : _Salary.IncreamentAmount,
                                              CurrentSalary = _Salary.CurrentAmount

                                          }).ToListAsync();

                _SalaryIncrementReportBaseModel.SalaryIncrementReportList = _SalaryTable;

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _SalaryIncrementReportBaseModel;
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
        public async Task<ApiResponse> GetSalaryRegisterWorkerAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                DateTime dateFrom = new DateTime(_PayrollReportCreteria.DateFrom.Year, _PayrollReportCreteria.DateFrom.Month, 1);
                DateTime dateTo = new DateTime(_PayrollReportCreteria.DateTo.Year, _PayrollReportCreteria.DateTo.Month, DateTime.DaysInMonth(_PayrollReportCreteria.DateTo.Year, _PayrollReportCreteria.DateTo.Month));

                SalaryProcessViewModelBaseModel _SalaryProcessViewModelBaseModel = new SalaryProcessViewModelBaseModel();
                _SalaryProcessViewModelBaseModel.SalaryProcessViewModels = new List<SalaryProcessViewModel>();

                _SalaryProcessViewModelBaseModel.CompanyName = _userInformation.CompanyName;
                _SalaryProcessViewModelBaseModel.BranchName = _userInformation.BranchName;
                _SalaryProcessViewModelBaseModel.UserName = _userInformation.UserName;
                _SalaryProcessViewModelBaseModel.DateFrom = dateFrom;
                _SalaryProcessViewModelBaseModel.DateTo = dateTo;

                var _WorkerSalary = await (from _WorkerSalaries in _context.WorkerSalaries
                                           join _Employee in _context.Employees on _WorkerSalaries.EmployeeId equals _Employee.Id
                                           join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                           join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id

                                           where _WorkerSalaries.Date >= dateFrom && _WorkerSalaries.Date <= dateTo &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
                                           (string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                           select new SalaryProcessViewModel
                                           {
                                               EmployeeId = _Employee.Id,
                                               EmployeeNo = _Employee.MachineId,
                                               EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                               DepartmentId = _Department.Id,
                                               DepartmentName = _Department.Name,
                                               DesignationId = _Designation.Id,
                                               DesignationName = _Designation.Name,
                                               DateOfJoining = _Employee.DateofJoin,
                                               SalaryAmount = _WorkerSalaries.SalaryAmount,
                                               SalaryAllowanceAmount = _WorkerSalaries.SalaryAllowanceAmount,
                                               SalaryGrossAmount = _WorkerSalaries.SalaryGrossAmount,
                                               NoOfDaysMonth = _WorkerSalaries.NoOfDaysMonth,
                                               LateHours = _WorkerSalaries.LateHours,
                                               LateDays = _WorkerSalaries.LateDays,
                                               LateDaysTotal = _WorkerSalaries.LateDaysTotal,
                                               PresentDays = _WorkerSalaries.PresentDays,
                                               AbsentDays = _WorkerSalaries.AbsentDays,
                                               AttendanceAllowanceAmount = _WorkerSalaries.AttendanceAllowanceAmount,
                                               WorkingDays = _WorkerSalaries.WorkingDays,
                                               AdditionAmount = _WorkerSalaries.AdditionAmount,
                                               DeductionAmount = _WorkerSalaries.DeductionAmount,
                                               LateDaysActual = _WorkerSalaries.LateDaysActual,
                                               LateDaysActualAmount = _WorkerSalaries.LateDaysActualAmount,
                                               Takaful = _WorkerSalaries.Takaful,
                                               AdvanceAmount = _WorkerSalaries.AdvanceAmount,
                                               LoanAmount = _WorkerSalaries.LoanAmount,
                                               IncomeTaxAmount = _WorkerSalaries.IncomeTaxAmount,
                                               GrossAmount = _WorkerSalaries.GrossAmount,
                                               OvertimeActual = _WorkerSalaries.OvertimeActual,
                                               OvertimeActualAmount = _WorkerSalaries.OvertimeActualAmount,
                                               PayableAmount = _WorkerSalaries.PayableAmount,
                                               VoucherPostCk = _WorkerSalaries.VoucherPostCk,
                                           }).OrderBy(o => o.EmployeeName).ToListAsync();
                _SalaryProcessViewModelBaseModel.SalaryProcessViewModels.AddRange(_WorkerSalary);

                if (_SalaryProcessViewModelBaseModel.SalaryProcessViewModels == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_SalaryProcessViewModelBaseModel.SalaryProcessViewModels.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _SalaryProcessViewModelBaseModel;
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
        public async Task<ApiResponse> GetSalaryRegisterStaffAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                DateTime dateFrom = new DateTime(_PayrollReportCreteria.DateFrom.Year, _PayrollReportCreteria.DateFrom.Month, 1);
                DateTime dateTo = new DateTime(_PayrollReportCreteria.DateTo.Year, _PayrollReportCreteria.DateTo.Month, DateTime.DaysInMonth(_PayrollReportCreteria.DateTo.Year, _PayrollReportCreteria.DateTo.Month));

                SalaryProcessViewModelBaseModel _SalaryProcessViewModelBaseModel = new SalaryProcessViewModelBaseModel();
                _SalaryProcessViewModelBaseModel.SalaryProcessViewModels = new List<SalaryProcessViewModel>();

                _SalaryProcessViewModelBaseModel.CompanyName = _userInformation.CompanyName;
                _SalaryProcessViewModelBaseModel.BranchName = _userInformation.BranchName;
                _SalaryProcessViewModelBaseModel.UserName = _userInformation.UserName;
                _SalaryProcessViewModelBaseModel.DateFrom = dateFrom;
                _SalaryProcessViewModelBaseModel.DateTo = dateTo;

                var _StaffSalary = await (from _StaffSalaries in _context.StaffSalaries
                                          join _Employee in _context.Employees on _StaffSalaries.EmployeeId equals _Employee.Id
                                          join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                          join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id

                                          where _StaffSalaries.Date >= dateFrom && _StaffSalaries.Date <= dateTo &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
                                          (string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                          select new SalaryProcessViewModel
                                          {
                                              EmployeeId = _Employee.Id,
                                              EmployeeNo = _Employee.MachineId,
                                              EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                              DepartmentId = _Department.Id,
                                              DepartmentName = _Department.Name,
                                              DesignationId = _Designation.Id,
                                              DesignationName = _Designation.Name,
                                              DateOfJoining = _Employee.DateofJoin,
                                              SalaryAmount = _StaffSalaries.SalaryAmount,
                                              SalaryAllowanceAmount = _StaffSalaries.SalaryAllowanceAmount,
                                              SalaryGrossAmount = _StaffSalaries.SalaryGrossAmount,
                                              NoOfDaysMonth = _StaffSalaries.NoOfDaysMonth,
                                              LateHours = _StaffSalaries.LateHours,
                                              LateDays = _StaffSalaries.LateDays,
                                              LateDaysTotal = _StaffSalaries.LateDaysTotal,
                                              PresentDays = _StaffSalaries.PresentDays,
                                              AbsentDays = _StaffSalaries.AbsentDays,
                                              AttendanceAllowanceAmount = _StaffSalaries.AttendanceAllowanceAmount,
                                              WorkingDays = _StaffSalaries.WorkingDays,
                                              AdditionAmount = _StaffSalaries.AdditionAmount,
                                              DeductionAmount = _StaffSalaries.DeductionAmount,
                                              LateDaysActual = _StaffSalaries.LateDaysActual,
                                              LateDaysActualAmount = _StaffSalaries.LateDaysActualAmount,
                                              Takaful = _StaffSalaries.Takaful,
                                              AdvanceAmount = _StaffSalaries.AdvanceAmount,
                                              LoanAmount = _StaffSalaries.LoanAmount,
                                              IncomeTaxAmount = _StaffSalaries.IncomeTaxAmount,
                                              GrossAmount = _StaffSalaries.GrossAmount,
                                              OvertimeActual = _StaffSalaries.OvertimeActual,
                                              OvertimeActualAmount = _StaffSalaries.OvertimeActualAmount,
                                              PayableAmount = _StaffSalaries.PayableAmount,
                                              VoucherPostCk = _StaffSalaries.VoucherPostCk,
                                          }).OrderBy(o => o.EmployeeName).ToListAsync();
                _SalaryProcessViewModelBaseModel.SalaryProcessViewModels.AddRange(_StaffSalary);

                if (_SalaryProcessViewModelBaseModel.SalaryProcessViewModels == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_SalaryProcessViewModelBaseModel.SalaryProcessViewModels.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _SalaryProcessViewModelBaseModel;
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

        public async Task<ApiResponse> GetSalaryPaySlipAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                if (string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker))
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Invalid Creteria";
                    return ApiResponse;
                }
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);

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


                //Worker
                if (_PayrollReportCreteria.OfficeWorker == Enums.Payroll.Worker.ToString())
                {

                    var _salaryTable = await _context.WorkerSalaries.Include(e => e.employee).Where(s => s.Date == _PayrollReportCreteria.DateTo).ToListAsync();


                    if (_salaryTable == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                        ApiResponse.message = "Record Not Found";
                        return ApiResponse;
                    }

                    if (_salaryTable.Count == 0)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record Not Found";
                        return ApiResponse;
                    }
                    var _SalaryPaySlipViewModelBaseModel = new SalaryPaySlipViewModelBaseModel();
                    _SalaryPaySlipViewModelBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                    _SalaryPaySlipViewModelBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                    _SalaryPaySlipViewModelBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                    _SalaryPaySlipViewModelBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                    _SalaryPaySlipViewModelBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;
                    _SalaryPaySlipViewModelBaseModel.SalaryDetails = new List<SalaryDetail>();
                    _SalaryPaySlipViewModelBaseModel.SalaryPaySlipLists = new List<SalaryPaySlipList>();

                    foreach (var _salaryRecord in _salaryTable)
                    {
                        _SalaryPaySlipViewModelBaseModel.SalaryDetails.Add(new SalaryDetail
                        {
                            EmployeeId = _salaryRecord.EmployeeId,
                            EmployeeNo = _salaryRecord.employee.MachineId,
                            EmployeeName = _salaryRecord.employee.Name,
                            PresentDays = _salaryRecord.PresentDays,
                            AbsentDays = _salaryRecord.AbsentDays,
                            LateDays = _salaryRecord.LateDays,
                            LateDaysDeduction = _salaryRecord.LateDaysActual,
                            OThours = _salaryRecord.OvertimeActual,
                            PayableDays = _salaryRecord.PresentDays,
                            BasicSalary = _salaryRecord.SalaryAmount,
                            Allowance = _salaryRecord.SalaryAllowanceAmount,
                            GrossSalary = _salaryRecord.SalaryGrossAmount,
                            MonthDays = _salaryRecord.NoOfDaysMonth,
                            AttendanceAllowance = _salaryRecord.AttendanceAllowanceAmount,
                            OtherAddition = _salaryRecord.AdditionAmount,
                            LateDeduction = _salaryRecord.LateDaysActualAmount,
                            Advance = _salaryRecord.AdvanceAmount,
                            Loan = _salaryRecord.LoanAmount,
                            Takaful = _salaryRecord.Takaful,
                            IncomeTax = _salaryRecord.IncomeTaxAmount,
                            OtherDeduction = _salaryRecord.DeductionAmount,
                            TotalGrossSalary = _salaryRecord.GrossAmount,
                            OTAmount = _salaryRecord.OvertimeActualAmount,
                            NetSalary = _salaryRecord.GrossAmount
                        });


                        var _SalaryPaySlipTable = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(a => a.EmployeeId == _salaryRecord.EmployeeId)
                                                   select new SalaryPaySlipList
                                                   {
                                                       EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                                       EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                                       EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                                       DateOfJoin = _vCheckTimeViewModel.DateOfJoin,
                                                       DesignationName = _vCheckTimeViewModel.DesignationName,
                                                       DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                                       DailyDate = _vCheckTimeViewModel.DailyDate,
                                                       RosterInn = _vCheckTimeViewModel.RosterInn,
                                                       RosterOut = _vCheckTimeViewModel.RosterOut,
                                                       RosterOverTime = _vCheckTimeViewModel.RosterOverTime,
                                                       OverTimeCheck = _vCheckTimeViewModel.OverTimeCheck,
                                                       LateCheck = _vCheckTimeViewModel.LateCheck,
                                                       InnTime = _vCheckTimeViewModel.InnTime,
                                                       OutTime = _vCheckTimeViewModel.OutTime,
                                                       RosterWorkingHours = _vCheckTimeViewModel.RosterWorkingHours,
                                                       WorkingHours = _vCheckTimeViewModel.WorkingHours,
                                                       LateComing = _vCheckTimeViewModel.LateComing,
                                                       OverTime = _vCheckTimeViewModel.OverTime,
                                                       EarlyGoing = _vCheckTimeViewModel.EarlyGoing,
                                                       EarlyOverTime = _vCheckTimeViewModel.EarlyOverTime,
                                                       TotalOverTime = _vCheckTimeViewModel.TotalOverTime,
                                                       Remarks = _vCheckTimeViewModel.Remarks
                                                   }).ToList();




                        _SalaryPaySlipViewModelBaseModel.SalaryPaySlipLists.AddRange(_SalaryPaySlipTable);

                    }
                    ApiResponse.data = _SalaryPaySlipViewModelBaseModel;

                }//Office
                else if (_PayrollReportCreteria.OfficeWorker == Enums.Payroll.Office.ToString())
                {

                    var _salaryTable = await _context.StaffSalaries.Include(e => e.employee).Where(s => s.Date == _PayrollReportCreteria.DateTo).ToListAsync();


                    if (_salaryTable == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                        ApiResponse.message = "Record Not Found";
                        return ApiResponse;
                    }

                    if (_salaryTable.Count == 0)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record Not Found";
                        return ApiResponse;
                    }
                    var _SalaryPaySlipViewModelBaseModel = new SalaryPaySlipViewModelBaseModel();
                    _SalaryPaySlipViewModelBaseModel.CompanyName = vCheckTimeViewModelBaseModel.CompanyName;
                    _SalaryPaySlipViewModelBaseModel.BranchName = vCheckTimeViewModelBaseModel.BranchName;
                    _SalaryPaySlipViewModelBaseModel.DateFrom = _PayrollReportCreteria.DateFrom;
                    _SalaryPaySlipViewModelBaseModel.DateTo = _PayrollReportCreteria.DateTo;
                    _SalaryPaySlipViewModelBaseModel.UserName = vCheckTimeViewModelBaseModel.UserName;
                    _SalaryPaySlipViewModelBaseModel.SalaryDetails = new List<SalaryDetail>();
                    _SalaryPaySlipViewModelBaseModel.SalaryPaySlipLists = new List<SalaryPaySlipList>();

                    foreach (var _salaryRecord in _salaryTable)
                    {
                        _SalaryPaySlipViewModelBaseModel.SalaryDetails.Add(new SalaryDetail
                        {
                            EmployeeId = _salaryRecord.EmployeeId,
                            EmployeeNo = _salaryRecord.employee.MachineId,
                            EmployeeName = _salaryRecord.employee.Name,
                            PresentDays = _salaryRecord.PresentDays,
                            AbsentDays = _salaryRecord.AbsentDays,
                            LateDays = _salaryRecord.LateDays,
                            LateDaysDeduction = _salaryRecord.LateDaysActual,
                            OThours = _salaryRecord.OvertimeActual,
                            PayableDays = _salaryRecord.PresentDays,
                            BasicSalary = _salaryRecord.SalaryAmount,
                            Allowance = _salaryRecord.SalaryAllowanceAmount,
                            GrossSalary = _salaryRecord.GrossAmount,
                            MonthDays = _salaryRecord.NoOfDaysMonth,
                            AttendanceAllowance = _salaryRecord.AttendanceAllowanceAmount,
                            OtherAddition = _salaryRecord.AdditionAmount,
                            LateDeduction = _salaryRecord.LateDaysActualAmount,
                            Advance = _salaryRecord.AdvanceAmount,
                            Loan = _salaryRecord.LoanAmount,
                            Takaful = _salaryRecord.Takaful,
                            IncomeTax = _salaryRecord.IncomeTaxAmount,
                            OtherDeduction = _salaryRecord.DeductionAmount,
                            TotalGrossSalary = _salaryRecord.GrossAmount,
                            OTAmount = _salaryRecord.OvertimeActualAmount,
                            NetSalary = _salaryRecord.GrossAmount
                        });


                        var _SalaryPaySlipTable = (from _vCheckTimeViewModel in vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(a => a.EmployeeId == _salaryRecord.EmployeeId)
                                                   select new SalaryPaySlipList
                                                   {
                                                       EmployeeId = _vCheckTimeViewModel.EmployeeId,
                                                       EmployeeNo = _vCheckTimeViewModel.EmployeeNo,
                                                       EmployeeName = _vCheckTimeViewModel.EmployeeName,
                                                       DateOfJoin = _vCheckTimeViewModel.DateOfJoin,
                                                       DesignationName = _vCheckTimeViewModel.DesignationName,
                                                       DepartmentName = _vCheckTimeViewModel.DepartmentName,
                                                       DailyDate = _vCheckTimeViewModel.DailyDate,
                                                       RosterInn = _vCheckTimeViewModel.RosterInn,
                                                       RosterOut = _vCheckTimeViewModel.RosterOut,
                                                       RosterOverTime = _vCheckTimeViewModel.RosterOverTime,
                                                       OverTimeCheck = _vCheckTimeViewModel.OverTimeCheck,
                                                       LateCheck = _vCheckTimeViewModel.LateCheck,
                                                       InnTime = _vCheckTimeViewModel.InnTime,
                                                       OutTime = _vCheckTimeViewModel.OutTime,
                                                       RosterWorkingHours = _vCheckTimeViewModel.RosterWorkingHours,
                                                       WorkingHours = _vCheckTimeViewModel.WorkingHours,
                                                       LateComing = _vCheckTimeViewModel.LateComing,
                                                       OverTime = _vCheckTimeViewModel.OverTime,
                                                       EarlyGoing = _vCheckTimeViewModel.EarlyGoing,
                                                       EarlyOverTime = _vCheckTimeViewModel.EarlyOverTime,
                                                       TotalOverTime = _vCheckTimeViewModel.TotalOverTime,
                                                       Remarks = _vCheckTimeViewModel.Remarks
                                                   }).ToList();




                        _SalaryPaySlipViewModelBaseModel.SalaryPaySlipLists.AddRange(_SalaryPaySlipTable);

                    }

                    ApiResponse.data = _SalaryPaySlipViewModelBaseModel;


                }
                if (string.IsNullOrEmpty(ApiResponse.data.ToString()))
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
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
        public async Task<ApiResponse> GetCurrentSalariesAsync(string _TokenString, PayrollReportCreteria _PayrollReportCreteria)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                CurrentSalariesReportBaseModel _CurrentSalariesReportBaseModel = new CurrentSalariesReportBaseModel();
                _CurrentSalariesReportBaseModel.CurrentSalariesReportLists = new List<CurrentSalariesReportList>();

                _CurrentSalariesReportBaseModel.CompanyName = _userInformation.CompanyName;
                _CurrentSalariesReportBaseModel.BranchName = _userInformation.BranchName;
                _CurrentSalariesReportBaseModel.DailyDate = _PayrollReportCreteria.DateFrom;

                var _EmployeeTable = await (from _Employee in _context.Employees
                                            join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                            join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                            where _Employee.Active == true && _Employee.Action != Enums.Operations.D.ToString() &&
(string.IsNullOrEmpty(_PayrollReportCreteria.BranchId) ? (true) : (_Employee.BranchId == new Guid(_PayrollReportCreteria.BranchId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.EmployeeId) ? (true) : (_Employee.Id == new Guid(_PayrollReportCreteria.EmployeeId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.OfficeWorker) ? (true) : (_Employee.OfficeWorker == _PayrollReportCreteria.OfficeWorker)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.TemporaryPermanent) ? (true) : (_Employee.TemporaryPermanent == _PayrollReportCreteria.TemporaryPermanent)) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DesignationId) ? (true) : (_Employee.DesignationId == new Guid(_PayrollReportCreteria.DesignationId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.DepartmentId) ? (true) : (_Employee.DepartmentId == new Guid(_PayrollReportCreteria.DepartmentId))) &&
(string.IsNullOrEmpty(_PayrollReportCreteria.Gender) ? (true) : (_Employee.Gender == _PayrollReportCreteria.Gender))
                                            select new CurrentSalariesReportList
                                            {
                                                EmployeeId = _Employee.Id,
                                                EmployeeNo = _Employee.MachineId,
                                                EmployeeName = _Employee.Name + ' ' + _Employee.FatherName,
                                                DepartmentName = _Department.Name,
                                                DesignationName = _Designation.Name,
                                                DateOfJoin = _Employee.DateofJoin,
                                                Allowance = 0

                                            }).ToListAsync();

                if (_EmployeeTable == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                if (_EmployeeTable.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_EmployeeTable.Count() > 0)
                {
                    foreach (var _TableEmployeeRecord in _EmployeeTable)
                    {
                        var _Salary = await _context.Salaries.Where(a => a.EmployeeId == _TableEmployeeRecord.EmployeeId).OrderByDescending(x => x.Date).FirstOrDefaultAsync();
                        if (_Salary != null)
                        {
                            _TableEmployeeRecord.CurrentSalary = _Salary.CurrentAmount;
                        }
                    }
                }

                _CurrentSalariesReportBaseModel.CurrentSalariesReportLists.AddRange(_EmployeeTable);

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CurrentSalariesReportBaseModel;
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