using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using TWP_API_Payroll.Bussiness;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.Services;

namespace TWP_API_Payroll.Generic
{
    public static class Builder
    {
        public static AbsBusiness MakeBusinessClass(Enums.ClassName ClassName, App_Data.DataContext _context)
        {
            switch (ClassName)
            {


                //Payroll Start
                case Enums.ClassName.Designation:
                    { return new BDesignation(_context); }
                case Enums.ClassName.Department:
                    { return new BDepartment(_context); }
                case Enums.ClassName.Roster:
                    { return new BRoster(_context); }
                case Enums.ClassName.RosterGroup:
                    { return new BRosterGroup(_context); }
                case Enums.ClassName.IncomeTaxSlabEmployee:
                    { return new BIncomeTaxSlabEmployee(_context); }
                case Enums.ClassName.EmployeeCategory:
                    { return new BEmployeeCategory(_context); }
                case Enums.ClassName.MachineCompany:
                    { return new BMachineCompany(_context); }
                case Enums.ClassName.AttendanceMachineGroup:
                    { return new BAttendanceMachineGroup(_context); }
                case Enums.ClassName.AttendanceMachine:
                    { return new BAttendanceMachine(_context); }
                case Enums.ClassName.AnnualLeaves:
                    { return new BAnnualLeaves(_context); }
                case Enums.ClassName.InOutCategory:
                    { return new BInOutCategory(_context); }
                case Enums.ClassName.LoanCategory:
                    { return new BLoanCategory(_context); }
                case Enums.ClassName.Allowance:
                    { return new BAllowance(_context); }
                case Enums.ClassName.EmployeeProfile:
                    { return new BEmployeeProfile(_context); }
                case Enums.ClassName.EmployeeProfileDocument:
                    { return new BEmployeeProfileDocument(_context); }
                case Enums.ClassName.Advance:
                    { return new BAdvance(_context); }
                case Enums.ClassName.LoanIssue:
                    { return new BLoanIssue(_context); }
                case Enums.ClassName.SalaryAdditionDeduction:
                    { return new BSalaryAdditionDeduction(_context); }
                case Enums.ClassName.LoanReceive:
                    { return new BLoanReceive(_context); }
                case Enums.ClassName.Salary:
                    { return new BSalary(_context); }

                //Payroll End

                default:
                    return null;

            }

        }


    }
}