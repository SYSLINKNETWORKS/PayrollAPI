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
    public class InOutEditorApprovalController : ControllerBase
    {
        private readonly IPayrollInOutEditorApprovalSevicesRepository IPayrollInOutEditorApprovalSevicesRepository = null;
        public InOutEditorApprovalController(IPayrollInOutEditorApprovalSevicesRepository _IPayrolllInOutEditorApprovalSevicesRepository)
        {
            IPayrollInOutEditorApprovalSevicesRepository = _IPayrolllInOutEditorApprovalSevicesRepository;
        }

        ///<summary>
        ///Get In Out Editor Record
        ///</summary>
        [HttpGet]
        public async Task<IActionResult> GetInOutEditorApproval([FromHeader] Guid _Menuid, [FromHeader] DateTime _DateFrom, [FromHeader] DateTime _DateTo)
        {
            var result = await IPayrollInOutEditorApprovalSevicesRepository.GetInOutEditorApprovalLovAsync(User, _Menuid, _DateFrom, _DateTo);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Update In Out Editor Record
        ///</summary>
        [HttpPut]
        public async Task<IActionResult> EditInOutEditorApprovalApproval([FromBody] InOutEditorApprovalEditModel _InOutEditorApprovalApprovalEditModel)
        {
            var result = await IPayrollInOutEditorApprovalSevicesRepository.EditInOutEditorApprovalLovAsync(_InOutEditorApprovalApprovalEditModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Edit InOutEditorApproval  End

    }

}