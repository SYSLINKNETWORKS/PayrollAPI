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

namespace TWP_API_Payroll.Bussiness {
    public class BSalaryAdditionDeduction : AbsBusiness {
        private readonly DataContext _context;
        private ErrorLog _ErrorLog = new ErrorLog ();

        String _UserName = "";

        public BSalaryAdditionDeduction (DataContext context) {
            _context = context;
        }

        public override async Task<ApiResponse> GetDataAsync (ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();

            try {
                var _Table = await _context.SalaryAdditionDeductions.Include (emp => emp.Employee).Where (a => a.Action != Enums.Operations.D.ToString ()).OrderBy (o => o.Id).ToListAsync ();

                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }

                if (_Table.Count == 0) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();;
                    ApiResponse.message = "Record Not Found";
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.data = _Table;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> GetDataByIdAsync (Guid _Id, ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();

            try {

                var _Table = await _context.SalaryAdditionDeductions.Include (emp => emp.Employee).Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefaultAsync ();

                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.data = _Table;
                return ApiResponse;
            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> AddAsync (object model, ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            try {

                _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();
                var _model = (SalaryAdditionDeduction) model;

                string error = "";
                bool _NameExists = _context.SalaryAdditionDeductions.Any (rec => rec.Date.Equals (_model.Date) && rec.EmployeeId == _model.EmployeeId && rec.Action != Enums.Operations.D.ToString ());

                if (_NameExists) {
                    error = error + "Name";
                }

                if (_NameExists) {
                    ApiResponse.statusCode = StatusCodes.Status409Conflict.ToString ();
                    ApiResponse.message = error + " Already Exist";
                    return ApiResponse;
                }

                _model.UserNameInsert = _UserName;
                _model.InsertDate = DateTime.Now;
                _model.Action = Enums.Operations.A.ToString ();

                await _context.SalaryAdditionDeductions.AddAsync (_model);
                _context.SaveChanges ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Save : " + _model.Date;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> UpdateAsync (object model, ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            try {
                _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();
                var _model = (SalaryAdditionDeduction) model;

                string error = "";
                bool _NameExists = _context.SalaryAdditionDeductions.Any (rec => rec.Date.Equals (_model.Date) && rec.EmployeeId == _model.EmployeeId && rec.Id != _model.Id && rec.Action != Enums.Operations.D.ToString ());

                if (_NameExists) {
                    error = error + "Record ";
                }

                if (_NameExists) {
                    ApiResponse.statusCode = StatusCodes.Status409Conflict.ToString ();
                    ApiResponse.message = error + " Already Exist";
                    return ApiResponse;
                }

                var result = _context.SalaryAdditionDeductions.Where (a => a.Id == _model.Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (result == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                result.UserNameUpdate = _UserName;
                result.Action = Enums.Operations.E.ToString ();
                result.Type = Enums.Operations.U.ToString ();
                result.Date = _model.Date;
                result.EmployeeId = _model.EmployeeId;
                result.AdditionAmount = _model.AdditionAmount;
                result.DeductionAmount = _model.DeductionAmount;
                result.UserNameUpdate = _UserName;
                result.DeleteDate = DateTime.Now;

                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Update : " + result.Date;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
        public override async Task<ApiResponse> DeleteAsync (Guid _Id, ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            try {

                _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();
                var _Table = _context.SalaryAdditionDeductions.Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                _context.Remove (_Table);
                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Delete : " + _Table.Date;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
    }
}