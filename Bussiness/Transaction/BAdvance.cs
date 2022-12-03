using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TWP_API.Helpers;
using TWP_API.ViewModels;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;

namespace TWP_API_Payroll.Bussiness
{
    public class BAdvance : AbsBusiness
    {
        private readonly DataContext _context;
        private ErrorLog _ErrorLog = new ErrorLog();

        String _UserName = "";

        private MSVoucher _MSVoucher = new MSVoucher();

        private NotificationService _NotificationService = new NotificationService();
        public BAdvance(DataContext context)
        {
            _context = context;
        }

        public override async Task<ApiResponse> GetDataAsync(ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

            try
            {
                //.OrderBy (o => o.Date).ThenBy (t => t.Employee.Name)
                var _Table = await _context.Advances.Include(emp => emp.Employee).Where(a => a.Action != Enums.Operations.D.ToString()).ToListAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_Table.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Table;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> GetDataByIdAsync(Guid _Id, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

            try
            {

                var _Table = await _context.Advances.Include(emp => emp.Employee).Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefaultAsync();

                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Table;
                return ApiResponse;
            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> AddAsync(object model, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();

                var _model = (Advance)model;

                string error = "";
                bool _NameExists = _context.Advances.Any(rec => rec.Date.Equals(_model.Date) && rec.EmployeeId.Equals(_model.EmployeeId) && rec.Action != Enums.Operations.D.ToString());

                if (_NameExists)
                {
                    error = error + "Name";
                }

                if (_NameExists)
                {
                    ApiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                    ApiResponse.message = error + " Already Exist";
                    return ApiResponse;
                }

                _model.UserNameInsert = _UserName;
                _model.InsertDate = DateTime.Now;
                _model.Action = Enums.Operations.A.ToString();

                var _EmployeeTable = await _context.Employees.Where(x => x.Id == _model.EmployeeId).FirstOrDefaultAsync();
                string _EmployeeName = _EmployeeTable.Name.Trim() + " " + _EmployeeTable.FatherName.Trim();

                string _Narration = "Paid to " + _EmployeeName + " against salary dated  " + _model.Date.ToString("MMM-yyyy");

                // //Voucher
                // AdvanceVoucherViewModel _Voucher = new AdvanceVoucherViewModel();
                // _Voucher.CompanyId = _CompanyId;
                // _Voucher.UserId = _UserId;
                // _Voucher.UserName = _UserName;
                // _Voucher.Date = _model.Date;
                // _Voucher.EmployeeName = _EmployeeName;
                // _Voucher.Remarks = "";
                // _Voucher.Narration = _Narration;

                // _Voucher.AdvanceId = _model.Id;

                // _Voucher.Amount = _model.Amount;

                // var _VoucherApiResponse = await _MSVoucher.AddAdvanceVoucher(_Voucher, _UserKey);
                // if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString()) { return _VoucherApiResponse; }
                // var _VoucherResponse = (VoucherResponse)_VoucherApiResponse.data;

                // if (_VoucherApiResponse.statusCode == StatusCodes.Status200OK.ToString())
                // {
                //     _model.VoucherNo = _VoucherResponse.No;
                //     await _context.Advances.AddAsync(_model);
                //     _context.SaveChanges();
                // }

                await _context.Advances.AddAsync(_model);
                _context.SaveChanges();




                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Record Save : " + _EmployeeName;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> UpdateAsync(object model, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();

                var _model = (Advance)model;

                var _EmployeeTable = await _context.Employees.Where(x => x.Id == _model.EmployeeId).FirstOrDefaultAsync();
                string _EmployeeName = _EmployeeTable.Name.Trim() + " " + _EmployeeTable.FatherName.Trim();
                // //Voucher
                // string _Narration = "Paid to "+_EmployeeName+" against salary dated  " + _model.Date.ToString("MMM-yyyy");
                // AdvanceVoucherViewModel _Voucher = new AdvanceVoucherViewModel();
                // _Voucher.VoucherNo = _model.VoucherNo;
                // _Voucher.CompanyId = _CompanyId;
                // _Voucher.UserId = _UserId;
                // _Voucher.UserName = _UserName;
                // _Voucher.Date = _model.Date;
                // _Voucher.Remarks = "";
                // _Voucher.EmployeeName = _EmployeeName;
                // _Voucher.Narration = _Narration;

                // _Voucher.AdvanceId = _model.Id;

                // _Voucher.Amount = _model.Amount;

                // var _VoucherApiResponse = await _MSVoucher.UpdateAdvanceVoucher(_Voucher, _UserKey);
                // if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString()) { return _VoucherApiResponse; }
                // var _VoucherResponse = (VoucherResponse)_VoucherApiResponse.data;
                // if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString())
                // {
                //     return _VoucherApiResponse;
                // }

                _context.Update(_model);
                await _context.SaveChangesAsync();



                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Record Update :  " + _EmployeeName;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> DeleteAsync(Guid _Id, ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();
                string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();
                string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();

                var _Table = _context.Advances.Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }


                var _EmployeeTable = await _context.Employees.Where(x => x.Id == _Table.EmployeeId).FirstOrDefaultAsync();
                var _EmployeeName = _EmployeeTable.Name.Trim() + " " + _EmployeeTable.FatherName.Trim();


                // var _VoucherApiResponse = await _MSVoucher.DeleteAdvanceVoucher(_Table.VoucherNo, _CompanyId, _UserId, _UserName, _UserKey);

                // if (_VoucherApiResponse.statusCode != StatusCodes.Status200OK.ToString())
                // {
                //     return _VoucherApiResponse;
                // }
                _Table.Action = Enums.Operations.D.ToString();
                _Table.UserNameDelete = _UserName;
                _Table.DeleteDate = DateTime.Now;

                _context.Update(_Table);
                await _context.SaveChangesAsync();
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Record Delete :  " + _EmployeeName;

                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
    }
}