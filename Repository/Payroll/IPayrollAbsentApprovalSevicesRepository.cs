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
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Report;

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollAbsentApprovalSevicesRepository
    {

        Task<ApiResponse> GetAbsentApprovalLovAsync(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date);
        Task<ApiResponse> AddAbsentApprovalLovAsync(AbsentApprovalAddModel _AbsentApprovalAddModel, ClaimsPrincipal _User);

    }
    public class PayrollAbsentApprovalSevicesRepository : IPayrollAbsentApprovalSevicesRepository
    {
        private readonly DataContext _context = null;

        private ErrorLog _ErrorLog = new ErrorLog();
        String _UserName = "";
        vCheckTimeTable _vCheckTimeTable;
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        public PayrollAbsentApprovalSevicesRepository(DataContext context)
        {
            _context = context;
            _vCheckTimeTable = new vCheckTimeTable(_context);
        }

        public async Task<ApiResponse> GetAbsentApprovalLovAsync(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                PayrollReportCreteria _PayrollReportCreteria = new PayrollReportCreteria();

                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                if (Convert.ToInt32(apiResponse.statusCode) == 200)
                {
                    // var _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                    // var _userInformation = await _context.UserLoginAudits.Where(x => x.Key == _UserKey.ToString()).Include(u => u.ApplicationUsers).Include(b => b.Branches).Include(c => c.Companies).Include(y => y.FinancialYears).FirstOrDefaultAsync();
                    _PayrollReportCreteria.DateFrom = _Date;
                    _PayrollReportCreteria.DateTo = _Date;

                    ApiResponse = await _SecurityHelper.UserInfo(_TokenString);
                    if (ApiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return ApiResponse; }
                    var _userInformation = (UserLoginInfoBaseModel)ApiResponse.data;

                    var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_userInformation, _PayrollReportCreteria);



                    //                    var vCheckTimeViewModelBaseModel = await _vCheckTimeTable.vCheckTimeAsync(_TokenString, _PayrollReportCreteria);

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
                    var _vCheckTimeViewTable = vCheckTimeViewModelBaseModel.vCheckTimeViewModel.Where(x => x.AbsentCheck == true).ToList();
                    List<AbsentApprovalGetApprovalModel> _AbsentApprovalGetApprovalModel = new List<AbsentApprovalGetApprovalModel>();
                    foreach (var item in _vCheckTimeViewTable)
                    {
                        var _AbsentTable = await _context.Absents.Where(x => x.EmployeeId == item.EmployeeId && x.Date == item.DailyDate).FirstOrDefaultAsync();
                        bool Approved = false;
                        bool ApprovedAdjust = false;
                        string ApprovedAdjustType = "";

                        if (_AbsentTable != null)
                        {
                            Approved = _AbsentTable.Approved;
                            ApprovedAdjust = _AbsentTable.ApprovedAdjust;
                            ApprovedAdjustType = _AbsentTable.ApprovedAdjustType;

                        }
                        _AbsentApprovalGetApprovalModel.Add(new AbsentApprovalGetApprovalModel
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeName = item.EmployeeName,
                            Date = item.DailyDate,
                            Approved = Approved, // _Absent.Approved,
                            ApprovedAdjust = ApprovedAdjust, // _Absent.ApprovedAdjust,
                            ApprovedAdjustType = ApprovedAdjustType, // _Absent.ApprovedAdjustType,
                            NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                            UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                            DeletePermission = _UserMenuPermissionAsync.Delete_Permission
                        });
                    }

                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    ApiResponse.data = _AbsentApprovalGetApprovalModel;
                    return ApiResponse;
                }
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = "Invalid Class";
                return apiResponse;
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

        public async Task<ApiResponse> AddAbsentApprovalLovAsync(AbsentApprovalAddModel _AbsentApprovalAddModel, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();
            try
            {
                _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                ApiResponse apiResponse = new ApiResponse();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_AbsentApprovalAddModel.MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }

                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                var _EmployeeTable = await _context.Employees.Where(x => x.Id == _AbsentApprovalAddModel.EmployeeId).FirstOrDefaultAsync();
                if (_EmployeeTable == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Invalid Employee";
                    return apiResponse;
                }

                var dt = Convert.ToDateTime(_AbsentApprovalAddModel.Date.ToShortDateString());
                var _Table = await _context.Absents.Where(a => a.Date == dt && a.EmployeeId == _EmployeeTable.Id && a.Type == Enums.Operations.U.ToString() && a.Action != Enums.Operations.D.ToString()).FirstOrDefaultAsync();
                if (_Table != null)
                {
                    _context.RemoveRange(_Table);
                    _context.SaveChanges();
                }
                var _TableAnnualLeaveAdjustment = await _context.AnnualLeaveAdjustments.Where(a => a.Date == dt && a.EmployeeId == _EmployeeTable.Id && a.Type == Enums.Operations.U.ToString() && a.Action != Enums.Operations.D.ToString()).FirstOrDefaultAsync();
                if (_TableAnnualLeaveAdjustment != null)
                {
                    _context.RemoveRange(_TableAnnualLeaveAdjustment);
                    _context.SaveChanges();
                }

                bool _approved = false;
                if (_AbsentApprovalAddModel.Approved.Trim() == "1") { _approved = true; }

                bool _approvedAdjust = false;
                if (_AbsentApprovalAddModel.ApprovedAdjust.Trim() == "1") { _approvedAdjust = true; }

                var _model = new Absent();
                var _modelAnnualLeaveAdjustment = new AnnualLeaveAdjustment();
                int result = 0;

                _model.Date = _AbsentApprovalAddModel.Date;
                _model.EmployeeId = _AbsentApprovalAddModel.EmployeeId;
                _model.Approved = _approved;
                _model.ApprovedAdjust = _approvedAdjust;
                _model.ApprovedAdjustType = _AbsentApprovalAddModel.ApprovedAdjustType;
                _model.Type = Enums.Operations.U.ToString();
                _model.Action = Enums.Operations.E.ToString();
                _model.CompanyId = _UserMenuPermissionAsync.CompanyId;
                _model.UpdateDate = DateTime.Now;
                _model.UserNameUpdate = _UserName;
                await _context.Absents.AddAsync(_model);
                result = _context.SaveChanges();

                if (_approvedAdjust && _approved)
                {
                    _modelAnnualLeaveAdjustment.Date = _AbsentApprovalAddModel.Date;
                    _modelAnnualLeaveAdjustment.LeaveAdjust = 1;
                    _modelAnnualLeaveAdjustment.ApprovedAdjustType = _AbsentApprovalAddModel.ApprovedAdjustType;
                    _modelAnnualLeaveAdjustment.EmployeeId = _AbsentApprovalAddModel.EmployeeId;
                    _modelAnnualLeaveAdjustment.Type = Enums.Operations.U.ToString(); ;
                    _modelAnnualLeaveAdjustment.Action = Enums.Operations.A.ToString();
                    _modelAnnualLeaveAdjustment.CompanyId = _UserMenuPermissionAsync.CompanyId;
                    _modelAnnualLeaveAdjustment.InsertDate = DateTime.Now;
                    _modelAnnualLeaveAdjustment.UserNameInsert = _UserName;
                    await _context.AnnualLeaveAdjustments.AddAsync(_modelAnnualLeaveAdjustment);
                    result = _context.SaveChanges();
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = "Record Updated " + _model.Date;
                return ApiResponse;
            }
            catch (Exception e)
            {
                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
               string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
    }
}