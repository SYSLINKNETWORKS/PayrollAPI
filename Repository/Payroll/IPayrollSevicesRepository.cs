using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

namespace TWP_API_Payroll.Repository
{
    public interface IPayrollSevicesRepository
    {

        Task<ApiResponse> GetDesignationLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetDepartmentLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetRosterLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetEmployeeLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetSalesmanLovAsync(ClaimsPrincipal _User, string _Search);

        Task<ApiResponse> GetAttendanceMachineCategoryLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetAttendanceMachineGroupLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetEmployeeCategoryLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetAnnualLeaveLovAsync(ClaimsPrincipal _User, string _Search);

        Task<ApiResponse> GetInOutCategoryLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetLoanCategoryLovAsync(ClaimsPrincipal _User, string _Search);
        Task<ApiResponse> GetPerviousSalaryLovAsync(ClaimsPrincipal _User, DateTime _Date, Guid _EmployeeId);
        Task<ApiResponse> GetRosterByEmployeeIdLovAsync(ClaimsPrincipal _User, Guid _EmployeeId, DateTime _DateFrom, DateTime _DateTo, bool _Tag);
        Task<ApiResponse> GetLoanforReceivingLovAsync(ClaimsPrincipal _User, string _Search);

        Task<ApiResponse> GetReadImageToBytesAsync();

    }
    public class PayrollSevicesRepository : IPayrollSevicesRepository
    {
        private readonly DataContext _context = null;

        CompressImage _CompressImage = new CompressImage();
        SecurityHelper _SecurityHelper = new SecurityHelper();

        //        DateTime _date = Convert.ToDateTime (DateTime.Now.ToString ("dd-MMM-yyyy"));
        public PayrollSevicesRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> GetDesignationLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _Designations = await (from Designations in _context.Designations.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                           select new LovServicesViewModel
                                           {
                                               Id = Designations.Id,
                                               Name = Designations.Name
                                           }).OrderBy(o => o.Name).ToListAsync();

                if (_Designations == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_Designations.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Designations;
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

        public async Task<ApiResponse> GetDepartmentLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _Departments = await (from Departments in _context.Departments.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                          select new LovServicesViewModel
                                          {
                                              Id = Departments.Id,
                                              Name = Departments.Name
                                          }).OrderBy(o => o.Name).ToListAsync();

                if (_Departments == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_Departments.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Departments;
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

        public async Task<ApiResponse> GetEmployeeLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CfEmployees = await (from CfEmployees in _context.Employees.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && a.Approved == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search))).OrderBy(o => o.Name)
                                          join _Department in _context.Departments on CfEmployees.DepartmentId equals _Department.Id
                                          join _Designation in _context.Designations on CfEmployees.DesignationId equals _Designation.Id
                                          select new LovServicesViewModel
                                          {
                                              Id = CfEmployees.Id,
                                              Name = "[" + CfEmployees.MachineId.ToString() + "]-" + CfEmployees.Name.ToString() + " " + CfEmployees.FatherName.ToString() + " " + _Department.Name.ToString() + " " + _Designation.Name.ToString()
                                          }).ToListAsync();

