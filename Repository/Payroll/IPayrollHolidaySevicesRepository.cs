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
    public interface IPayrollHolidaySevicesRepository
    {

        Task<ApiResponse> GetHolidayLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date);
        Task<ApiResponse> AddHolidayLovAsync(HolidayAddModel _HolidayAddModel, ClaimsPrincipal _User);

    }
    public class PayrollHolidaySevicesRepository : IPayrollHolidaySevicesRepository
    {
        private readonly DataContext _context = null;
        private ErrorLog _ErrorLog = new ErrorLog();
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        public PayrollHolidaySevicesRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> GetHolidayLovAsync(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                ApiResponse apiResponse = new ApiResponse();
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                if (Convert.ToInt32(apiResponse.statusCode) == 200)
                {

                    DateTime dtSelectedDate = Convert.ToDateTime(_Date);
                    DateTime dtFirstDayOfMonth = new DateTime(dtSelectedDate.Year, dtSelectedDate.Month, 1);
                    DateTime dtLastDayOfMonth = new DateTime(dtFirstDayOfMonth.Year, dtFirstDayOfMonth.Month, DateTime.DaysInMonth(dtSelectedDate.Year, dtSelectedDate.Month));

                    var _Table = await (from _Dailydates in _context.DailyDates
                                        join _Holiday in _context.Holidays on _Dailydates.Date equals _Holiday.Date into Holiday
                                        from _Holidays in Holiday.DefaultIfEmpty()
                                        where (_Dailydates.Date >= dtFirstDayOfMonth && _Dailydates.Date <= dtLastDayOfMonth) && _Holidays.Action != Enums.Operations.D.ToString()
                                        select new HolidayViewModel
                                        {
                                            Date = _Dailydates.Date,
                                            Holidaycheck = _Holidays.HolidayCheck ? true : _Dailydates.Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() ? true : false,
                                            Remarks = string.IsNullOrEmpty(_Holidays.Remarks) ? _Dailydates.Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() ? Enums.Days.Sunday.ToString() : "" : _Holidays.Remarks,
                                            NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                                            UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                                            DeletePermission = _UserMenuPermissionAsync.Delete_Permission
                                        }).OrderByDescending(o => o.Date).ToListAsync();

                    if (_Table == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }
                    if (_Table.Count() == 0)
                    {

                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Record not found";
                        return ApiResponse;
                    }

                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    ApiResponse.data = _Table;
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
        public async Task<ApiResponse> AddHolidayLovAsync(HolidayAddModel _HolidayAddModel, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            GetUserPermissionViewModel _UserMenuPermissionAsync = new GetUserPermissionViewModel();

            string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
            try
            {

                //Permission
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_HolidayAddModel.HolidayListAddModel.FirstOrDefault().MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }

                _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }

                //Delete Pervious Record
                DateTime dtSelectedDate = Convert.ToDateTime(_HolidayAddModel.HolidayListAddModel.FirstOrDefault().Date);
                DateTime dtFirstDayOfMonth = new DateTime(dtSelectedDate.Year, dtSelectedDate.Month, 1);
                DateTime dtLastDayOfMonth = new DateTime(dtFirstDayOfMonth.Year, dtFirstDayOfMonth.Month, DateTime.DaysInMonth(dtSelectedDate.Year, dtSelectedDate.Month));

                // foreach (var _RecordDate in _HolidayAddModel.HolidayListAddModel) {
                //     var dt = Convert.ToDateTime (_RecordDate.Date);
                var _Table = _context.Holidays.Where(a => a.Date >= dtFirstDayOfMonth && a.Date <= dtLastDayOfMonth).ToList();
                if (_Table != null)
                {
                    _context.RemoveRange(_Table);
                    _context.SaveChanges();
                }
                // }
                List<Holiday> _HolidayModel = new List<Holiday>();

                foreach (var _Record in _HolidayAddModel.HolidayListAddModel)
                {

                    bool _HolidayCheck = false;
                    if (_Record.HolidayCheck.Trim() == "1") { _HolidayCheck = true; }

                    _HolidayModel.Add(new Holiday
                    {
                        Date = Convert.ToDateTime(Convert.ToDateTime(_Record.Date).ToString("yyyy-MM-dd")),
                        HolidayCheck = _HolidayCheck,
                        FactoryOverTimeCheck = false,
                        Remarks = _Record.Remarks,
                        Type = Enums.Operations.U.ToString(),
                        Action = Enums.Operations.A.ToString(),
                        CompanyId = _UserMenuPermissionAsync.CompanyId,
                        InsertDate = DateTime.Now,
                        UserNameInsert = _UserName,
                    });

                }
                _context.Holidays.AddRange(_HolidayModel);
                await _context.SaveChangesAsync();
                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = "Record Saved ";
                return apiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;
            }

        }
    }

}