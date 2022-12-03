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
    public class BLoanCategory : AbsBusiness {
        private readonly DataContext _context;
        private ErrorLog _ErrorLog = new ErrorLog ();

        String _UserName = "";

        public BLoanCategory (DataContext context) {
            _context = context;
        }

        public override async Task<ApiResponse> GetDataAsync (ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();

            try {
                var _Table = await _context.LoanCategories.Where (a => a.Action != Enums.Operations.D.ToString ()).OrderBy (o => o.Name).ToListAsync ();

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

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
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

                var _Table = await _context.LoanCategories.Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefaultAsync ();

                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.data = _Table;
                return ApiResponse;
            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
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
                var _model = (LoanCategory) model;

                string error = "";
                bool _NameExists = _context.LoanCategories.Any (rec => rec.Name.Trim ().ToLower ().Equals (_model.Name.Trim ().ToLower ()) && rec.Action != Enums.Operations.D.ToString ());

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

                await _context.LoanCategories.AddAsync (_model);
                _context.SaveChanges ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Save : " + _model.Name;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
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
                var _model = (LoanCategory) model;

                string error = "";
                bool _NameExists = _context.LoanCategories.Any (rec => rec.Name.Trim ().ToLower ().Equals (_model.Name.Trim ().ToLower ()) && rec.Id!=_model.Id && rec.Action != Enums.Operations.D.ToString ());

                if (_NameExists) {
                    error = error + "Name";
                }

                if (_NameExists) {
                    ApiResponse.statusCode = StatusCodes.Status409Conflict.ToString ();
                    ApiResponse.message = error + " Already Exist";
                    return ApiResponse;
                }

                var result = _context.LoanCategories.Where (a => a.Id == _model.Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (result == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                result.Name = _model.Name;
                result.Type = _model.Type;
                result.Active = _model.Active;
                result.Action = Enums.Operations.E.ToString ();
                result.UserNameUpdate = _UserName;
                result.UpdateDate = DateTime.Now;

                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Update : " + result.Name;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
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
                var _Table = _context.LoanCategories.Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                _context.Remove (_Table);
                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Delete : " + _Table.Name;
                return ApiResponse;

            } catch (DbUpdateException e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            } catch (Exception e) {

                string innerexp = e.InnerException == null? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString ();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString ();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
    }
}