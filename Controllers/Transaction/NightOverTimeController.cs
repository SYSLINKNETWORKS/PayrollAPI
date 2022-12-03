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
    ///NightOverTime
    ///</summary>

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class NightOverTimeController : ControllerBase
    {
        private readonly IPayrollNightOverTimeSevicesRepository IPayrollNightOverTimeSevicesRepository = null;
        public NightOverTimeController(IPayrollNightOverTimeSevicesRepository _IPayrolllNightOverTimeSevicesRepository)
        {
            IPayrollNightOverTimeSevicesRepository = _IPayrolllNightOverTimeSevicesRepository;
        }

        ///<summary>
        ///Get Night Overtime
        ///</summary>
        [HttpGet]
//        [Route("GetNightOverTime")]
        public async Task<IActionResult> GetNightOverTime([FromHeader] DateTime dateAsOn)
        {
            var result = await IPayrollNightOverTimeSevicesRepository.GetNightOverTimeLovAsync(User, dateAsOn);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //Fetch NightOverTime Start
        [HttpPost]
        public async Task<IActionResult> AddNightOverTime([FromBody] NightOverTimeAddModel _NightOverTimeAddModel)
        {
            var result = await IPayrollNightOverTimeSevicesRepository.AddNightOverTimeLovAsync(_NightOverTimeAddModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Fetch NightOverTime End

        //         //Edit NightOverTime Start
        //         [HttpPut]
        // //        [Route ("GetById")]
        //         public async Task<IActionResult> GetById ([FromBody] NightOverTimeGetByIdModel _NightOverTimeGetByIdModel) {
        //             var result = await IPayrollNightOverTimeSevicesRepository.GetNightOverTimeByIDLovAsync (_NightOverTimeGetByIdModel, User);
        //             if (result == null) {
        //                 return NotFound ();
        //             }
        //             return Ok (result);
        //         }
        //         //Edit NightOverTime End

    }

}