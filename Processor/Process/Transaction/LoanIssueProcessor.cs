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
    public class LoanIssueProcessor : IProcessor<LoanIssueBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public LoanIssueProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.LoanIssue, _context);
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
                    var _Table = (IEnumerable<LoanIssue>)response.data;
                    var result = (from ViewTable in _Table
                                  select new LoanIssueViewModel
                                  {
                                      Id = ViewTable.Id,
                                      EmployeeId = ViewTable.EmployeeId,
                                      Employee = ViewTable.Employee.Name + " " + ViewTable.Employee.FatherName,
                                      LoanCategoryId = ViewTable.LoanCategoryId,
                                      LoanCategory = ViewTable.LoanCategory.Name,
                                      Date = ViewTable.Date,
                                      Amount = ViewTable.Amount,
                                      NoOfInstalment = ViewTable.NoOfInstalment,
                                      InstalmentAmount = ViewTable.InstalmentAmount,
                                      Remarks = ViewTable.Remarks,
                                      LoanStatus = ViewTable.LoanStatus,
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
                    var _Table = (LoanIssue)response.data;
                    var _ViewModel = new LoanIssueViewByIdModel
                    {
                        Id = _Table.Id,
                        Date = _Table.Date,
                        EmployeeId = _Table.EmployeeId,
                        EmployeeName = _Table.Employee.Name + " " + _Table.Employee.FatherName,
                        LoanCategoryId = _Table.LoanCategoryId,
                        LoanCategoryName = _Table.LoanCategory.Name,
                        Amount = _Table.Amount,
                        NoOfInstalment = _Table.NoOfInstalment,
                        InstalmentAmount = _Table.InstalmentAmount,
                        Remarks = _Table.Remarks,
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
                var _request = (LoanIssueAddModel)request;


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

                var _Table = new LoanIssue
                {
                    Date = _request.Date,
                    Amount = _request.Amount,
                    NoOfInstalment = _request.NoOfInstalment,
                    InstalmentAmount = _request.InstalmentAmount,
                    EmployeeId = _request.EmployeeId,
                    LoanCategoryId = _request.LoanCategoryId,
                    Remarks = _request.Remarks,
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
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                var _request = (LoanIssueUpdateModel)request;

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

                string error = "";
                bool _NameExists = _context.LoanIssues.Any(rec => rec.Date.Equals(_request.Date) && rec.EmployeeId.Equals(_request.EmployeeId) && rec.Id != _request.Id && rec.Action != Enums.Operations.D.ToString());

                if (_NameExists)
                {
                    error = error + "Name";
                }

                if (_NameExists)
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = error + " Already Exist";
                    return apiResponse;
                }

                var result = _context.LoanIssues.Where(a => a.Id == _request.Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (result == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found ";
                    return apiResponse;
                }

                result.Action = Enums.Operations.E.ToString();
                result.Type = Enums.Operations.U.ToString();
                result.Date = _request.Date;
                result.EmployeeId = _request.EmployeeId;
                result.Amount = _request.Amount;
                result.NoOfInstalment = _request.NoOfInstalment;
                result.InstalmentAmount = _request.InstalmentAmount;
                result.Remarks = _request.Remarks;
                result.UserNameUpdate = _UserName;
                result.DeleteDate = DateTime.Now;

                return await _AbsBusiness.UpdateAsync(result, _User);

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
                var _request = (LoanIssueDeleteModel)request;

                Guid _MenuId = _request.Menu_Id;

                var _table = await _context.LoanIssues.Where(x => x.Id == _request.Id).FirstOrDefaultAsync();
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