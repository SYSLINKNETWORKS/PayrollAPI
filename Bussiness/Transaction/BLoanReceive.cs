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
    public class BLoanReceive : AbsBusiness {
        private readonly DataContext _context;
        private ErrorLog _ErrorLog = new ErrorLog ();

        String _UserName = "";

        public BLoanReceive (DataContext context) {
            _context = context;
        }

        public override async Task<ApiResponse> GetDataAsync (ClaimsPrincipal _User) {
            var ApiResponse = new ApiResponse ();
            _UserName = _User.Claims.FirstOrDefault (c => c.Type == Enums.Misc.UserName.ToString ())?.Value.ToString ();

            try {
                var _Table = await _context.LoanReceives.Include (li => li.LoanIssue).Include (cat => cat.LoanIssue.LoanCategory).Include (emp => emp.LoanIssue.Employee).Where (a => a.Action != Enums.Operations.D.ToString () && a.Type==Enums.Operations.U.ToString() ).OrderBy (o => o.Date).ToListAsync ();

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

                var _Table = await _context.LoanReceives.Include (li => li.LoanIssue).Include (emp => emp.LoanIssue.Employee).Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefaultAsync ();

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
                var _model = (LoanReceive) model;

               
                _model.Type = Enums.Operations.U.ToString ();
                _model.UserNameInsert = _UserName;
                _model.InsertDate = DateTime.Now;
                _model.Action = Enums.Operations.A.ToString ();

                await _context.LoanReceives.AddAsync (_model);
                _context.SaveChanges ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Save : " + _model.Date;
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
                var _model = (LoanReceive) model;


                var result = _context.LoanReceives.Include (li => li.LoanIssue).Include (x => x.LoanIssue.Employee).Where (a => a.Id == _model.Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (result == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                result.Date = _model.Date;
                result.Amount = _model.Amount;
                result.CheaqueCash = _model.CheaqueCash;
                result.Type = Enums.Operations.U.ToString();
                //result.Active = _model.Active;
                result.UserNameUpdate = _UserName;
                result.Action = Enums.Operations.E.ToString ();
                result.UserNameUpdate = _UserName;
                result.DeleteDate = DateTime.Now;

                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Update : " + result.Date + " Employee Name " + result.LoanIssue.Employee.Name;
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
                var _Table = _context.LoanReceives.Include (x => x.LoanIssue.Employee).Where (a => a.Id == _Id && a.Action != Enums.Operations.D.ToString ()).FirstOrDefault ();
                if (_Table == null) {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString ();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                _context.Remove (_Table);
                await _context.SaveChangesAsync ();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString ();
                ApiResponse.message = "Record Delete : " + _Table.Date + "Employee Name" + _Table.LoanIssue.Employee.Name;
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