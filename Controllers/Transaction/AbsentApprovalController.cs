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
    ///Absent Approval
    ///</summary>

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class AbsentApprovalController : ControllerBase
    {
        private readonly IPayrollAbsentApprovalSevicesRepository IPayrollAbsentApprovalSevicesRepository = null;
        public AbsentApprovalController(IPayrollAbsentApprovalSevicesRepository _IPayrolllAbsentApprovalSevicesRepository)
        {
            IPayrollAbsentApprovalSevicesRepository = _IPayrolllAbsentApprovalSevicesRepository;
        }

        //Get AbsentApproval  Start
        [HttpGet]
        public async Task<IActionResult> GetAbsentApproval([FromHeader] Guid _Menuid, [FromHeader] DateTime _Date)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollAbsentApprovalSevicesRepository.GetAbsentApprovalLovAsync(User, _TokenString, _Menuid, _Date);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Get AbsentApproval  End

        //Add AbsentApproval  Start
        [HttpPut]
        public async Task<IActionResult> AddAbsentApprovalApproval([FromBody] AbsentApprovalAddModel _AbsentApprovalAddModel)
        {
            var result = await IPayrollAbsentApprovalSevicesRepository.AddAbsentApprovalLovAsync(_AbsentApprovalAddModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }

}