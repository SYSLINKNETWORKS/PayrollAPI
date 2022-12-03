using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Processor.Process.Payroll {
    public class AnnualLeavesProcessor : IProcessor<AnnualLeavesBaseModel> {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper ();

        public AnnualLeavesProcessor (App_Data.DataContext context) {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass (Enums.ClassName.AnnualLeaves, _context);
        }
        public async Task<ApiResponse> ProcessGet (Guid _MenuId, ClaimsPrincipal _User) {
            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_MenuId, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataAsync (_User);

                if (Convert.ToInt32 (response.statusCode) == 200) {
                    var _Table = (IEnumerable<AnnualLeaves>) response.data;
                    var result = (from ViewTable in _Table select new AnnualLeavesViewModel {
                        Id = ViewTable.Id,
                            Name = ViewTable.Name,
                            AnnualLeaveAllow=ViewTable.AnnualLeaveAllow,
                            AnnualLeaveDays=ViewTable.AnnualLeaveDays,
                            SickLeaveAllow=ViewTable.SickLeaveAllow,
                            SickLeaveDays=ViewTable.SickLeaveDays,
                            CasualLeaveAllow=ViewTable.CasualLeaveAllow,
                            CasualLeaveDays=ViewTable.CasualLeaveDays,
                            Type = ViewTable.Type,
                            Active = ViewTable.Active,
                            NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                            UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                            DeletePermission = _UserMenuPermissionAsync.Delete_Permission,

                    }).ToList ();
                    response.data = result;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessGetById (Guid _Id, Guid _MenuId, ClaimsPrincipal _User) {
            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_MenuId, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataByIdAsync (_Id, _User);
                if (Convert.ToInt32 (response.statusCode) == 200) {
                    var _Table = (AnnualLeaves) response.data;
                    var _ViewModel = new AnnualLeavesViewByIdModel {
                        Id = _Table.Id,
                        Name = _Table.Name,
                        AnnualLeaveAllow=_Table.AnnualLeaveAllow,
                        AnnualLeaveDays=_Table.AnnualLeaveDays,
                        SickLeaveAllow=_Table.SickLeaveAllow,
                        SickLeaveDays=_Table.SickLeaveDays,
                        CasualLeaveAllow=_Table.CasualLeaveAllow,
                        CasualLeaveDays=_Table.CasualLeaveDays,
                        Type = _Table.Type,
                        Active = _Table.Active
                    };
                    response.data = _ViewModel;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessPost (object request, ClaimsPrincipal _User) {
            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                var _request = (AnnualLeavesAddModel) request;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }
                var _Table = new AnnualLeaves {
                    Name = _request.Name,
                    AnnualLeaveAllow=_request.AnnualLeaveAllow,
                    AnnualLeaveDays=_request.AnnualLeaveDays,
                    SickLeaveAllow=_request.SickLeaveAllow,
                    SickLeaveDays=_request.SickLeaveDays,
                    CasualLeaveAllow=_request.CasualLeaveAllow,
                    CasualLeaveDays=_request.CasualLeaveDays,
                    CompanyId = _UserMenuPermissionAsync.CompanyId,
                    Type = _request.Type,
                    Active = _request.Active
                };
                return await _AbsBusiness.AddAsync (_Table, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessPut (object request, ClaimsPrincipal _User) {

            

            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                var _request = (AnnualLeavesUpdateModel) request;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }

                var _Table = new AnnualLeaves {
                    Id = _request.Id,
                    Name = _request.Name,      
                    AnnualLeaveAllow=_request.AnnualLeaveAllow,
                    AnnualLeaveDays=_request.AnnualLeaveDays,
                    SickLeaveAllow=_request.SickLeaveAllow,
                    SickLeaveDays=_request.SickLeaveDays,
                    CasualLeaveAllow=_request.CasualLeaveAllow,
                    CasualLeaveDays=_request.CasualLeaveDays,              
                    Type = _request.Type,
                    Active = _request.Active
                };
                return await _AbsBusiness.UpdateAsync (_Table, _User);

            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessDelete (object request, ClaimsPrincipal _User) {
            ApiResponse apiResponse = new ApiResponse ();
            if (_AbsBusiness != null) {
                var _request = (AnnualLeavesDeleteModel) request;

                Guid _Id = _request.Id;
                Guid _MenuId = _request.Menu_Id;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync (_MenuId, _User);
                if (apiResponse.statusCode.ToString () != StatusCodes.Status200OK.ToString ()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel) apiResponse.data;

                if (!_UserMenuPermissionAsync.Delete_Permission) {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString ();
                    return apiResponse;
                }
                return await _AbsBusiness.DeleteAsync (_Id, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

    }
}