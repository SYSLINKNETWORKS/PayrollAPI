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

namespace TWP_API_Payroll.Repository {
    public interface IPayrollInOutEditorApprovalSevicesRepository {

        Task<ApiResponse> GetInOutEditorApprovalLovAsync (ClaimsPrincipal _User, Guid _Menuid, DateTime _DateFrom, DateTime _DateTo);
        Task<ApiResponse> EditInOutEditorApprovalLovAsync (InOutEditorApprovalEditModel _InOutEditorApprovalEditModel, ClaimsPrincipal _User);

    }
    public class PayrollInOutEditorApprovalSevicesRepository : IPayrollInOutEditorApprovalSevicesRepository {
        private readonly DataContext _context = null;
        private SecurityHelper _SecurityHelper = new SecurityHelper ();
        public PayrollInOutEditorApprovalSevicesRepository (DataContext context) {
            _context = context;
        }

        public async Task<ApiResponse> GetInOutEditorApprovalLovAsync (ClaimsPrincipal _User, Guid _Menuid, DateTime _DateFrom, DateTime _DateTo) {
            var ApiResponse = new ApiResponse ();
            try {

                string _UserId = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserId.ToString ())?.Value.ToString ();
                ApiResponse apiResponse = new ApiResponse ();
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_Menuid, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                if (Convert.ToInt32 (apiResponse.statusCode) == 200) {

                    var _CheckInOutDetail = await (from _CheckInOutTable in _context.CheckInOuts join _EmployeeTable in _context.Employees on _CheckInOutTable.MachineId equals _EmployeeTable.MachineId where (_CheckInOutTable.Action != Enums.Operations.D.ToString () && _CheckInOutTable.Date >= _DateFrom && _CheckInOutTable.Date <= _DateTo &&
                        _CheckInOutTable.Type == Enums.Operations.U.ToString ()) select new InOutEditorGetApprovalDetailModel {
                        MachineId = _CheckInOutTable.MachineId,
                            EmployeeName = _EmployeeTable.Name.Trim () + ' ' + _EmployeeTable.FatherName.Trim (),
                            Date = Convert.ToDateTime (Convert.ToDateTime (_CheckInOutTable.CheckTime).ToString ("yyyy-MM-dd")),
                            CheckTime = _CheckInOutTable.CheckTime,
                            Approved = _CheckInOutTable.Approved,
                            NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                            UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                            DeletePermission = _UserMenuPermissionAsync.Delete_Permission
                    }).OrderByDescending (o => o.CheckTime).ToListAsync ();

                    if (_CheckInOutDetail == null) {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }
                    if (_CheckInOutDetail.Count () == 0) {

                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }
                    //InOutEditorGetApprovalSummaryModel _CheckInOutSummary = new InOutEditorGetApprovalSummaryModel ();
                    var _CheckInOutSummary = (from _CheckInOutDetailSummary in _CheckInOutDetail group _CheckInOutDetailSummary by new { _CheckInOutDetailSummary.Date, _CheckInOutDetailSummary.Approved } into grp

                        select new InOutEditorGetApprovalSummaryModel {
                            Date = grp.Key.Date,
                                Approved = grp.Key.Approved,
                        }).ToList ();

                    InOutEditorGetApprovalModel _InOutEditorGetApprovalModel = new InOutEditorGetApprovalModel ();
                    _InOutEditorGetApprovalModel.InOutEditorGetApprovalDetailModels = _CheckInOutDetail;
                    _InOutEditorGetApprovalModel.inOutEditorGetApprovalSummaryModels = _CheckInOutSummary;

                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                    ApiResponse.data = _InOutEditorGetApprovalModel;
                    return ApiResponse;
                }
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                apiResponse.message = "Invalid Class";
                return apiResponse;
            } catch (Exception e) {

                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = e.Message.ToString () + innerexp;
                return ApiResponse;
            }
        }
        public async Task<ApiResponse> EditInOutEditorApprovalLovAsync (InOutEditorApprovalEditModel _InOutEditorApprovalEditModel, ClaimsPrincipal _User)

        {
            var ApiResponse = new ApiResponse ();
            try {

                string _UserId = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserId.ToString ())?.Value.ToString ();

                ApiResponse apiResponse = new ApiResponse ();
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_InOutEditorApprovalEditModel.MenuId, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }

                var _CheckTime = Convert.ToDateTime (Convert.ToDateTime (_InOutEditorApprovalEditModel.CheckTime).ToString ("yyyy-MM-dd HH:mm:ss"));
                var _CheckDate = Convert.ToDateTime (Convert.ToDateTime (_InOutEditorApprovalEditModel.Date).ToString ("yyyy-MM-dd"));

                var result = await (from _CheckInOuts in _context.CheckInOuts where _CheckInOuts.Type == Enums.Operations.U.ToString () &&
                    _CheckInOuts.Action != Enums.Operations.D.ToString () &&
                    (string.IsNullOrEmpty (_InOutEditorApprovalEditModel.CheckTime) ? (true) : _CheckInOuts.CheckTime == _CheckTime) &&
                    (string.IsNullOrEmpty (_InOutEditorApprovalEditModel.Date) ? (true) : _CheckInOuts.Date == _CheckDate) &&
                    (_InOutEditorApprovalEditModel.MachineId == 0 ? (true) : _CheckInOuts.MachineId == _InOutEditorApprovalEditModel.MachineId)

                    select _CheckInOuts).ToListAsync ();
                if (result == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }
                if (result.Count () == 0) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                bool _approved = false;
                if (_InOutEditorApprovalEditModel.Approved.Trim () == "1") { _approved = true; }
                foreach (var item in result) {
                    item.Approved = _approved;
                }

                _context.UpdateRange (result);
                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Updated ";
                return ApiResponse;
            } catch (Exception e) {

                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = e.Message.ToString () + innerexp;
                return ApiResponse;
            }
        }
    }
}