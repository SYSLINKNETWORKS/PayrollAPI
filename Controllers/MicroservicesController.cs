using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Services;

namespace TWP_API_Payroll.Controllers
{
    ///<summary>
    ///Micro Service
    ///</summary>
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MicroservicesController : ControllerBase
    {
        private readonly IMicroservices _IMicroservices = null;
        public MicroservicesController(IMicroservices IMicroservices)
        {
            _IMicroservices = IMicroservices;
        }

        ///<summary>
        ///Get Deparment All 
        ///</summary>
        [HttpGet]
        [Route("MSDepartment")]
        public async Task<IActionResult> MSDepartment([FromHeader] string Key)
        {
            try
            {
                ApiResponse result = await _IMicroservices.MSDepartmentsAsync(Key);
                if (result.statusCode == StatusCodes.Status200OK.ToString())
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }
        //Get End

        ///<summary>
        ///Get All Employees
        ///</summary>
        [HttpGet]
        [Route("MSEmployees")]
        public async Task<IActionResult> MSEmployees([FromHeader] string Key)
        {
            try
            {
                ApiResponse result = await _IMicroservices.MSEmployeesAsync(Key);
                if (result.statusCode == StatusCodes.Status200OK.ToString())
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }
        //Get End
        ///<summary>
        ///Get Employee from Respective Employee ID
        ///</summary>
        [HttpGet]
        [Route("MSEmployeeById")]
        public async Task<IActionResult> MSEmployeeById([FromHeader] string Key, [FromHeader] Guid _Id)
        {
            try
            {
                ApiResponse result = await _IMicroservices.MSEmployeeByIdAsync(Key, _Id);
                if (result.statusCode == StatusCodes.Status200OK.ToString())
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }

        ///<summary>
        ///Get Employee from Respective Employee ID Reporting
        ///</summary>
        [HttpGet]
        [Route("MSEmployeeByIdReporting")]
        public async Task<IActionResult> MSEmployeeByIdReporting([FromHeader] string Key, [FromHeader] Guid _Id)
        {
            try
            {
                ApiResponse result = await _IMicroservices.MSEmployeeByIdReportingAsync(Key, _Id);
                if (result.statusCode == StatusCodes.Status200OK.ToString())
                {
                    return Ok(result);
                }
                return BadRequest(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }
        ///<summary>
        ///Get Employee Attendance for specific date
        ///</summary>
        [HttpGet]
        [Route("MSLoginAttendance")]
        public async Task<IActionResult> MSLoginAttendance([FromHeader] string Passphase, [FromHeader] string EmployeeId, [FromHeader] string LoginDate)
        {
            try
            {
                ApiResponse result = await _IMicroservices.MSLoginAttendanceAsync(Passphase, EmployeeId, LoginDate);
                // if (result.statusCode == StatusCodes.Status200OK.ToString())
                // {
                return Ok(result);
                // }
                // return BadRequest(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message.ToString() + innerexp);
            }
        }

    }

}