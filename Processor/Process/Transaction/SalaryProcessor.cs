using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Processor.Process.Payroll
{
    public class SalaryProcessor : IProcessor<SalaryBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public SalaryProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.Salary, _context);
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
                    var _Table = (IEnumerable<Salary>)response.data;
                    var result = (from ViewTable in _Table
                                  select new SalaryViewModel
                                  {
                                      Id = ViewTable.Id,
                                      EmployeeId = ViewTable.EmployeeId,
                                      Employee = ViewTable.employee.Name,
                                      Date = ViewTable.Date,
                                      PreviousAmount = ViewTable.PreviousAmount,
                                      IncreamentPercentage = ViewTable.IncreamentPercentage,
                                      IncreamentAmount = ViewTable.IncreamentAmount,
                                      CurrentAmount = ViewTable.CurrentAmount,
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
                    var _Table = (Salary)response.data;
                    var _ViewModel = new SalaryViewByIdModel
                    {
                        Id = _Table.Id,
                        Date = _Table.Date,
                        EmployeeId = _Table.EmployeeId,
                        EmployeeName = _Table.employee.Name,
                        PreviousAmount = _Table.PreviousAmount,
                        IncreamentPercentage = _Table.IncreamentPercentage,
                        IncreamentAmount = _Table.IncreamentAmount,
                        CurrentAmount = _Table.CurrentAmount,
                        NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                        UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                        DeletePermission = _UserMenuPermissionAsync.Delete_Permission,
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
                var _request = (SalaryAddModel)request;

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermission = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermission.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                ApiResponse _validateDate = _SecurityHelper.CheckPermission(_UserMenuPermission, _request.Date);
                if (_validateDate.statusCode != "200")
                {
                    return _validateDate;
                }

                var _Table = new Salary
                {
                    Date = _request.Date,
                    PreviousAmount = _request.PreviousAmount,
                    IncreamentPercentage = _request.IncreamentPercentage,
                    IncreamentAmount = _request.IncreamentAmount,
                    CurrentAmount = _request.CurrentAmount,
                    EmployeeId = _request.EmployeeId,
                    CompanyId = _UserMenuPermission.CompanyId,
                    Type = Enums.Operations.U.ToString(),
                    Action = Enums.Operations.A.ToString()
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
                var _request = (SalaryUpdateModel)request;

                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermission = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermission.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                ApiResponse _validateDate = _SecurityHelper.CheckPermission(_UserMenuPermission, _request.Date);
                if (_validateDate.statusCode != "200")
                {
                    return _validateDate;
                }

                var _Table = new Salary
                {
                    Id = _request.Id,
                    Date = _request.Date,
                    EmployeeId = _request.EmployeeId,
                    PreviousAmount = _request.PreviousAmount,
                    IncreamentPercentage = _request.IncreamentPercentage,
                    IncreamentAmount = _request.IncreamentAmount,
                    CurrentAmount = _request.CurrentAmount,

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
                var _request = (SalaryDeleteModel)request;

                Guid _MenuId = _request.Menu_Id;

                var _table = await _context.Salaries.Where(x => x.Id == _request.Id).FirstOrDefaultAsync();
                if (_table == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }



                ApiResponse apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermission = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermission.Delete_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                ApiResponse _validateDate = _SecurityHelper.CheckPermission(_UserMenuPermission, _table.Date);
                if (_validateDate.statusCode != "200")
                {
                    return _validateDate;
                }

                return await _AbsBusiness.DeleteAsync(_table.Id, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

    }
}