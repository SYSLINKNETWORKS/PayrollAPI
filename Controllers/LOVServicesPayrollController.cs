using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Repository;

namespace TWP_API_Payroll.Controllers
{
    ///<summary>
    ///LOV
    ///</summary>


    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class LOVServicesPayrollController : ControllerBase
    {
        private readonly IPayrollSevicesRepository IPayrollSevicesRepository = null;
        public LOVServicesPayrollController(IPayrollSevicesRepository _IPayrollSevicesRepository)
        {
            IPayrollSevicesRepository = _IPayrollSevicesRepository;
        }

        ///<summary>
        ///Get All Departments
        ///</summary>
        [HttpGet]
        [Route("GetDepartment")]
        public async Task<IActionResult> GetDepartment(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetDepartmentLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Roster
        ///</summary>

        [HttpGet]
        [Route("GetRoster")]
        public async Task<IActionResult> GetRoster(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetRosterLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Annaul Leave
        ///</summary>
        [HttpGet]
        [Route("GetAnnualLeave")]
        public async Task<IActionResult> GetAnnualLeave(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetAnnualLeaveLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Attendance Machine Category
        ///</summary>
        [HttpGet]
        [Route("GetAttendanceMachineCategory")]
        public async Task<IActionResult> GetAttendanceMachineCategory(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetAttendanceMachineCategoryLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Attendance Machine Group
        ///</summary>
        [HttpGet]
        [Route("GetAttendanceMachineGroup")]
        public async Task<IActionResult> GetAttendanceMachineGroup(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetAttendanceMachineGroupLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Employee Category
        ///</summary>
        [HttpGet]
        [Route("GetEmployeeCategory")]
        public async Task<IActionResult> GetEmployeeCategory(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetEmployeeCategoryLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Designation
        ///</summary>
        [HttpGet]
        [Route("GetDesignation")]
        public async Task<IActionResult> GetDesignation(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetDesignationLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Employess
        ///</summary>
        [HttpGet]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmployee(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetEmployeeLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Salesman
        ///</summary>
        [HttpGet]
        [Route("GetSalesman")]
        public async Task<IActionResult> GetSalesman(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetSalesmanLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        ///<summary>
        ///Get All Inout Category
        ///</summary>
        [HttpGet]
        [Route("GetInOutCategory")]
        public async Task<IActionResult> GetInOutCategory(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetInOutCategoryLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Loan Category
        ///</summary>
        [HttpGet]
        [Route("GetLoanCategory")]
        public async Task<IActionResult> GetLoanCategory(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetLoanCategoryLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        ///<summary>
        ///Get All Previous Salary Date and Employee Wise
        ///</summary>
        [HttpGet]
        [Route("GetPerviousSalary")]
        public async Task<IActionResult> GetPerviousSalary([FromHeader] DateTime SalaryDate, [FromHeader] Guid EmployeeId)
        {
            var result = await IPayrollSevicesRepository.GetPerviousSalaryLovAsync(User, SalaryDate, EmployeeId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get All Roster Employee Id Wise
        ///</summary>
        [HttpGet]
        [Route("GetRosterByEmployeeId")]
        public async Task<IActionResult> GetRosterByEmployeeId([FromHeader] Guid EmployeeId, [FromHeader] DateTime RosterDateFrom, [FromHeader] DateTime RosterDateTo, [FromHeader] bool Tag)
        {
            var result = await IPayrollSevicesRepository.GetRosterByEmployeeIdLovAsync(User, EmployeeId, RosterDateFrom, RosterDateTo,Tag);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

              

        ///<summary>
        ///Get All Loan for Receiving
        ///</summary>
        [HttpGet]
        [Route("GetLoanforReceiving")]
        public async Task<IActionResult> GetLoanforReceiving(String _srch)
        {
            var result = await IPayrollSevicesRepository.GetLoanforReceivingLovAsync(User, _srch);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        ///<summary>
        ///Get Image in Bytes
        ///</summary>
        [HttpGet]
        [Route("GetReadImageToBytes")]
        public async Task<IActionResult> GetReadImageToBytes()
        {
            var result = await IPayrollSevicesRepository.GetReadImageToBytesAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Fetch Loan for Receiving Id End
    }
}