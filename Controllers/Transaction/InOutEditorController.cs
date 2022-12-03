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
    ///InOutEditor
    ///</summary>

    [Route ("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion ("1.0")]
    [Authorize]
    public class InOutEditorController : ControllerBase {
        private readonly IPayrollInOutEditorSevicesRepository IPayrollInOutEditorSevicesRepository = null;
        public InOutEditorController (IPayrollInOutEditorSevicesRepository _IPayrolllInOutEditorSevicesRepository) {
            IPayrollInOutEditorSevicesRepository = _IPayrolllInOutEditorSevicesRepository;
        }

        //Fetch InOutEditor Start
        [HttpGet]
        public async Task<IActionResult> GetInOutEditor ([FromHeader] Guid _Menuid, [FromHeader] DateTime _DateFrom,[FromHeader] DateTime _DateTo) {
            var result = await IPayrollInOutEditorSevicesRepository.GetInOutEditorLovAsync (User, _Menuid, _DateFrom,_DateTo);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Fetch InOutEditor End

        //Fetch InOutEditor Start
        [HttpPost]
        public async Task<IActionResult> AddInOutEditor ([FromBody] InOutEditorAddModel _InOutEditorAddModel) {
            var result = await IPayrollInOutEditorSevicesRepository.AddInOutEditorLovAsync (_InOutEditorAddModel, User);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Fetch InOutEditor End

        //Edit InOutEditor Start
        [HttpPut]
//        [Route ("GetById")]
        public async Task<IActionResult> GetById ([FromBody] InOutEditorGetByIdModel _InOutEditorGetByIdModel) {
            var result = await IPayrollInOutEditorSevicesRepository.GetInOutEditorByIDLovAsync (_InOutEditorGetByIdModel, User);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Edit InOutEditor End

    }

}