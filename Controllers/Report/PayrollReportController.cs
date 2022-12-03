using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Repository;
using TWP_API_Payroll.ViewModels.Payroll.Report;

namespace TWP_API_Payroll.Controllers.Payroll.Report
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]

    public class PayrollReportController : ControllerBase
    {
        private readonly IPayrollReportSevicesRepository IPayrollReportSevicesRepository = null;
        public PayrollReportController(IPayrollReportSevicesRepository _IPayrollReportSevicesRepository)
        {
            IPayrollReportSevicesRepository = _IPayrollReportSevicesRepository;
        }

        [HttpPost]
        [Route("TimeSheet")]
        public async Task<IActionResult> TimeSheet([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetTimeSheetAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("Absent")]
        public async Task<IActionResult> Absent([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetAbsentAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        [Route("OverTime")]
        public async Task<IActionResult> OverTime([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetOverTimeAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //Single Day Attandance Start
        [HttpPost]
        // [Route ("~/Payroll/Report/SingleDayAttandance/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("SingleDayAttandance")]
        public async Task<IActionResult> SingleDayAttandance([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {

            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetSingleDayAttandanceAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Single Day Attandance End
        //Inout Editor Start
        [HttpPost]
        // [Route ("~/Payroll/Report/Advance/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("InOutEditor")]
        public async Task<IActionResult> InOutEditor([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetInOutEditorAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Advance End

        //Advanced Start
        [HttpPost]
        // [Route ("~/Payroll/Report/Advance/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("Advance")]
        public async Task<IActionResult> Advance([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetAdvanceAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Advance End

        //Loan Start
        [HttpPost]
        // [Route ("~/Payroll/Report/Loan/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("Loan")]
        public async Task<IActionResult> Loan([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetLoanAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }

        //Loan Start
        [HttpPost]
        // [Route ("~/Payroll/Report/LoanRecieve/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("LoanRecieve")]
        public async Task<IActionResult> LoanRecieve([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetLoanReceiveAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Loan End

        //DailyAttendance Start
        [HttpPost]
        // [Route ("~/Payroll/Report/DailyAttendance/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("DailyAttendance")]
        public async Task<IActionResult> DailyAttendance([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetDailyAttandanceAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //DailyAttendance End

        //Employee Profile Start
        [HttpPost]
        // [Route ("~/Payroll/Report/EmployeeProfile/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("EmployeeProfile")]
        public async Task<IActionResult> EmployeeProfile([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetEmployeeProfileAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Employee Profile End

        //Employee Profile Image Start
        [HttpGet]
        [Route("EmployeeProfileDocument")]
        public async Task<IActionResult> EmployeeProfileDocument([FromHeader] Guid EmployeeId)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetEmployeeProfileDocumentAsync(_TokenString, EmployeeId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Employee Profile Image End

        //SalaryIncrement Start
        [HttpPost]
        // [Route ("~/Payroll/Report/Loan/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("SalaryIncrement")]
        public async Task<IActionResult> SalaryIncrement([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

            var result = await IPayrollReportSevicesRepository.GetSalaryIncrementAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //SalaryIncrement End

        //CurrentSalaries Start
        [HttpPost]
        // [Route ("~/Payroll/Report/CurrentSalaries/{_key?}/{_date}/{_emppro_id?}/{_emmpro_com_id?}")]
        [Route("CurrentSalaries")]
        public async Task<IActionResult> CurrentSalaries([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetCurrentSalariesAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //CurrentSalaries End

        //Salary Register Staff Worker
        [HttpPost]
        [Route("SalaryRegisterWorker")]
        public async Task<IActionResult> SalaryRegisterWorker([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetSalaryRegisterWorkerAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //Salary Register Staff Start
        [HttpPost]
        [Route("SalaryRegisterStaff")]
        public async Task<IActionResult> SalaryRegisterStaff([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetSalaryRegisterStaffAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        //Leave Start

        [HttpPost]
        [Route("Leave")]
        public async Task<IActionResult> Leave([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {
            string _TokenString=HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetLeaveAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);

        }
        //Leave End

        [HttpPost]
        [Route("SalaryPaySlip")]
        public async Task<IActionResult> SalaryPaySlip([FromBody] PayrollReportCreteria _PayrollReportCreteria)
        {

            string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();
            var result = await IPayrollReportSevicesRepository.GetSalaryPaySlipAsync(_TokenString, _PayrollReportCreteria);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}