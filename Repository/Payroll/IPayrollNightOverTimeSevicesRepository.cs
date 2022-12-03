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
    public interface IPayrollNightOverTimeSevicesRepository
    {

        Task<ApiResponse> GetNightOverTimeLovAsync(ClaimsPrincipal _User, DateTime _DateAsOn);
        Task<ApiResponse> AddNightOverTimeLovAsync(NightOverTimeAddModel _NightOverTimeAddModel, ClaimsPrincipal _User);

    }
    public class PayrollNightOverTimeSevicesRepository : IPayrollNightOverTimeSevicesRepository
    {
        private readonly DataContext _context = null;
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        private ErrorLog _ErrorLog = new ErrorLog();

        public PayrollNightOverTimeSevicesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> GetNightOverTimeLovAsync(ClaimsPrincipal _User, DateTime _DateAsOn)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _NightOverTimeEmployeeByIdModel = await (from _CheckInOuts in _context.CheckInOuts
                                                             join _Employee in _context.Employees on _CheckInOuts.MachineId equals _Employee.MachineId
                                                             join _Designation in _context.Designations on _Employee.DesignationId equals _Designation.Id
                                                             where _CheckInOuts.Date == _DateAsOn && _CheckInOuts.CheckType == Enums.Payroll.I.ToString()
                                                             select new NightOverTimeEmployeeByIdModel
                                                             {
                                                                 EmployeeId = _Employee.Id,
                                                                 EmployeeName = _Employee.Name.Trim() + " " + _Employee.FatherName.Trim(),
                                                                 DesignationName = _Designation.Name,
                                                                 Date = _DateAsOn,
                                                                 OverTime = 0,
                                                                 Remarks = "",
                                                             }).Distinct().OrderBy(o => o.EmployeeName).ToListAsync();
                foreach (var _RecordInn in _NightOverTimeEmployeeByIdModel)
                {
                    var _NightShiftTable = await _context.NightOverTimes.Where(i => i.Date == _DateAsOn && i.EmployeeId == _RecordInn.EmployeeId).FirstOrDefaultAsync();
                    if (_NightShiftTable != null)
                    {
                        _RecordInn.OverTime = _NightShiftTable.OverTime;
                        _RecordInn.Remarks = _NightShiftTable.Remarks;
                    }
                }


                if (_NightOverTimeEmployeeByIdModel.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;

                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _NightOverTimeEmployeeByIdModel;
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
        public async Task<ApiResponse> AddNightOverTimeLovAsync(NightOverTimeAddModel _NightOverTimeAddModel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();
            string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
            try
            {

                //Permission
                var _MenuTable = (from _InOutEditorListAddModel in _NightOverTimeAddModel.NightOverTimeListAddModel select new { MenuId = _InOutEditorListAddModel.MenuId }).Distinct().FirstOrDefault();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuTable.MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }

                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                //OverTime Approval Check


                DateTime _date = new DateTime(_NightOverTimeAddModel.NightOverTimeListAddModel.FirstOrDefault().Date.Year, _NightOverTimeAddModel.NightOverTimeListAddModel.FirstOrDefault().Date.Month, _NightOverTimeAddModel.NightOverTimeListAddModel.FirstOrDefault().Date.Day, 23, 00, 00);


                foreach (var _RecordApproved in _NightOverTimeAddModel.NightOverTimeListAddModel)
                {
                    var _CheckInOutTableSystem = _context.NightOverTimes.Where(a => a.Date == _RecordApproved.Date && a.EmployeeId == _RecordApproved.EmployeeId && a.Approved == true).FirstOrDefault();
                    if (_CheckInOutTableSystem != null)
                    {
                        apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                        apiResponse.message = " Overtime already mark Approved";
                        return apiResponse;
                    }
                }

                //Delete Pervious Record
                // foreach (var _RecordDate in _NightOverTimeAddModel.NightOverTimeListAddModel)
                // {
                var _Table = await _context.NightOverTimes.Where(a => a.Date == _NightOverTimeAddModel.NightOverTimeListAddModel.FirstOrDefault().Date && a.Approved == false && a.Action != Enums.Operations.D.ToString()).ToListAsync();
                if (_Table.Count > 0)
                {
                    _context.RemoveRange(_Table);
                    _context.SaveChanges();
                }

                var _CheckoutTable = await _context.CheckInOuts.Where(a => a.Date == _NightOverTimeAddModel.NightOverTimeListAddModel.FirstOrDefault().Date && a.Type == Enums.Operations.N.ToString()).ToListAsync();
                if (_CheckoutTable.Count > 0)
                {
                    _context.RemoveRange(_CheckoutTable);
                    _context.SaveChanges();
                }
                //                }


                List<NightOverTime> _NightOverTime = new List<NightOverTime>();

                foreach (var _Record in _NightOverTimeAddModel.NightOverTimeListAddModel.Where(x => x.OverTime > 0))
                {

                    var _EmployeeTable = await _context.Employees.Where(x => x.Id == _Record.EmployeeId).FirstOrDefaultAsync();
                    if (_EmployeeTable == null)
                    {
                        apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        apiResponse.message = "Invalid Employee";
                        return apiResponse;
                    }



                    _NightOverTime.Add(new NightOverTime
                    {
                        EmployeeId = _Record.EmployeeId,
                        OverTime = _Record.OverTime,
                        Remarks = _Record.Remarks,
                        Date = _Record.Date,
                        Type = Enums.Operations.U.ToString(),
                        Action = Enums.Operations.A.ToString(),
                        Approved = false,
                        CompanyId = _UserMenuPermissionAsync.CompanyId,
                        InsertDate = DateTime.Now,
                        UserNameInsert = _UserName,

                    });



                }
                _context.NightOverTimes.AddRange(_NightOverTime);
                await _context.SaveChangesAsync();

                List<CheckInOut> _CheckInOutModel = new List<CheckInOut>();

                foreach (var _Record in _NightOverTimeAddModel.NightOverTimeListAddModel.Where(x => x.OverTime > 0))
                {
                    var _EmployeeTable = await _context.Employees.Where(x => x.Id == _Record.EmployeeId).FirstOrDefaultAsync();
                    _CheckInOutModel.Add(new CheckInOut
                    {
                        MachineId = _EmployeeTable.MachineId,
                        CheckTime = _date,
                        CheckType = Enums.Operations.O.ToString(),
                        Type = Enums.Operations.N.ToString(),
                        Action = Enums.Operations.A.ToString(),
                        Approved = true,
                        Date = _Record.Date,
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