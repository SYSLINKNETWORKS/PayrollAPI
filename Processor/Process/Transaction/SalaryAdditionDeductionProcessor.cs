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
    public class SalaryAdditionDeductionProcessor : IProcessor<SalaryAdditionDeductionBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public SalaryAdditionDeductionProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.SalaryAdditionDeduction, _context);
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
                    var _Table = (IEnumerable<SalaryAdditionDeduction>)response.data;
                    var result = (from ViewTable in _Table
                                  select new SalaryAdditionDeductionViewModel
                                  {
                                      Id = ViewTable.Id,
                                      EmployeeId = ViewTable.EmployeeId,
                                      Employee = ViewTable.Employee.Name,
                                      Date = ViewTable.Date,
                                      AdditionAmount = ViewTable.AdditionAmount,
                                      DeductionAmount = ViewTable.DeductionAmount,
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
                    var _Table = (SalaryAdditionDeduction)response.data;
                    var _ViewModel = new SalaryAdditionDeductionViewByIdModel
                    {
                        Id = _Table.Id,
                        Date = _Table.Date,
                        EmployeeId = _Table.EmployeeId,
                        EmployeeName = _Table.Employee.Name,
                        AdditionAmount = _Table.AdditionAmount,
                        DeductionAmount = _Table.DeductionAmount,
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
                var _request = (SalaryAdditionDeductionAddModel)request;

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

                var _Table = new SalaryAdditionDeduction
                {
                    Date = _request.Date,
                    AdditionAmount = _request.AdditionAmount,
                    DeductionAmount = _request.DeductionAmount,
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
                var _request = (SalaryAdditionDeductionUpdateModel)request;

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

                var _Table = new SalaryAdditionDeduction
                {
                    Id = _request.Id,
                    Date = _request.Date,
                    EmployeeId = _request.EmployeeId,
                    AdditionAmount = _request.AdditionAmount,
                    DeductionAmount = _request.DeductionAmount

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
                var _request = (SalaryAdditionDeductionDeleteModel)request;

                Guid _MenuId = _request.Menu_Id;

                var _table = await _context.SalaryAdditionDeductions.Where(x => x.Id == _request.Id).FirstOrDefaultAsync();
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