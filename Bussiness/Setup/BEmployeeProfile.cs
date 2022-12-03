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

namespace TWP_API_Payroll.Bussiness
{
    public class BEmployeeProfile : AbsBusiness
    {
        private readonly DataContext _context;
        private ErrorLog _ErrorLog = new ErrorLog();
        SecurityHelper _SecurityHelper = new SecurityHelper();

        String _UserName = "";

        public BEmployeeProfile(DataContext context)
        {
            _context = context;
        }

        public override async Task<ApiResponse> GetDataAsync(ClaimsPrincipal _User)
        {
            var ApiResponse = new ApiResponse();
            _UserName = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserName.ToString())?.Value.ToString();

            try
            {
                var _Table = await _context.Employees.Include(des => des.designation).Include(dep => dep.department).Where(a => a.Action != Enums.Operations.D.ToString()).OrderBy(o => o.Name).ToListAsync();

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
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
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

                var _Table = await _context.Employees.Include(x => x.employeeCategory).Include(x => x.designation).Include(x => x.department).Include(x => x.roster).Include(x => x.annualLeaves).Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefaultAsync();

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
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
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
                var _model = (Employee)model;

                string error = "";
                bool _NameExists = _context.Employees.Any(rec => rec.MachineId.Equals(_model.MachineId) && rec.Action != Enums.Operations.D.ToString());

                if (_NameExists)
                {
                    error = error + "Card # already registered";
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

                await _context.Employees.AddAsync(_model);
                _context.SaveChanges();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = _model.Id;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
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
                var _model = (Employee)model;



                var result = _context.Employees.Where(a => a.Id == _model.Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (result == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                result.Name = _model.Name;

                result.FatherName = _model.FatherName;
                result.MachineId = _model.MachineId;
                result.DateofParmanent = _model.DateofParmanent;
                result.ProvisionPeriod = _model.ProvisionPeriod;
                result.DesignationId = _model.DesignationId;
                result.DepartmentId = _model.DepartmentId;
                result.EmployeeCategoryId = _model.EmployeeCategoryId;
                result.DateofBirth = _model.DateofBirth;
                result.Address = _model.Address;
                result.AddressPermanent = _model.AddressPermanent;
                result.Gender = _model.Gender;
                result.Married = _model.Married;
                result.RosterId = _model.RosterId;
                result.OfficeWorker = _model.OfficeWorker;
                result.BranchId = _model.BranchId;
                result.EmergencyContactOne = _model.EmergencyContactOne;
                result.EmergencyContactTwo = _model.EmergencyContactTwo;
                result.CNIC = _model.CNIC;
                result.CNICExpire = _model.CNICExpire;
                result.Mobile = _model.Mobile;
                result.NTN = _model.NTN;
                result.Email = _model.Email;
                result.Remarks = _model.Remarks;
                result.ReferenceOne = _model.ReferenceOne;
                result.ReferenceTwo = _model.ReferenceTwo;
                result.ReferenceCNICOne = _model.ReferenceCNICOne;
                result.ReferenceCNICTwo = _model.ReferenceCNICTwo;
                result.ReferenceAddressOne = _model.ReferenceAddressOne;
                result.ReferenceAddressTwo = _model.ReferenceAddressTwo;
                result.ReferenceContactOne = _model.ReferenceContactOne;
                result.ReferenceContactTwo = _model.ReferenceContactTwo;
                result.DateofJoin = _model.DateofJoin;
                result.AnnualLeavesId = _model.AnnualLeavesId;
                result.ModeOfPayment = _model.ModeOfPayment;
                result.SalaryAccount = _model.SalaryAccount;
                result.Gratuity = _model.Gratuity;
                result.AttendanceAllowance = _model.AttendanceAllowance;
                result.OverTimeHoliday = _model.OverTimeHoliday;
                result.TakafulRate = _model.TakafulRate;
                result.OverTime = _model.OverTime;
                result.OverTimeRate = _model.OverTimeRate;
                result.OverTimeFactory = _model.OverTimeFactory;
                result.IncomeTax = _model.IncomeTax;
                result.AttendanceExempt = _model.AttendanceExempt;
                result.OvertimeSaturday = _model.OvertimeSaturday;
                result.SESSI = _model.SESSI;
                result.SESSIRegistrationDate = _model.SESSIRegistrationDate;
                result.SESSIRegistrationNo = _model.SESSIRegistrationNo;
                result.EOBI = _model.EOBI;
                result.EOBIRegistrationDate = _model.EOBIRegistrationDate;
                result.EOBIRegistrationNo = _model.EOBIRegistrationNo;
                result.StopPayment = _model.StopPayment;
                result.LateDeduction = _model.LateDeduction;
                result.ResignationCheck = _model.ResignationCheck;
                result.ResignationDate = _model.ResignationDate;
                result.ResignationRemarks = _model.ResignationRemarks;
                result.QualificationInstitute = _model.QualificationInstitute;
                result.Qualification = _model.Qualification;
                result.QualificationYear = _model.QualificationYear;
                result.QualificationRemarks = _model.QualificationRemarks;
                result.CompanyExpirence = _model.CompanyExpirence;
                result.CompanyExpirenceDescription = _model.CompanyExpirenceDescription;
                result.CompanyExpirenceFrom = _model.CompanyExpirenceFrom;
                result.CompanyExpirenceTo = _model.CompanyExpirenceTo;
                result.CompanyExpirenceRemarks = _model.CompanyExpirenceRemarks;
                result.DocumentAuthorize = _model.DocumentAuthorize;
                result.ReportOfficerId=_model.ReportOfficerId;

                result.Type = _model.Type;
                result.Active = _model.Active;
                result.UserNameUpdate = _UserName;
                result.Action = Enums.Operations.E.ToString();

                await _context.SaveChangesAsync();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = result.Id;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
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
                var _Table = _context.Employees.Where(a => a.Id == _Id && a.Action != Enums.Operations.D.ToString()).FirstOrDefault();
                if (_Table == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found ";
                    return ApiResponse;
                }

                _context.Remove(_Table);
                await _context.SaveChangesAsync();

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Record Delete : " + _Table.Name;
                return ApiResponse;

            }
            catch (DbUpdateException e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;

            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace,_User);
                ApiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                ApiResponse.message = _ErrorId;
                return ApiResponse;
            }
        }
    }
}