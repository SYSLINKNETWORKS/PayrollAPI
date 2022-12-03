using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Processor;
using TWP_API_Payroll.Repository;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Controllers
{
    ///<summary>
    ///InOutEditorApproval
    ///</summary>

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class SalaryProcessController : ControllerBase
    {
        private readonly IPayrollSalaryProcessSevicesRepository IPayrollSalaryProcessSevicesRepository = null;
        public SalaryProcessController(IPayrollSalaryProcessSevicesRepository _IPayrolllSalaryProcessSevicesRepository)
        {
            IPayrollSalaryProcessSevicesRepository = _IPayrolllSalaryProcessSevicesRepository;
        }



        ///<summary>
        ///Staff Salary Process
        ///</summary>
        [HttpPost]
        [Route("StaffSalaryProcess")]
        public async Task<IActionResult> StaffSalaryProcess([FromHeader] Guid MenuId, [FromHeader] DateTime SalaryDate, [FromHeader] String EmployeeId)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollSalaryProcessSevicesRepository.StaffSalaryProcess(User, _TokenString, MenuId, SalaryDate, EmployeeId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Staff Salary Voucher Post
        ///</summary>
        [HttpPost]
        [Route("StaffSalaryVoucherPost")]
        public async Task<IActionResult> StaffSalaryVoucherPost([FromHeader] Guid MenuId, [FromHeader] DateTime SalaryDate)
        {
            var result = await IPayrollSalaryProcessSevicesRepository.StaffSalaryVoucherPost(User, MenuId, SalaryDate);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Worker Salary Process
        ///</summary>
        [HttpPost]
        [Route("WorkerSalaryProcess")]
        public async Task<IActionResult> WorkerSalaryProcess([FromHeader] Guid Menuid, [FromHeader] DateTime SalaryDate, [FromHeader] String EmployeeId)
        {
            string _TokenString=HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollSalaryProcessSevicesRepository.WorkerSalaryProcess(User,_TokenString, Menuid, SalaryDate, EmployeeId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Worker Salary Voucher Post
        ///</summary>
        [HttpPost]
        [Route("WorkerSalaryVoucherPost")]
        public async Task<IActionResult> WorkerSalaryVoucherPost([FromHeader] Guid Menuid, [FromHeader] DateTime SalaryDate)
        {
            var result = await IPayrollSalaryProcessSevicesRepository.WorkerSalaryVoucherPost(User, Menuid, SalaryDate);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }

}