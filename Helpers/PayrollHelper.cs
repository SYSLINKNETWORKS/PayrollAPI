using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;

namespace TWP_API_Payroll.Helpers
{
    public class PayrollHelper
    {
        string _connectionString = "";
        DataContext _context;

        //Encrypt Password Start
        public PayrollHelper()
        {
            var configuration = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json", false)
                                      .Build();

            _connectionString = configuration.GetConnectionString("Connection").ToString();

            var options = new DbContextOptionsBuilder<DataContext>().UseSqlServer(_connectionString).Options;
            _context = new DataContext(options);
        }

        public async Task<List<string>> EmployeeByIdReportingAsync(Guid _Id)
        {
            List<string> _EmployeeIdList = new List<string>();
            var _Table = await _context.Employees.Include(x => x.designation).Where(a => a.Action != Enums.Operations.D.ToString() && a.Id == _Id).FirstOrDefaultAsync();
            EmployeeByIdReportingMicroServiceViewModel _EmployeesTable = new EmployeeByIdReportingMicroServiceViewModel();

            if (_Table == null)
            {
                return _EmployeeIdList;
            }
            var _EmployeeId = _Table.Id;
            //            _EmployeeIdStr = _Table.Id.ToString();

            _EmployeeIdList.Insert(0, _Table.Id.ToString());
            int _rowcnt = 0;
            while (true)
            {

                var _EmployeeReportingTable = await _context.Employees.Where(x => x.ReportOfficerId == _EmployeeId).FirstOrDefaultAsync();
                if (_EmployeeReportingTable == null)
                {
                    break;
                }
                _rowcnt += 1;
                _EmployeeId = _EmployeeReportingTable.Id;
                //_EmployeeIdStr += "," + _EmployeeReportingTable.Id;
                _EmployeeIdList.Insert(_rowcnt, _EmployeeReportingTable.Id.ToString());
            }
            return _EmployeeIdList;
        }

    }
}
