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
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollInOutEditorSevicesRepository
    {

        Task<ApiResponse> GetInOutEditorLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _DateFrom, DateTime _DateTo);
        Task<ApiResponse> GetInOutEditorByIDLovAsync(InOutEditorGetByIdModel _InOutEditorGetByIdModel, ClaimsPrincipal _User);
        Task<ApiResponse> AddInOutEditorLovAsync(InOutEditorAddModel _InOutEditorAddModel, ClaimsPrincipal _User);

    }
    public class PayrollInOutEditorSevicesRepository : IPayrollInOutEditorSevicesRepository
    {
        private readonly DataContext _context = null;
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        private ErrorLog _ErrorLog = new ErrorLog();

        public PayrollInOutEditorSevicesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> GetInOutEditorLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _DateFrom, DateTime _DateTo)
        {
            ApiResponse apiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();
            string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            try
            {

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                if (Convert.ToInt32(apiResponse.statusCode) == 200)
                {

                    var _CheckInOuts = await (from _CheckInOutTable in _context.CheckInOuts
                                              join _EmployeeTable in _context.Employees on _CheckInOutTable.MachineId equals _EmployeeTable.MachineId
                                              where (_CheckInOutTable.Action != Enums.Operations.D.ToString() && _CheckInOutTable.Type == Enums.Operations.U.ToString() && _CheckInOutTable.Date >= _DateFrom && _CheckInOutTable.Date <= _DateTo)
                                              select new InOutEditorViewModel
                                              {
                                                  EmployeeId = _EmployeeTable.Id,
                                                  EmployeeName = _EmployeeTable.Name + " " + _EmployeeTable.FatherName,
                                                  MachineId = _EmployeeTable.MachineId,
                                                  Date = _CheckInOutTable.Date,
                                                  Approved = _CheckInOutTable.Approved,
                                                  NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                                                  UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                                                  DeletePermission = _UserMenuPermissionAsync.Delete_Permission
                                              }).Distinct().OrderByDescending(o => o.Date).ToListAsync();

                    if (_CheckInOuts == null)
                    {
                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Record not found";
                        return apiResponse;
                    }
                    if (_CheckInOuts.Count() == 0)
                    {
                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Record not found";
                        return apiResponse;
                    }

                    apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    apiResponse.data = _CheckInOuts;
                    return apiResponse;
                }
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = "Invalid Class";
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;
            }
        }

        public async Task<ApiResponse> GetInOutEditorByIDLovAsync(InOutEditorGetByIdModel _InOutEditorGetByIdModel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();
            string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            try
            {

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_InOutEditorGetByIdModel.MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                if (Convert.ToInt32(apiResponse.statusCode) == 200)
                {

                    var _CheckInOuts = await (from _CheckInOutTable in _context.CheckInOuts
                                              join _EmployeeTable in _context.Employees on _CheckInOutTable.MachineId equals _EmployeeTable.MachineId
                                              join _InOutCategoryTable in _context.InOutCategories on _CheckInOutTable.InOutCategoryId equals _InOutCategoryTable.Id
                                              where (_CheckInOutTable.Action != Enums.Operations.D.ToString() &&
_CheckInOutTable.Type == Enums.Operations.U.ToString() &&
_EmployeeTable.Id == _InOutEditorGetByIdModel.EmployeeId &&
_CheckInOutTable.Date == _InOutEditorGetByIdModel.Date)
                                              select new InOutEditorViewByIdModel
                                              {
                                                  EmployeeId = _EmployeeTable.Id,
                                                  EmployeeName = _EmployeeTable.Name,
                                                  Date = _CheckInOutTable.Date,
                                                  InOutCategoryId = _InOutCategoryTable.Id,
                                                  InOutCategoryName = _InOutCategoryTable.Name,
                                                  Checktime = _CheckInOutTable.CheckTime,
                                                  CheckType = _CheckInOutTable.CheckType,
                                                  Approved = _CheckInOutTable.Approved,

                                              }).OrderByDescending(o => o.Date).ToListAsync();

                    if (_CheckInOuts == null)
                    {
                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Record not found";
                        return apiResponse;
                    }
                    if (_CheckInOuts.Count() == 0)
                    {

                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Record not found";
                        return apiResponse;
                    }

                    apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    apiResponse.data = _CheckInOuts;
                    return apiResponse;
                }
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = "Invalid Class";
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;
            }
        }

        public async Task<ApiResponse> AddInOutEditorLovAsync(InOutEditorAddModel _InOutEditorAddModel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();
            string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
            try
            {

                //Permission
                var _MenuTable = (from _InOutEditorListAddModel in _InOutEditorAddModel.InOutEditorListAddModel select new { MenuId = _InOutEditorListAddModel.MenuId }).Distinct().FirstOrDefault();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuTable.MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }

                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //Attendance Approval Check
                var _InOutEditorListApprovedTable = (from _InOutEditorListAddModel in _InOutEditorAddModel.InOutEditorListAddModel select new { Date = _InOutEditorListAddModel.CheckTime.ToShortDateString() }).Distinct().ToList();
                if (!_InOutEditorAddModel.InOutEditorListAddModel.FirstOrDefault().Tag)
                {
                    foreach (var _RecordApproved in _InOutEditorListApprovedTable)
                    {
                        var dt = Convert.ToDateTime(_RecordApproved.Date);
                        var _TableApproved = _context.CheckInOuts.Where(a => a.Date == dt && a.Type == Enums.Operations.U.ToString() && a.Approved == true && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                        if (_TableApproved != null)
                        {
                            apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                            apiResponse.message = "Attendance already approved";
                            return apiResponse;
                        }
                    }
                }
                //Delete Pervious Record
                var _InOutEditorListAddTable = (from _InOutEditorListAddModel in _InOutEditorAddModel.InOutEditorListAddModel join _EmployeeTable in _context.Employees on _InOutEditorListAddModel.EmployeeId equals _EmployeeTable.Id select new { MachineId = _EmployeeTable.MachineId, Date = _InOutEditorListAddModel.CheckTime.ToShortDateString() }).Distinct().ToList();
                foreach (var _RecordDate in _InOutEditorListAddTable)
                {
                    var dt = Convert.ToDateTime(_RecordDate.Date);
                    var _Table = _context.CheckInOuts.Where(a => a.Date == dt && a.MachineId == _RecordDate.MachineId && a.Type == Enums.Operations.U.ToString() && a.Approved == false && a.Action != Enums.Operations.D.ToString()).ToList();
                    if (_Table != null)
                    {
                        _context.RemoveRange(_Table);
                        _context.SaveChanges();
                    }
                }
                List<CheckInOut> _CheckInOutModel = new List<CheckInOut>();

                foreach (var _Record in _InOutEditorAddModel.InOutEditorListAddModel)
                {

                    //var response = await _AbsBusiness.GetDataAsync(_User);
                    var _EmployeeTable = await _context.Employees.Where(x => x.Id == _Record.EmployeeId).FirstOrDefaultAsync();
                    if (_EmployeeTable == null)
                    {
                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Invalid Employee";
                        return apiResponse;
                    }

                    var dtTime = Convert.ToDateTime(_Record.CheckTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    var _CheckInOutTableSystem = _context.CheckInOuts.Where(a => a.CheckTime == dtTime && a.MachineId == _EmployeeTable.MachineId && a.Type == Enums.Operations.S.ToString()).FirstOrDefault();
                    if (_CheckInOutTableSystem != null)
                    {
                        apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                        apiResponse.message = dtTime.ToString("dd-MMM-yyyy") + " Attendance already mark";
                        return apiResponse;
                    }

                    _CheckInOutModel.Add(new CheckInOut
                    {
                        MachineId = _EmployeeTable.MachineId,
                        CheckTime = _Record.CheckTime,
                        CheckType = _Record.CheckType == "O" ? Enums.Operations.O.ToString() : Enums.Operations.I.ToString(),
                        Type = Enums.Operations.U.ToString(),
                        Action = Enums.Operations.A.ToString(),
                        Approved = false,
                        Date = Convert.ToDateTime(_Record.CheckTime.ToShortDateString()),
                        InOutCategoryId = _Record.InOutCategoryId,
                        CompanyId = _UserMenuPermissionAsync.CompanyId,
                        InsertDate = DateTime.Now,
                        UserNameInsert = _UserName,

                    });

                }
                _context.CheckInOuts.AddRange(_CheckInOutModel);
                await _context.SaveChangesAsync();
                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = "Record Saved ";
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;
            }
        }

    }
}