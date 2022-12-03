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
    public class RosterGroupProcessor : IProcessor<RosterGroupBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public RosterGroupProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.RosterGroup, _context);
        }
        public async Task<ApiResponse> ProcessGet(Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
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
                    var _Table = (IEnumerable<RosterGroup>)response.data;
                    var result = (from ViewTable in _Table
                                  select new RosterGroupViewModel
                                  {
                                      Id = ViewTable.Id,
                                      Roster=ViewTable.roster.Name,
                                      Date=ViewTable.Date,
                                      Late=ViewTable.Late,
                                      Overtime=ViewTable.OverTime,
                                      WorkingHours=ViewTable.WorkingHours,
                                      Type = ViewTable.Type,
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
                apiResponse =await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
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
                    var _Table = (RosterGroup)response.data;
                    var _ViewModel = new RosterGroupViewByIdModel
                    {
                        Id = _Table.Id,
                        Date=_Table.Date,
                        RosterId=_Table.RosterId,
                        RosterName=_Table.roster.Name,
                        Late=_Table.Late,
                        EarlyGoing=_Table.EarlyGoing,
                        Overtime=_Table.OverTime,
                        WorkingHours=_Table.WorkingHours,
                        MorningWorkingHours=_Table.MorningWorkingHours,
                        EveningWorkingHours=_Table.EveningWorkingHours,
                        MondayCheck=_Table.MondayCheck,
                        MondayInn=_Table.MondayInn,
                        MondayOut=_Table.MondayOut,
                        TuesdayCheck=_Table.TuesdayCheck,
                        TuesdayInn=_Table.TuesdayInn,
                        TuesdayOut=_Table.TuesdayOut,
                        WednesdayCheck=_Table.WednesdayCheck,
                        WednesdayInn=_Table.WednesdayInn,
                        WednesdayOut=_Table.WednesdayOut,
                        ThursdayCheck=_Table.ThursdayCheck,
                        ThursdayInn=_Table.ThursdayInn,
                        ThursdayOut=_Table.ThursdayOut,
                        FridayCheck=_Table.FridayCheck,
                        FridayInn=_Table.FridayInn,
                        FridayOut=_Table.FridayOut,
                        SaturdayCheck=_Table.SaturdayCheck,
                        SaturdayInn=_Table.SaturdayInn,
                        SaturdayOut=_Table.SaturdayOut,
                        SundayCheck=_Table.SundayCheck,
                        SundayInn=_Table.SundayInn,
                        SundayOut=_Table.SundayOut,
                        Type = _Table.Type

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
                var _request = (RosterGroupAddModel)request;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var _Table = new RosterGroup
                {
                    Date=_request.Date,
                    RosterId=_request.RosterId,
                    Late=_request.Late,
                    EarlyGoing=_request.EarlyGoing,
                    OverTime=_request.Overtime,
                    WorkingHours=_request.WorkingHours,
                    MorningWorkingHours=_request.MorningWorkingHours,
                    EveningWorkingHours=_request.EveningWorkingHours,
                    MondayCheck=_request.MondayCheck,
                    MondayInn=_request.MondayInn,
                    MondayOut=_request.MondayOut,
                    TuesdayCheck=_request.TuesdayCheck,
                    TuesdayInn=_request.TuesdayInn,
                    TuesdayOut=_request.TuesdayOut,
                    WednesdayCheck=_request.WednesdayCheck,
                    WednesdayInn=_request.WednesdayInn,
                    WednesdayOut=_request.WednesdayOut,
                    ThursdayCheck=_request.ThursdayCheck,
                    ThursdayInn=_request.ThursdayInn,
                    ThursdayOut=_request.ThursdayOut,
                    FridayCheck=_request.FridayCheck,
                    FridayInn=_request.FridayInn,
                    FridayOut=_request.FridayOut,
                    SaturdayCheck=_request.SaturdayCheck,
                    SaturdayInn=_request.SaturdayInn,
                    SaturdayOut=_request.SaturdayOut,
                    SundayCheck=_request.SundayCheck,
                    SundayInn=_request.SundayInn,
                    SundayOut=_request.SundayOut,
                    CompanyId = _UserMenuPermissionAsync.CompanyId,
                    Type = _request.Type

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
                var _request = (RosterGroupUpdateModel)request;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                var _Table = new RosterGroup
                {
                    Id = _request.Id,
                    Date=_request.Date,
                    RosterId=_request.RosterId,
                    Late=_request.Late,
                    EarlyGoing=_request.EarlyGoing,
                    OverTime=_request.Overtime,
                    WorkingHours=_request.WorkingHours,
                    MorningWorkingHours=_request.MorningWorkingHours,
                    EveningWorkingHours=_request.EveningWorkingHours,
                    MondayCheck=_request.MondayCheck,
                    MondayInn=_request.MondayInn,
                    MondayOut=_request.MondayOut,
                    TuesdayCheck=_request.TuesdayCheck,
                    TuesdayInn=_request.TuesdayInn,
                    TuesdayOut=_request.TuesdayOut,
                    WednesdayCheck=_request.WednesdayCheck,
                    WednesdayInn=_request.WednesdayInn,
                    WednesdayOut=_request.WednesdayOut,
                    ThursdayCheck=_request.ThursdayCheck,
                    ThursdayInn=_request.ThursdayInn,
                    ThursdayOut=_request.ThursdayOut,
                    FridayCheck=_request.FridayCheck,
                    FridayInn=_request.FridayInn,
                    FridayOut=_request.FridayOut,
                    SaturdayCheck=_request.SaturdayCheck,
                    SaturdayInn=_request.SaturdayInn,
                    SaturdayOut=_request.SaturdayOut,
                    SundayCheck=_request.SundayCheck,
                    SundayInn=_request.SundayInn,
                    SundayOut=_request.SundayOut,
                    Type = _request.Type

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
                var _request = (RosterGroupDeleteModel)request;

                Guid _Id = _request.Id;
                Guid _MenuId = _request.Menu_Id;

                apiResponse =await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
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