                if (_CfEmployees == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CfEmployees.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CfEmployees;
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

        public async Task<ApiResponse> GetInOutCategoryLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CfInOutCategories = await (from CfInOutCategories in _context.InOutCategories.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                                select new LovServicesViewModel
                                                {
                                                    Id = CfInOutCategories.Id,
                                                    Name = CfInOutCategories.Name
                                                }).ToListAsync();

                if (_CfInOutCategories == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CfInOutCategories.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CfInOutCategories;
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

        public async Task<ApiResponse> GetAttendanceMachineCategoryLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CfAttendanceMachineCategories = await (from CfAttendanceMachineCategories in _context.AttendanceMachineCategories.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                                            select new LovServicesViewModel
                                                            {
                                                                Id = CfAttendanceMachineCategories.Id,
                                                                Name = CfAttendanceMachineCategories.Name
                                                            }).ToListAsync();

                if (_CfAttendanceMachineCategories == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CfAttendanceMachineCategories.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CfAttendanceMachineCategories;
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
        public async Task<ApiResponse> GetAttendanceMachineGroupLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CfAttendanceMachineCategories = await (from CfAttendanceMachineGroups in _context.AttendanceMachineGroups.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                                            select new LovServicesViewModel
                                                            {
                                                                Id = CfAttendanceMachineGroups.Id,
                                                                Name = CfAttendanceMachineGroups.Name
                                                            }).ToListAsync();

                if (_CfAttendanceMachineCategories == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_CfAttendanceMachineCategories.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CfAttendanceMachineCategories;
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

        public async Task<ApiResponse> GetEmployeeCategoryLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _CfEmployeeCategories = await (from CfEmployeeCategories in _context.EmployeeCategories.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                                   select new LovServicesViewModel
                                                   {
                                                       Id = CfEmployeeCategories.Id,
                                                       Name = CfEmployeeCategories.Name
                                                   }).ToListAsync();

                if (_CfEmployeeCategories == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                if (_CfEmployeeCategories.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _CfEmployeeCategories;
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

        public async Task<ApiResponse> GetRosterLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _Rosters = await (from Rosters in _context.Rosters.Where(a => a.Action != Enums.Operations.D.ToString() && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                      select new LovServicesViewModel
                                      {
                                          Id = Rosters.Id,
                                          Name = Rosters.Name
                                      }).OrderBy(o => o.Name).ToListAsync();

                if (_Rosters == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                if (_Rosters.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Rosters;
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

        public async Task<ApiResponse> GetAnnualLeaveLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _AnnualLeaves = await (from AnnualLeaves in _context.AnnualLeaves.Where(a => a.Action != Enums.Operations.D.ToString() && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                           select new LovServicesViewModel
                                           {
                                               Id = AnnualLeaves.Id,
                                               Name = AnnualLeaves.Name
                                           }).OrderBy(o => o.Name).ToListAsync();

                if (_AnnualLeaves == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                if (_AnnualLeaves.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _AnnualLeaves;
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

        public async Task<ApiResponse> GetLoanCategoryLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _LoanCategory = await (from LoanCategory in _context.LoanCategories.Where(a => a.Action != Enums.Operations.D.ToString() && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search)))
                                           select new LovServicesViewModel
                                           {
                                               Id = LoanCategory.Id,
                                               Name = LoanCategory.Name
                                           }).OrderBy(o => o.Name).ToListAsync();

                if (_LoanCategory == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                if (_LoanCategory.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _LoanCategory;
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

        public async Task<ApiResponse> GetPerviousSalaryLovAsync(ClaimsPrincipal _User, DateTime _Date, Guid _EmployeeId)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _Salary = await _context.Salaries.Where(a => a.Action != Enums.Operations.D.ToString() && a.Date <= _Date && a.EmployeeId == _EmployeeId).OrderByDescending(d => d.Date).FirstOrDefaultAsync();
                if (_Salary == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                    ApiResponse.data = 0;
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _Salary.CurrentAmount;
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
        public async Task<ApiResponse> GetRosterByEmployeeIdLovAsync(ClaimsPrincipal _User, Guid _EmployeeId, DateTime _DateFrom, DateTime _DateTo, bool _Tag)
        {
            var ApiResponse = new ApiResponse();
            try
            {
                List<RosterEmployeeByIdModel> _RosterEmployeeByIdModel = new List<RosterEmployeeByIdModel>();

                int _DateDiff = (_DateTo - _DateFrom).Days + 1;
                for (int _DayCount = 0; _DayCount < _DateDiff; _DayCount++)
                {
                    DateTime _Date = _DateFrom.AddDays(_DayCount);
                    if (!_Tag)
                    {
                        var _CheckInOutApproved = await (from _CheckInOuts in _context.CheckInOuts.Where(x => x.Date >= _DateFrom && x.Date <= _DateTo && x.Approved == true && x.Type == Enums.Operations.U.ToString() && x.Action != Enums.Operations.D.ToString()) select new { Date = _CheckInOuts.Date }).Distinct().ToListAsync();
                        if (_CheckInOutApproved.Count() > 0)
                        {
                            string _ApprovedDatesStr = "";
                            foreach (var _ApprovedDates in _CheckInOutApproved)
                            {
                                _ApprovedDatesStr += "<br/>" + _ApprovedDates.Date.ToString("dd-MMM-yyyy");
                            }
                            ApiResponse.statusCode = StatusCodes.Status409Conflict.ToString();
                            ApiResponse.message = "Already approved " + _ApprovedDatesStr;
                            return ApiResponse;
                        }
                    }
                    var _EmployeeTable = await (_context.Employees).Where(rg => rg.Active == true && rg.Approved == true && rg.Action != Enums.Operations.D.ToString() && (_EmployeeId != Guid.Empty ? rg.Id == _EmployeeId : true)).ToListAsync();
                    if (_EmployeeTable == null)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Invalid employee";
                        return ApiResponse;
                    }
                    if (_EmployeeTable.Count() == 0)
                    {
                        ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                        ApiResponse.message = "Invalid employee";
                        return ApiResponse;
                    }
                    foreach (var _EmployeeRecord in _EmployeeTable)
                    {

                        var _RosterGroups = await (_context.RosterGroups).Where(rg => rg.RosterId == _EmployeeRecord.RosterId && rg.Date <= _Date).OrderByDescending(rgo => rgo.Date).FirstOrDefaultAsync();
                        if (_RosterGroups == null)
                        {
                            ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                            ApiResponse.message = "Invalid roster of employee : " + _EmployeeRecord.Name + " " + _EmployeeRecord.FatherName;
                            return ApiResponse;
                        }
                        var _CheckInOutTableInn = await _context.CheckInOuts.Where(i => i.MachineId == _EmployeeRecord.MachineId && i.Date == _Date && i.CheckType == Enums.Payroll.I.ToString()).FirstOrDefaultAsync();
                        var _CheckInOutTableOut = await _context.CheckInOuts.Where(i => i.MachineId == _EmployeeRecord.MachineId && i.Date == _Date && i.CheckType == Enums.Payroll.O.ToString()).FirstOrDefaultAsync();
                        _RosterEmployeeByIdModel.Add(new RosterEmployeeByIdModel
                        {
                            RosterId = _RosterGroups.RosterId,
                            EmployeeId = _EmployeeRecord.Id,
                            EmployeeName = _EmployeeRecord.Name + " " + _EmployeeRecord.FatherName,
                            Date = _Date,
                            RosterInn = _CheckInOutTableInn != null ? _CheckInOutTableInn.CheckTime : _Date.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroups.MondayCheck ? _RosterGroups.MondayInn : _Date.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroups.TuesdayCheck ? _RosterGroups.TuesdayInn : _Date.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroups.WednesdayCheck ? _RosterGroups.WednesdayInn : _Date.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroups.ThursdayCheck ? _RosterGroups.ThursdayInn : _Date.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroups.FridayCheck ? _RosterGroups.FridayInn : _Date.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroups.SaturdayCheck ? _RosterGroups.SaturdayInn : _Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroups.SundayCheck ? _RosterGroups.SundayInn : null,
                            RosterOut = _CheckInOutTableOut != null ? _CheckInOutTableOut.CheckTime : _Date.DayOfWeek.ToString() == Enums.Days.Monday.ToString() && _RosterGroups.MondayCheck ? _RosterGroups.MondayOut : _Date.DayOfWeek.ToString() == Enums.Days.Tuesday.ToString() && _RosterGroups.TuesdayCheck ? _RosterGroups.TuesdayOut : _Date.DayOfWeek.ToString() == Enums.Days.Wednesday.ToString() && _RosterGroups.WednesdayCheck ? _RosterGroups.WednesdayOut : _Date.DayOfWeek.ToString() == Enums.Days.Thursday.ToString() && _RosterGroups.ThursdayCheck ? _RosterGroups.ThursdayOut : _Date.DayOfWeek.ToString() == Enums.Days.Friday.ToString() && _RosterGroups.FridayCheck ? _RosterGroups.FridayOut : _Date.DayOfWeek.ToString() == Enums.Days.Saturday.ToString() && _RosterGroups.SaturdayCheck ? _RosterGroups.SaturdayOut : _Date.DayOfWeek.ToString() == Enums.Days.Sunday.ToString() && _RosterGroups.SundayCheck ? _RosterGroups.SundayOut : null
                        });
                    }
                }
                if (_RosterEmployeeByIdModel.Count() == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = _DateDiff; // "Roster inn out time not found";
                    return ApiResponse;

                }
                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _RosterEmployeeByIdModel;
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

        public async Task<ApiResponse> GetLoanforReceivingLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var ApiResponse = new ApiResponse();
            try
            {

                var _LoanTable = await (from _LoanIssue in _context.LoanIssues
                                        join _Employee in _context.Employees on _LoanIssue.EmployeeId equals _Employee.Id
                                        join _LoanCategories in _context.LoanCategories on _LoanIssue.LoanCategoryId equals _LoanCategories.Id
                                        where _LoanIssue.Action != Enums.Operations.D.ToString()
                                        select new LovServicesLoanReceivedViewModel
                                        {
                                            LoanId = _LoanIssue.Id,
                                            EmployeeName = _Employee.Name + " " + _Employee.FatherName,
                                            LoanCategoryName = _LoanCategories.Name,
                                            Amount = _LoanIssue.Amount,
                                            InstallmentAmount = _LoanIssue.InstalmentAmount,
                                            BalanceAmount = _LoanIssue.Amount,
                                            Status = false
                                        }).ToListAsync();

                foreach (var _LoanTableRecord in _LoanTable)
                {
                    var _LoanReceived = await _context.LoanReceives.Where(lr => lr.Action != Enums.Operations.D.ToString() && lr.LoanIssueId == _LoanTableRecord.LoanId).SumAsync(s => s.Amount);
                    if (_LoanReceived > 0)
                    {
                        _LoanTableRecord.ReceivedAmount = _LoanReceived;
                        _LoanTableRecord.BalanceAmount = _LoanTableRecord.Amount - _LoanReceived;
                        _LoanTableRecord.Status = _LoanTableRecord.Amount - _LoanReceived > 0 ? false : true;
                    }
                }

                var _LoanReceivedTable = (from _LoanTable1 in _LoanTable
                                          where _LoanTable1.Status == false && (string.IsNullOrEmpty(_Search) ? true : _LoanTable1.EmployeeName.Contains(_Search))
                                          select new LovServicesViewModel
                                          {
                                              Id = _LoanTable1.LoanId,
                                              Name = _LoanTable1.EmployeeName + " Balance : " + _LoanTable1.BalanceAmount.ToString() + " Installment : " + _LoanTable1.InstallmentAmount.ToString(),
                                          }).OrderBy(o => o.Name).ToList();

                if (_LoanReceivedTable == null)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }
                if (_LoanReceivedTable.Count == 0)
                {
                    _LoanReceivedTable = (from _LoanTable1 in _LoanTable
                                          where _LoanTable1.Status == false
                                          select new LovServicesViewModel
                                          {
                                              Id = _LoanTable1.LoanId,
                                              Name = _LoanTable1.EmployeeName + " Balance : " + _LoanTable1.BalanceAmount.ToString() + " Installment : " + _LoanTable1.InstallmentAmount.ToString(),
                                          }).OrderBy(o => o.Name).ToList();
                }
                if (_LoanReceivedTable.Count == 0)
                {
                    ApiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    ApiResponse.message = "Record not found";
                    return ApiResponse;
                }

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.data = _LoanReceivedTable;
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
        public async Task<ApiResponse> GetReadImageToBytesAsync()
        {
            var ApiResponse = new ApiResponse();
            try
            {
                //Profile Image Start
                string ProfileImage = Directory.GetCurrentDirectory() + "\\Images\\employee";
                DirectoryInfo _ProfileImageDirectory = new DirectoryInfo(ProfileImage);
                FileInfo[] _ProfileImageFiles = _ProfileImageDirectory.GetFiles("*.*");

                List<EmployeeImage> _EmployeeImage = new List<EmployeeImage>();

                foreach (var _ProfileImageFilesRecord in _ProfileImageFiles)
                {
                    var _FileNameProfile = Path.GetFileNameWithoutExtension(_ProfileImageFilesRecord.Name);
                    var _EmployeeTableProfile = await _context.Employees.Where(x => x.MachineId == Convert.ToInt32(_FileNameProfile)).FirstOrDefaultAsync();
                    if (_EmployeeTableProfile != null)
                    {

                        string base64StringProfile = "";
                        byte[] imageBytesProfile = null;
                        using (Image image = Image.FromFile(_ProfileImageFilesRecord.FullName))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                imageBytesProfile = m.ToArray();

                            }
                        };
                        imageBytesProfile = _CompressImage.GetCompressImage(imageBytesProfile);
                        base64StringProfile = Convert.ToBase64String(imageBytesProfile);
                        //                        File.WriteAllBytes (Directory.GetCurrentDirectory () + "\\Images\\" + _ProfileImageFilesRecord.Name, Convert.FromBase64String (base64StringProfile));

                        _EmployeeImage.Add(new EmployeeImage
                        {
                            EmployeeId = _EmployeeTableProfile.Id,
                            ImageProfileCheck = true,
                            ImageName = "Profile",
                            ImageExtension = _ProfileImageFilesRecord.Extension,
                            ImageBytes = base64StringProfile,
                        });

                    }

                }
                await _context.EmployeeImages.AddRangeAsync(_EmployeeImage);
                _context.SaveChanges();
                //Profile Image End

                //CNIC Front Image Start
                string CNICFrontImage = Directory.GetCurrentDirectory() + "\\Images\\Cnic\\Front";
                DirectoryInfo _CNICFrontDirectory = new DirectoryInfo(CNICFrontImage);
                FileInfo[] _CNICFrontFiles = _CNICFrontDirectory.GetFiles("*.*");

                List<EmployeeImage> _EmployeeImageCNICFront = new List<EmployeeImage>();

                foreach (var _CNICFrontFilesRecord in _CNICFrontFiles)
                {
                    var _FileNameCNICFront = Path.GetFileNameWithoutExtension(_CNICFrontFilesRecord.Name);
                    var _EmployeeTableCNICFront = await _context.Employees.Where(x => x.MachineId == Convert.ToInt32(_FileNameCNICFront)).FirstOrDefaultAsync();
                    if (_EmployeeTableCNICFront != null)
                    {

                        string base64StringCNICFront = "";
                        byte[] imageBytesCNICFront = null;
                        using (Image image = Image.FromFile(_CNICFrontFilesRecord.FullName))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                imageBytesCNICFront = m.ToArray();

                            }
                        };
                        imageBytesCNICFront = _CompressImage.GetCompressImage(imageBytesCNICFront);
                        base64StringCNICFront = Convert.ToBase64String(imageBytesCNICFront);
                        //                        File.WriteAllBytes (Directory.GetCurrentDirectory () + "\\Images\\" + _CNICFrontFilesRecord.Name, Convert.FromBase64String (base64String));

                        _EmployeeImageCNICFront.Add(new EmployeeImage
                        {
                            EmployeeId = _EmployeeTableCNICFront.Id,
                            ImageProfileCheck = false,
                            ImageName = "CNIC Front",
                            ImageExtension = _CNICFrontFilesRecord.Extension,
                            ImageBytes = base64StringCNICFront,
                        });

                    }

                }
                await _context.EmployeeImages.AddRangeAsync(_EmployeeImageCNICFront);
                _context.SaveChanges();
                // CNIC Front Image End

                //CNIC Back Image Start
                string CNICBackImage = Directory.GetCurrentDirectory() + "\\Images\\Cnic\\Back";
                DirectoryInfo _CNICBackDirectory = new DirectoryInfo(CNICBackImage);
                FileInfo[] _CNICBackFiles = _CNICBackDirectory.GetFiles("*.*");

                List<EmployeeImage> _EmployeeImageCNICBack = new List<EmployeeImage>();

                foreach (var _CNICBackFilesRecord in _CNICBackFiles)
                {
                    var _FileNameCNICBack = Path.GetFileNameWithoutExtension(_CNICBackFilesRecord.Name);
                    var _EmployeeTableCNICBack = await _context.Employees.Where(x => x.MachineId == Convert.ToInt32(_FileNameCNICBack)).FirstOrDefaultAsync();
                    if (_EmployeeTableCNICBack != null)
                    {

                        string base64StringCNICBack = "";
                        byte[] imageBytesCNICBack = null;
                        using (Image image = Image.FromFile(_CNICBackFilesRecord.FullName))
                        {
                            using (MemoryStream m = new MemoryStream())
                            {
                                image.Save(m, image.RawFormat);
                                imageBytesCNICBack = m.ToArray();

                            }
                        };
                        imageBytesCNICBack = _CompressImage.GetCompressImage(imageBytesCNICBack);
                        base64StringCNICBack = Convert.ToBase64String(imageBytesCNICBack);
                        //                        File.WriteAllBytes (Directory.GetCurrentDirectory () + "\\Images\\" + _CNICBackFilesRecord.Name, Convert.FromBase64String (base64String));

                        _EmployeeImageCNICBack.Add(new EmployeeImage
                        {
                            EmployeeId = _EmployeeTableCNICBack.Id,
                            ImageProfileCheck = false,
                            ImageName = "CNIC Back",
                            ImageExtension = _CNICBackFilesRecord.Extension,
                            ImageBytes = base64StringCNICBack,
                        });

                    }

                }
                await _context.EmployeeImages.AddRangeAsync(_EmployeeImageCNICBack);
                _context.SaveChanges();
                //CNIC Back Image End

                ApiResponse.statusCode = StatusCodes.Status200OK.ToString();
                ApiResponse.message = "Image Uploaded";
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
        //Employee

        public async Task<ApiResponse> GetSalesmanLovAsync(ClaimsPrincipal _User, string _Search)
        {
            var apiResponse = new ApiResponse();
            try
            {
                string _UserKey = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();
                apiResponse = await _SecurityHelper.UserInfoKey(_UserKey);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserLoginInfoBaseModel = (UserLoginInfoBaseModel)apiResponse.data;


                var _Employee = await _context.Employees.Include(e => e.designation).Where(a => a.Action != Enums.Operations.D.ToString() && a.designation.Salesman == true && a.Active == true && (string.IsNullOrEmpty(_Search) ? true : a.Name.Contains(_Search))).ToListAsync();



                if (_Employee == null)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }

                if (_Employee.Count == 0)
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Record not found";
                    return apiResponse;
                }

                List<string> _EmployeeReporting = new List<string>();
                if (_UserLoginInfoBaseModel.ckSalesman)
                {
                    PayrollHelper _PayrollHelper = new PayrollHelper();
                    _EmployeeReporting = await _PayrollHelper.EmployeeByIdReportingAsync(_UserLoginInfoBaseModel.EmployeeId.Value);

                }
                // && x.Id == _UserLoginInfoBaseModel.EmployeeId
                List<LovServicesViewModel> _LovServicesViewModel = new List<LovServicesViewModel>();
                var _EmployeeFilter = _UserLoginInfoBaseModel.ckDirector ? _Employee : _UserLoginInfoBaseModel.ckSalesman ? _Employee.Where(x => _EmployeeReporting.Contains(x.Id.ToString())).ToList() : _Employee;
                foreach (var _Record in _EmployeeFilter)
                {
                    _LovServicesViewModel.Add(new LovServicesViewModel
                    {
                        Id = _Record.Id,
                        Name = _Record.Name
                    });//.OrderBy(o => o.Name).ToListAsync();
                }
                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.data = _LovServicesViewModel.OrderBy(o => o.Name);
                return apiResponse;

            }
            catch (Exception e)
            {

                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = e.Message.ToString() + innerexp;
                return apiResponse;
            }
        }


    }

}