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
    ///MultiLoanReceiving
    ///</summary>

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class MultiLoanReceivingController : ControllerBase
    {
        private readonly IPayrollMultiLoanReceivingSevicesRepository IPayrollMultiLoanReceivingSevicesRepository = null;
        public MultiLoanReceivingController(IPayrollMultiLoanReceivingSevicesRepository _IPayrolllMultiLoanReceivingSevicesRepository)
        {
            IPayrollMultiLoanReceivingSevicesRepository = _IPayrolllMultiLoanReceivingSevicesRepository;
        }

        //Edit MultiLoanReceiving  Start
        [HttpGet]
        public async Task<IActionResult> GetMultiLoanReceiving([FromHeader] Guid _Menuid, [FromHeader] DateTime _Date)
        {
            var result = await IPayrollMultiLoanReceivingSevicesRepository.GetMultiLoanReceivingLovAsync(User, _Menuid, _Date);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Edit MultiLoanReceiving  End

        //Edit MultiLoanReceiving  Start
        [HttpPut]
        public async Task<IActionResult> EditMultiLoanReceivingApproval([FromBody] MultiLoanReceivingEditModel _MultiLoanReceivingApprovalEditModel, [FromHeader] DateTime _Date)
        {
            var result = await IPayrollMultiLoanReceivingSevicesRepository.EditMultiLoanReceivingLovAsync(_MultiLoanReceivingApprovalEditModel, _Date, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Edit MultiLoanReceiving  End

    }

}