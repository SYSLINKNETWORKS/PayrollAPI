using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TWP_API.Helpers;
using TWP_API.ViewModels;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Controllers.Payroll;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollSalaryProcessSevicesRepository
    {

        Task<ApiResponse> StaffSalaryProcess(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date, String _EmployeeId);
        Task<ApiResponse> StaffSalaryVoucherPost(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date);
        Task<ApiResponse> WorkerSalaryProcess(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date, String _EmployeeId);
        Task<ApiResponse> WorkerSalaryVoucherPost(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date);

    }
    public class PayrollSalaryProcessSevicesRepository : IPayrollSalaryProcessSevicesRepository
    {
        private readonly DataContext _context = null;
        private ErrorLog _ErrorLog = new ErrorLog();
        private SecurityHelper _SecurityHelper = new SecurityHelper();
        private SalaryStaffTable _SalaryStaffTable = null;
        private SalaryWorkerTable _SalaryWorkerTable = null;
        private MSVoucher _MSVoucher = new MSVoucher();
        public PayrollSalaryProcessSevicesRepository(DataContext context)
        {
            _context = context;
            _SalaryStaffTable = new SalaryStaffTable(_context);
            _SalaryWorkerTable = new SalaryWorkerTable(_context);
        }

        public async Task<ApiResponse> StaffSalaryProcess(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date, string _EmployeeId)
        {
            var apiResponse = new ApiResponse();

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

                apiResponse = await _SalaryStaffTable.SalaryStaffAsync(_TokenString, _Date, _EmployeeId, _User);

                return apiResponse;
            }
            return apiResponse;



        }
        public async Task<ApiResponse> StaffSalaryVoucherPost(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date)
        {

            string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
            string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
            string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();


            DateTime DateTo = new DateTime(_Date.Year, _Date.Month, DateTime.DaysInMonth(_Date.Year, _Date.Month));


            var apiResponse = new ApiResponse();

            var apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
            if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
            var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

            if (!_UserMenuPermissionAsync.View_Permission)
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                return apiResponse;
            }

            if (Convert.ToInt32(apiResponseUser.statusCode) == 200)
            {

                var _StaffSalaryTable = await _context.StaffSalaries.Where(a => a.Date == DateTo).ToListAsync();
                if (_StaffSalaryTable == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }
                var _StaffSalaryTableFilter = _StaffSalaryTable.Where(x => x.VoucherPostCk == true).FirstOrDefault();
                if (_StaffSalaryTableFilter != null)
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = "Voucher already post";
                    return apiResponse;

                }

                //Voucher
                var configuration = new ConfigurationBuilder()
                  .SetBasePath(Directory.GetCurrentDirectory())
                  .AddJsonFile("appsettings.json", false)
                  .Build();

                var _PostVoucher = configuration["AccountSettings:SalaryVoucher"];
                Guid _VoucherNo = new Guid();
                if (Convert.ToBoolean(_PostVoucher))
                {
                    double _Amount = 0, _IncomeTaxAmount = 0, _TakafulAmount = 0, _AdvanceAmount = 0, _LoanAmount = 0, _OverTimeAmount = 0, _NetAmount = 0;
                    _NetAmount = _StaffSalaryTable.Sum(a => a.GrossAmount);
                    _AdvanceAmount = _StaffSalaryTable.Sum(a => a.AdvanceAmount);
                    _LoanAmount = _StaffSalaryTable.Sum(a => a.LoanAmount);
                    _TakafulAmount = _StaffSalaryTable.Sum(a => a.Takaful);
                    _IncomeTaxAmount = _StaffSalaryTable.Sum(a => a.IncomeTaxAmount);
                    _OverTimeAmount = _StaffSalaryTable.Sum(a => a.OvertimeActualAmount);
                    _Amount = _NetAmount + _AdvanceAmount + _LoanAmount + _TakafulAmount + _IncomeTaxAmount;


                    SalaryVoucherViewModel _Voucher = new SalaryVoucherViewModel();
                    _Voucher.VoucherNo = !(_StaffSalaryTable.FirstOrDefault().VoucherNo.HasValue) ? "" : _StaffSalaryTable.FirstOrDefault().VoucherNo.Value == Guid.Empty ? "" : _StaffSalaryTable.FirstOrDefault().VoucherNo.Value.ToString();
                    _Voucher.CompanyId = _CompanyId;
                    _Voucher.UserId = _UserId;
                    _Voucher.UserName = _UserName;
                    _Voucher.Date = _StaffSalaryTable.FirstOrDefault().Date;
                    _Voucher.Amount = _Amount;
                    _Voucher.NetAmount = _NetAmount;
                    _Voucher.AdvanceAmount = _AdvanceAmount;
                    _Voucher.LoanAmount = _LoanAmount;
                    _Voucher.TakafulAmount = _TakafulAmount;
                    _Voucher.IncomeTaxAmount = _IncomeTaxAmount;
                    _Voucher.OverTimeAmount = _OverTimeAmount;
                    _Voucher.Category = Enums.Payroll.Staff.ToString();

                    var _VoucherApiResponse = await _MSVoucher.AddSalaryVoucher(_Voucher, _UserKey);
                    if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString()) { return _VoucherApiResponse; }
                    var _VoucherResponse = (VoucherResponse)_VoucherApiResponse.data;

                    if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString())
                    {
                        return _VoucherApiResponse;
                    }

                    _VoucherNo = _VoucherResponse.No;
                }
                foreach (var _StaffSalaryRecord in _StaffSalaryTable)
                {
                    _StaffSalaryRecord.VoucherPostCk = true;
                    _StaffSalaryRecord.UserNamePost = _UserName;
                    _StaffSalaryRecord.VoucherPostDate = DateTime.Now;
                    _StaffSalaryRecord.VoucherNo = _VoucherNo;
                }

                _context.StaffSalaries.UpdateRange(_StaffSalaryTable);
                await _context.SaveChangesAsync();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = "Voucher Post";
                return apiResponse;

            }
            return apiResponse;

        }
        public async Task<ApiResponse> WorkerSalaryProcess(ClaimsPrincipal _User, string _TokenString, Guid _Menuid, DateTime _Date, String _EmployeeId)
        {
            var apiResponse = new ApiResponse();

            var apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
            if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
            var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

            if (!_UserMenuPermissionAsync.View_Permission)
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                return apiResponse;
            }

            if (Convert.ToInt32(apiResponseUser.statusCode) == 200)
            {

                apiResponse = await _SalaryWorkerTable.SalaryWorkerAsync(_TokenString, _Date, _EmployeeId, _User);

                return apiResponse;
            }
            return apiResponse;

        }
        public async Task<ApiResponse> WorkerSalaryVoucherPost(ClaimsPrincipal _User, Guid _Menuid, DateTime _Date)
        {
            string _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
            string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
            string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
            string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();


            var apiResponse = new ApiResponse();

            DateTime DateTo = new DateTime(_Date.Year, _Date.Month, DateTime.DaysInMonth(_Date.Year, _Date.Month));

            var apiResponseUser = await _SecurityHelper.UserMenuPermissionAsync(_Menuid, _User);
            if (apiResponseUser.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponseUser; }
            var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponseUser.data;

            if (!_UserMenuPermissionAsync.View_Permission)
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                return apiResponse;
            }

            if (Convert.ToInt32(apiResponseUser.statusCode) == 200)
            {
                var _WorkerSalaryTable = await _context.WorkerSalaries.Where(a => a.Date == DateTo).ToListAsync();
                if (_WorkerSalaryTable == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }

                var _WorkerSalaryTableFilter = _WorkerSalaryTable.Where(x => x.VoucherPostCk == true).FirstOrDefault();
                if (_WorkerSalaryTableFilter != null)
                {
                    apiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    apiResponse.message = "Voucher already post";
                    return apiResponse;

                }

                //Voucher
                var configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile("appsettings.json", false)
                               .Build();
                var _PostVoucher = configuration["AccountSettings:SalaryVoucher"];
                Guid _VoucherNo = new Guid();
                if (Convert.ToBoolean(_PostVoucher))
                {

                    double _Amount = 0, _IncomeTaxAmount = 0, _TakafulAmount = 0, _AdvanceAmount = 0, _LoanAmount = 0, _OverTimeAmount = 0, _NetAmount = 0;
                    _NetAmount = _WorkerSalaryTable.Sum(a => a.GrossAmount);
                    _AdvanceAmount = _WorkerSalaryTable.Sum(a => a.AdvanceAmount);
                    _LoanAmount = _WorkerSalaryTable.Sum(a => a.LoanAmount);
                    _TakafulAmount = _WorkerSalaryTable.Sum(a => a.Takaful);
                    _IncomeTaxAmount = _WorkerSalaryTable.Sum(a => a.IncomeTaxAmount);
                    _OverTimeAmount = _WorkerSalaryTable.Sum(a => a.OvertimeActualAmount);
                    _Amount = _NetAmount + _AdvanceAmount + _LoanAmount + _TakafulAmount + _IncomeTaxAmount;


                    SalaryVoucherViewModel _Voucher = new SalaryVoucherViewModel();
                    _Voucher.VoucherNo =  !(_WorkerSalaryTable.FirstOrDefault().VoucherNo.HasValue) ? "" :_WorkerSalaryTable.FirstOrDefault().VoucherNo.Value == Guid.Empty ? "" : _WorkerSalaryTable.FirstOrDefault().VoucherNo.Value.ToString();
                    _Voucher.CompanyId = _CompanyId;
                    _Voucher.UserId = _UserId;
                    _Voucher.UserName = _UserName;
                    _Voucher.Date = _WorkerSalaryTable.FirstOrDefault().Date;
                    _Voucher.Amount = _Amount;
                    _Voucher.NetAmount = _NetAmount;
                    _Voucher.AdvanceAmount = _AdvanceAmount;
                    _Voucher.LoanAmount = _LoanAmount;
                    _Voucher.TakafulAmount = _TakafulAmount;
                    _Voucher.IncomeTaxAmount = _IncomeTaxAmount;
                    _Voucher.OverTimeAmount = _OverTimeAmount;

                    _Voucher.Category = Enums.Payroll.Worker.ToString();

                    var _VoucherApiResponse = await _MSVoucher.AddSalaryVoucher(_Voucher, _UserKey);
                    if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString()) { return _VoucherApiResponse; }
                    var _VoucherResponse = (VoucherResponse)_VoucherApiResponse.data;

                    if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString())
                    {
                        return _VoucherApiResponse;
                    }

                    _VoucherNo = _VoucherResponse.No;
                }
                foreach (var _WorkerSalaryRecord in _WorkerSalaryTable)
                {
                    _WorkerSalaryRecord.VoucherPostCk = true;
                    _WorkerSalaryRecord.UserNamePost = _UserName;
                    _WorkerSalaryRecord.VoucherPostDate = DateTime.Now;
                    _WorkerSalaryRecord.VoucherNo = _VoucherNo;
                }
                _context.WorkerSalaries.UpdateRange(_WorkerSalaryTable);
                await _context.SaveChangesAsync();

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = "Record Update";
                return apiResponse;
            }
            return apiResponse;

        }
    }

}