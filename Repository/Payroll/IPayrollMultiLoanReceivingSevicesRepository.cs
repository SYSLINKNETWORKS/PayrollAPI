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
    public interface IPayrollMultiLoanReceivingSevicesRepository
    {

        Task<ApiResponse> GetMultiLoanReceivingLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date);
        Task<ApiResponse> EditMultiLoanReceivingLovAsync(MultiLoanReceivingEditModel _MultiLoanReceivingEditModel, DateTime _date, ClaimsPrincipal _User);

    }
    public class PayrollMultiLoanReceivingSevicesRepository : IPayrollMultiLoanReceivingSevicesRepository
    {
        private readonly DataContext _context = null;
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        public PayrollMultiLoanReceivingSevicesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> GetMultiLoanReceivingLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
                MultiLoanReceivingBaseModel _MultiLoanReceivingBaseModel = new MultiLoanReceivingBaseModel();
                _MultiLoanReceivingBaseModel.MultiLoanReceivingViewModel = new List<MultiLoanReceivingViewModel>();

                ApiResponse apiResponse = new ApiResponse();


                var apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
                if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //var response = await _AbsBusiness.GetDataAsync(_User);
                if (Convert.ToInt32(apiResponseUser.statusCode) == 200)
                {
                    DateTime now = _Date;
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    var _MultiLoanReceive = await (from _Employee in _context.Employees
                                                   join _LoanIssues in _context.LoanIssues on _Employee.Id equals _LoanIssues.EmployeeId
                                                   join _LoanCategories in _context.LoanCategories on _LoanIssues.LoanCategoryId equals _LoanCategories.Id
                                                   join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                                   join _Department in _context.Departments on _Employee.DepartmentId equals _Department.Id
                                                   where _LoanIssues.Date <= endDate
                                                   select new MultiLoanReceivingViewModel
                                                   {
                                                       Id = _LoanIssues.Id,
                                                       Date = _LoanIssues.Date,
                                                       EmployeeId = _Employee.Id,
                                                       EmployeeMachineId = _Employee.MachineId,
                                                       EmployeeName = _Employee.Name + ' ' + _Employee.FatherName,
                                                       LoanCatgoryId = _LoanIssues.LoanCategoryId,
                                                       LoanCategoryName = _LoanCategories.Name,
                                                       LoanAmount = _LoanIssues.Amount,
                                                       NoofInstalment = _LoanIssues.NoOfInstalment,
                                                       InstallmentAmount = _LoanIssues.InstalmentAmount,
                                                       Receiving = _LoanIssues.InstalmentAmount,
                                                       MenuId = _Menuid,
                                                       NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                                                       UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                                                       DeletePermission = _UserMenuPermissionAsync.Delete_Permission
                                                   }).OrderBy(o => o.EmployeeName).ToListAsync();

                    if (_MultiLoanReceive == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }
                    if (_MultiLoanReceive.Count() == 0)
                    {

                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }

                    if (_MultiLoanReceive.Count() > 0)
                    {
                        foreach (var _TableLoanRecord in _MultiLoanReceive)
                        {
                            double _RecivedAmount = 0;
                            var _TableLoanReceiving = _context.LoanReceives.Where(a => a.LoanIssueId == _TableLoanRecord.Id && a.Type == Enums.Operations.S.ToString() && a.Date == endDate).FirstOrDefault();
                            if (_TableLoanReceiving != null) { _RecivedAmount = _TableLoanReceiving.Amount; _TableLoanRecord.Receiving = _TableLoanReceiving.Amount; }

                            // var _TableLoanReceiving = _context.LoanReceives.Where(a => a.LoanIssueId == _TableLoanRecord.Id && a.Type == Enums.Operations.S.ToString() && a.Date == endDate).FirstOrDefault();
                            // if (_TableLoanReceiving != null) { _TableLoanRecord.Receiving = _TableLoanReceiving.Amount; }

                            var _TableLoanReceived = await _context.LoanReceives.Where(a => a.LoanIssueId == _TableLoanRecord.Id).SumAsync(s => s.Amount);

                            _TableLoanRecord.Received = _TableLoanReceived - _RecivedAmount;
                            _TableLoanRecord.Balance = _TableLoanRecord.LoanAmount - _TableLoanReceived + _RecivedAmount;
                            _TableLoanRecord.Receiving = _TableLoanRecord.Balance < _TableLoanRecord.InstallmentAmount ? _TableLoanRecord.Balance : _TableLoanRecord.Balance > _TableLoanRecord.InstallmentAmount ? _TableLoanRecord.InstallmentAmount : _RecivedAmount;
                            _TableLoanRecord.TotalBalance = _RecivedAmount > 0 ? _TableLoanRecord.LoanAmount - _TableLoanReceived : _TableLoanRecord.LoanAmount - _TableLoanReceived;
                            _TableLoanRecord.Status = (_TableLoanRecord.LoanAmount - _TableLoanReceived - _RecivedAmount) == 0 ? Enums.Status.Complete.ToString() : Enums.Status.InProcess.ToString();
                            // _TableLoanRecord.Received = _TableLoanReceived;
                            // _TableLoanRecord.Balance = _TableLoanRecord.LoanAmount - _TableLoanReceived;
                            // _TableLoanRecord.TotalBalance = _TableLoanRecord.LoanAmount - _TableLoanReceived - _TableLoanRecord.InstallmentAmount;
                            // _TableLoanRecord.Status = (_TableLoanRecord.LoanAmount - _TableLoanReceived) == 0 ? Enums.Status.Complete.ToString() : Enums.Status.InProcess.ToString();

                        }
                    }

                    _MultiLoanReceivingBaseModel.MultiLoanReceivingViewModel.AddRange(_MultiLoanReceive);


                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    ApiResponse.data = _MultiLoanReceive.Where(a => a.Status == Enums.Status.InProcess.ToString());
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
        public async Task<ApiResponse> EditMultiLoanReceivingLovAsync(MultiLoanReceivingEditModel _MultiLoanReceivingEditModel, DateTime _date, ClaimsPrincipal _User)

        {
            var ApiResponse = new ApiResponse();
            try
            {
                string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

                ApiResponse apiResponse = new ApiResponse();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MultiLoanReceivingEditModel.MultiLoanReceivingListAddModel.FirstOrDefault().MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }

                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;
                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                DateTime now = _date;
                var startDate = new DateTime(now.Year, now.Month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);

                var _Table = _context.LoanReceives.Where(a => a.Date == endDate
               && a.Type == Enums.Operations.S.ToString()).ToList();
                if (_Table != null && _Table.Count != 0)
                {
                    _context.RemoveRange(_Table);
                    _context.SaveChanges();
                }

                List<LoanReceive> _LoanReceive = new List<LoanReceive>();
                foreach (var _Record in _MultiLoanReceivingEditModel.MultiLoanReceivingListAddModel)
                {
                    // if (_Record.Receiving > 0)
                    // {
                    _LoanReceive.Add(new LoanReceive
                    {
                        Date = endDate,
                        Amount = _Record.Receiving,
                        LoanIssueId = _Record.LoanIssueId,
                        Type = Enums.Operations.S.ToString(),
                        Action = Enums.Operations.A.ToString(),
                        UserNameInsert = _UserName,
                        InsertDate = DateTime.Now,
                        CompanyId = _UserMenuPermissionAsync.CompanyId,
                    });
                    //}

                }
                _context.LoanReceives.AddRange(_LoanReceive);
                await _context.SaveChangesAsync();

                // var _model = new LoanReceive();
                // int result = 0;
                // _model.Date = endDate;
                // _model.Amount = _MultiLoanReceivingEditModel.Receiving;
                // _model.LoanIssueId = _MultiLoanReceivingEditModel.LoanIssueId;
                // _model.Type = Enums.Operations.M.ToString();
                // _model.Action = Enums.Operations.A.ToString();
                // _model.UserNameInsert = _UserName;
                // _model.InsertDate = DateTime.Now;
                // _model.CompanyId = _UserMenuPermissionAsync.CompanyId;
                // await _context.LoanReceives.AddAsync(_model);
                // result = _context.SaveChanges();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Record Saved ";
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