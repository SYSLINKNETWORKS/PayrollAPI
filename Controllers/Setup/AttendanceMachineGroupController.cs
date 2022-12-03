using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Processor;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Controllers {
    ///<summary>
    ///AttendanceMachineGroup
    ///</summary>

    [Route ("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion ("1.0")]
    [Authorize]
    public class AttendanceMachineGroupController : ControllerBase {
        private readonly IProcessor<AttendanceMachineGroupBaseModel> _IProcessor = null;
        public AttendanceMachineGroupController (IProcessor<AttendanceMachineGroupBaseModel> IProcessor) {
            _IProcessor = IProcessor;
        }

        ///<summary>
        ///Get All Record
        ///</summary>
        [HttpGet]
        public async Task<IActionResult> GetAttendanceMachineGroup (Guid _MenuId) {
            try {
                var result = await _IProcessor.ProcessGet (_MenuId, User);
                return Ok (result);
            } catch (Exception e) {
                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                return BadRequest (e.Message.ToString () + innerexp);
            }
        }
        //Get End

        ///<summary>
        ///Get Record by id
        ///</summary>
        [HttpGet]
        [Route ("GetAttendanceMachineGroupById")]
        public async Task<IActionResult> GetAttendanceMachineGroupById ([FromHeader] Guid _MenuId,[FromHeader]  Guid _Id) {
            try {
                var result = await _IProcessor.ProcessGetById (_Id, _MenuId, User);
                return Ok (result);
            } catch (Exception e) {
                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                return BadRequest (e.Message.ToString () + innerexp);
            }
        }
        //Get by Id End


        ///<summary>
        ///Save Record
        ///</summary>
        [HttpPost]
        public async Task<IActionResult> AddAttendanceMachineGroup ([FromBody] AttendanceMachineGroupAddModel model) {
            try {
                return Ok (await _IProcessor.ProcessPost (model, User));
            } catch (Exception e) {
                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                return BadRequest (e.Message.ToString () + innerexp);
            }
        }
        //Create End

        //Update Start
        ///<summary>
        ///Update Record
        ///</summary>
        [HttpPut]
        public async Task<IActionResult> UpdateAttendanceMachineGroup ([FromBody] AttendanceMachineGroupUpdateModel model) {
            try {
                return Ok (await _IProcessor.ProcessPut (model, User));
            } catch (Exception e) {
                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                return BadRequest (e.Message.ToString () + innerexp);
            }
        }
        //Update End

        //Delete Start
        ///<summary>
        ///Delete Record
        ///</summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteAttendanceMachineGroup ([FromBody] AttendanceMachineGroupDeleteModel model) {
            try {
                return Ok (await _IProcessor.ProcessDelete (model, User));
            } catch (Exception e) {
                string innerexp = "";
                if (e.InnerException != null) {
                    innerexp = " Inner Error : " + e.InnerException.ToString ();
                }
                return BadRequest (e.Message.ToString () + innerexp);
            }
        }
        //Delete End

    }

}