using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TWP_API_Payroll.Repository;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.ViewModels.Payroll.Dashboard;

namespace TWP_API_Payroll.Controllers.Dashboard
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class PayrollApprovalDashBoardController : ControllerBase
    {

        private readonly IPayrollApprovalDashBoardSevicesRepository IPayrollApprovalDashBoardSevicesRepository = null;
        public PayrollApprovalDashBoardController(IPayrollApprovalDashBoardSevicesRepository _IPayrollApprovalDashBoardSevicesRepository)
        {
            IPayrollApprovalDashBoardSevicesRepository = _IPayrollApprovalDashBoardSevicesRepository;
        }

        ///<summary>
        ///Get In Out Editor Record
        ///</summary>
        [HttpPost]
        [Route("GetInOutEditor")]
        public async Task<IActionResult> GetInOutEditor([FromBody] DashboardFilterViewModel DashboardFilterViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.GetInOutEditorApprovalLovAsync(DashboardFilterViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get Night Over Time Record
        ///</summary>
        [HttpPost]
        [Route("GetNightOverTime")]
        public async Task<IActionResult> GetNightOverTime([FromBody] DashboardFilterViewModel DashboardFilterViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.GetNightOverTimeLovAsync(DashboardFilterViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Approve Night Over Time Record
        ///</summary>
        [HttpPut]
        [Route("UpdateNightOverTime")]
        public async Task<IActionResult> UpdateNightOverTime([FromBody] UpdateNightOverTimeDashboardViewModel UpdateNightOverTimeDashboardViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.UpdateNightOverTimeLovAsync(UpdateNightOverTimeDashboardViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        ///<summary>
        ///Get Advance Record
        ///</summary>
        [HttpPost]
        [Route("GetAdvance")]
        public async Task<IActionResult> GetAdvance([FromBody] DashboardFilterViewModel DashboardFilterViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.GetAdvanceLovAsync(DashboardFilterViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Approve Advance Record
        ///</summary>
        [HttpPut]
        [Route("UpdateAdvance")]
        public async Task<IActionResult> UpdateAdvance([FromBody] UpdateDashboardViewModel UpdateDashboardViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.UpdateAdvanceLovAsync(UpdateDashboardViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Get Loan Record
        ///</summary>
        [HttpPost]
        [Route("GetLoan")]
        public async Task<IActionResult> GetLoan([FromBody] DashboardFilterViewModel DashboardFilterViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.GetLoanLovAsync(DashboardFilterViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Approve Advance Record
        ///</summary>
        [HttpPut]
        [Route("UpdateLoan")]
        public async Task<IActionResult> UpdateLoan([FromBody] UpdateDashboardViewModel UpdateDashboardViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.UpdateLoanLovAsync(UpdateDashboardViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        ///<summary>
        ///Get Employee Record
        ///</summary>
        [HttpPost]
        [Route("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromBody] DashboardFilterViewModel DashboardFilterViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.GetEmployeeLovAsync(DashboardFilterViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        ///<summary>
        ///Approve Employee Record
        ///</summary>
        [HttpPut]
        [Route("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateDashboardViewModel UpdateDashboardViewModel)
        {
            var result = await IPayrollApprovalDashBoardSevicesRepository.UpdateEmployeeLovAsync(UpdateDashboardViewModel, User);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


    }

}