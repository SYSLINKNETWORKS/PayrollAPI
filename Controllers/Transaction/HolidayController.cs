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
    ///InOutEditorApproval
    ///</summary>

    [Route ("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion ("1.0")]
    [Authorize]
    public class HolidayController : ControllerBase {
        private readonly IPayrollHolidaySevicesRepository IPayrollHolidaySevicesRepository = null;
        public HolidayController (IPayrollHolidaySevicesRepository _IPayrolllHolidaySevicesRepository) {
            IPayrollHolidaySevicesRepository = _IPayrolllHolidaySevicesRepository;
        }

        //Get Holiday  Start
        [HttpGet]
        public async Task<IActionResult> GetHoliday ([FromHeader] Guid _Menuid,[FromHeader] DateTime _date) {
            var result = await IPayrollHolidaySevicesRepository.GetHolidayLovAsync (User, _Menuid,_date);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Get Holiday  End

        //Add Holiday  Start
        [HttpPut]
        public async Task<IActionResult> AddHoliday ([FromBody] HolidayAddModel _HolidayAddModel ) {
            var result = await IPayrollHolidaySevicesRepository.AddHolidayLovAsync (_HolidayAddModel,User);
            if (result == null) {
                return NotFound ();
            }
            return Ok (result);
        }
        //Edit InOutEditorApproval  End

     
    }

}