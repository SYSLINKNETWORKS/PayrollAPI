
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll.Report;

namespace TWP_API_Payroll.Services
{
    public interface IMicroservices
    {
        Task<ApiResponse> MSDepartmentsAsync(string _Key);

        Task<ApiResponse> MSEmployeesAsync(string _Key);
        Task<ApiResponse> MSEmployeeByIdAsync(string _Key, Guid _Id);
        Task<ApiResponse> MSEmployeeByIdReportingAsync(string _Key, Guid _Id);
        Task<ApiResponse> MSLoginAttendanceAsync(string _Passphase, string _EmployeeId, string _LoginDate);

    }

    public class Microservices : IMicroservices
    {
        private readonly DataContext _context;
        SecurityHelper _SecurityHelper = new SecurityHelper();
        PayrollHelper _PayrollHelper = new PayrollHelper();

        public Microservices(DataContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse> MSDepartmentsAsync(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse = await _SecurityHelper.KeyValidation(_Key);
            if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
            {
                return apiResponse;
            }
            var _Table = await _context.Departments.Where(a => a.Action != Enums.Operations.D.ToString()).ToListAsync();
            List<DepartmentsViewModel> _DepartmentsViewModel = new List<DepartmentsViewModel>();
            foreach (var ViewTable in _Table)
            {
                _DepartmentsViewModel.Add(new DepartmentsViewModel
                {
                    Id = ViewTable.Id,
                    DepartmentName = ViewTable.Name,
                });

            }
            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            apiResponse.data = _DepartmentsViewModel;

            return apiResponse;
        }

        public async Task<ApiResponse> MSEmployeesAsync(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse = await _SecurityHelper.KeyValidation(_Key);
            if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
            {
                return apiResponse;
            }
            var _Table = await _context.Employees.Where(a => a.Action != Enums.Operations.D.ToString()).ToListAsync();
            List<EmployeesMicroServiceViewModel> _EmployeesMicroServiceViewModel = new List<EmployeesMicroServiceViewModel>();
            foreach (var ViewTable in _Table)
            {
                _EmployeesMicroServiceViewModel.Add(new EmployeesMicroServiceViewModel
                {
                    Id = ViewTable.Id,
                    MachineId = ViewTable.MachineId,
                    Name = ViewTable.Name,
                    FatherName = ViewTable.FatherName,
                    FullName = ViewTable.Name + " " + ViewTable.FatherName,
                });

            }
            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            apiResponse.data = _EmployeesMicroServiceViewModel;

            return apiResponse;
        }
        public async Task<ApiResponse> MSEmployeeByIdAsync(string _Key, Guid _Id)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse = await _SecurityHelper.KeyValidation(_Key);
            if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
            {
                return apiResponse;
            }

            var _Table = await _context.Employees.Include(x => x.designation).Where(a => a.Action != Enums.Operations.D.ToString() && a.Id == _Id).FirstOrDefaultAsync();
            EmployeeByIdMicroServiceViewModel _EmployeesTable = new EmployeeByIdMicroServiceViewModel();

            if (_Table == null)
            {
                apiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                apiResponse.message = "Record Not Found";
                return apiResponse;
            }

            _EmployeesTable.Id = _Table.Id;
            _EmployeesTable.Name = _Table.Name;
            _EmployeesTable.Salesman = _Table.designation.Salesman;
            _EmployeesTable.Director = _Table.designation.Director;


            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            apiResponse.data = _EmployeesTable;

            return apiResponse;
        }
        public async Task<ApiResponse> MSEmployeeByIdReportingAsync(string _Key, Guid _EmployeeId)
        {
            ApiResponse apiResponse = new ApiResponse();
            apiResponse = await _SecurityHelper.KeyValidation(_Key);
            if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
            {
                return apiResponse;
            }

            List<string> _EmployeeIdStr = await _PayrollHelper.EmployeeByIdReportingAsync(_EmployeeId);
            if (_EmployeeIdStr.Count == 0)
            {
                apiResponse.statusCode = StatusCodes.Status404NotFound.ToString(); ;
                apiResponse.message = "Record Not Found";
                return apiResponse;
            }



            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            apiResponse.data = _EmployeeIdStr;

            return apiResponse;
        }
        public async Task<ApiResponse> MSLoginAttendanceAsync(string _Passphase, string _EmployeeId, string _LoginDate)
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false)
                    .Build();
            var Passphase = configuration["AuthSettings:Key"];
            if (Passphase != _Passphase)
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                apiResponse.message = "Invalid credentials";
                return apiResponse;
            }

            DateTime _Date1 = Convert.ToDateTime(_LoginDate);
            var _AttendanceTable = await _context.CheckAttendances.Where(x => x.EmployeeId == new Guid(_EmployeeId) && x.Date == _Date1 && x.Approved == true).FirstOrDefaultAsync();
            if (_AttendanceTable != null)
            {
                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.data = true;
                return apiResponse;
            }



            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Attendance not mark";
            apiResponse.data = false;

            return apiResponse;
        }
    }

}
