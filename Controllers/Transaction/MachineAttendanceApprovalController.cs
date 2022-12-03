using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Processor;
using TWP_API_Payroll.Repository;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Controllers {
    ///<summary>
    ///MachineAttendanceApproval
    ///</summary>

    [Route ("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion ("1.0")]
    [Authorize]
    public class MachineAttendanceApprovalController : ControllerBase {
        private readonly IPayrollMachineAttendanceApprovalSevicesRepository IPayrollMachineAttendanceApprovalSevicesRepository = null;
        public MachineAttendanceApprovalController (IPayrollMachineAttendanceApprovalSevicesRepository _IPayrolllMachineAttendanceApprovalSevicesRepository) {
            IPayrollMachineAttendanceApprovalSevicesRepository = _IPayrolllMachineAttendanceApprovalSevicesRepository;
        }

        //Edit MachineAttendanceApproval  Start
        [HttpGet]
        public async Task<IActionResult> GetMachineAttendanceApproval ([FromHeader] Guid _Menuid, [FromHeader] DateTime _DateFrom, [FromHeader] DateTime _DateTo) {
            var result = await IPayrollMachineAttendanceApprovalSevicesRepository.GetMachineAttendanceApprovalLovAsync (User, _Menuid, _DateFrom, _DateTo);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Edit MachineAttendanceApproval  End

        //Edit MachineAttendanceApproval  Start
        [HttpPut]
        public async Task<IActionResult> EditMachineAttendanceApprovalApproval ([FromBody] MachineAttendanceApprovalEditModel _MachineAttendanceApprovalApprovalEditModel) {
            var result = await IPayrollMachineAttendanceApprovalSevicesRepository.EditMachineAttendanceApprovalLovAsync (_MachineAttendanceApprovalApprovalEditModel, User);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Edit MachineAttendanceApproval  End

    }

}