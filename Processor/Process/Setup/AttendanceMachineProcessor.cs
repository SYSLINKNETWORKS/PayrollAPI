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

namespace TWP_API_Payroll.Processor.Process.Payroll
{
    public class AttendanceMachineProcessor : IProcessor<AttendanceMachineBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public AttendanceMachineProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.AttendanceMachine, _context);
        }
        public async Task<ApiResponse> ProcessGet(Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataAsync(_User);

                if (Convert.ToInt32(response.statusCode) == 200)
                {
                    var _Table = (IEnumerable<AttendanceMachine>)response.data;
                    var result = (from ViewTable in _Table
                                  select new AttendanceMachineViewModel
                                  {
                                      Id = ViewTable.Id,
                                      Name = ViewTable.Name,
                                      AttendanceMachineCategoryName = ViewTable.attendanceMachineCategory.Name,
                                      AttendanceMachineGroupName = ViewTable.attendanceMachineGroup.Name,
                                      MacIP = ViewTable.Ip,
                                      Port = ViewTable.Port,
                                      Type = ViewTable.Type,
                                      Active = ViewTable.Active,
                                      NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                                      UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                                      DeletePermission = _UserMenuPermissionAsync.Delete_Permission,

                                  }).ToList();
                    response.data = result;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessGetById(Guid _Id, Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataByIdAsync(_Id, _User);
                if (Convert.ToInt32(response.statusCode) == 200)
                {
                    var _Table = (AttendanceMachine)response.data;
                    var _ViewModel = new AttendanceMachineViewByIdModel
                    {
                        Id = _Table.Id,
                        Name = _Table.Name,
                        AttendanceMachineCategoryId = _Table.AttendanceMachineCategoryId,
                        AttendanceMachineCategoryName = _Table.attendanceMachineCategory.Name,
                        AttendanceMachineGroupId = _Table.AttendanceMachineGroupId,
                        AttendanceMachineGroupName = _Table.attendanceMachineGroup.Name,
                        MacIP = _Table.Ip,
                        Port = _Table.Port,
                        Type = _Table.Type,
                        Active = _Table.Active
                    };
                    response.data = _ViewModel;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessPost(object request, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                var _request = (AttendanceMachineAddModel)request;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var _Table = new AttendanceMachine
                {
                    Name = _request.Name,
                    AttendanceMachineCategoryId = _request.AttendanceMachineCategoryId,
                    Ip = _request.MacIP,
                    Port = _request.Port,
                    AttendanceMachineGroupId = _request.AttendanceMachineGroupId,
                    CompanyId = _UserMenuPermissionAsync.CompanyId,
                    Type = _request.Type,
                    Active = _request.Active
                };
                return await _AbsBusiness.AddAsync(_Table, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessPut(object request, ClaimsPrincipal _User)
        {

            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                var _request = (AttendanceMachineUpdateModel)request;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Table = new AttendanceMachine
                {
                    Id = _request.Id,
                    Name = _request.Name,
                    AttendanceMachineCategoryId = _request.AttendanceMachineCategoryId,
                    AttendanceMachineGroupId = _request.AttendanceMachineGroupId,
                    Ip = _request.MacIP,
                    Port = _request.Port,
                    Type = _request.Type,
                    Active = _request.Active
                };
                return await _AbsBusiness.UpdateAsync(_Table, _User);

            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessDelete(object request, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                var _request = (AttendanceMachineDeleteModel)request;

                Guid _Id = _request.Id;
                Guid _MenuId = _request.Menu_Id;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Delete_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                return await _AbsBusiness.DeleteAsync(_Id, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

    }
}