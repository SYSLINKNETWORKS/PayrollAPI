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
    public class PayrollDashboardController : ControllerBase
    {

        private readonly IPayrollDashBoardSevicesRepository IPayrollDashBoardSevicesRepository = null;
        public PayrollDashboardController(IPayrollDashBoardSevicesRepository _IPayrollDashBoardSevicesRepository)
        {
            IPayrollDashBoardSevicesRepository = _IPayrollDashBoardSevicesRepository;
        }

        //Get Attendance Machine IP Start
        [HttpGet]
        [Route("GetAttendanceMachineIp")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAttendanceMachineIp([FromHeader] Guid GroupId)
        {

            try
            {
                var result = await IPayrollDashBoardSevicesRepository.GetAttendanceMachineIpAsync( GroupId);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message + innerexp);
            }

        }
        //Get Attendance Machine IP End

        //Insert Machine Attendance Start
        [HttpPost]
        [Route("AttendanceMachinePost")]
        [AllowAnonymous]
        //  [AllowAnonymous]
        public async Task<IActionResult> AttendanceMachinePost([FromBody] CheckInOutMachineListModel _CheckInOutMachineListModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await IPayrollDashBoardSevicesRepository.PostCheckInOutAsync(_CheckInOutMachineListModel);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    return Ok(result);
                }
                else { return BadRequest("Invalid Model"); }
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message + innerexp);
            }

        }
        //Insert Machine Attendance End

        //Process Attendance Start
        [HttpPost]
        [Route("ProcessAttendance")]
        //  [AllowAnonymous]
        public async Task<IActionResult> ProcessAttendance([FromBody] CheckAttendanceBaseModel _CheckAttendanceBaseModel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await IPayrollDashBoardSevicesRepository.ProcessAttendanceAsync( _CheckAttendanceBaseModel);
                    if (result == null)
                    {
                        return NotFound();
                    }
                    return Ok(result);
                }
                else { return BadRequest("Invalid Model"); }
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message + innerexp);
            }

        }
        //Process Attendance End

        //Get Attendance Start
        [HttpPost]
        [Route("GetAttendance")]
        public async Task<IActionResult> GetAttendance(DashboardCreteriaViewModel _DashboardCreteriaViewModel)
        {

            try
            {

                string _TokenString = HttpContext.Request.Headers["Authorization"].ToString();

                var result = await IPayrollDashBoardSevicesRepository.GetAttendanceAsync(User, _TokenString, _DashboardCreteriaViewModel.date);
                if (result == null)
                {
                    return NotFound();
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                string innerexp = "";
                if (e.InnerException != null)
                {
                    innerexp = " Inner Error : " + e.InnerException.ToString();
                }
                return BadRequest(e.Message + innerexp);
            }

        }
        //Get Attendance End

        //Total MAP Start
        [HttpPost]
        [Route("TotalInOutMap")]
        public async Task<IActionResult> TotalInOutMap(DashboardCreteriaViewModel _DashboardCreteriaViewModel)
        {
            //   DateTime _Date = new DateTime (2021, 5, 29);
            var result = await IPayrollDashBoardSevicesRepository.GetInOutMapAsync(User, _DashboardCreteriaViewModel.date);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        //Total Get Location Start
        [HttpPost]
        [Route("GetLocation")]
        public async Task<IActionResult> GetLocation(DashboardCreteriaViewModel _DashboardCreteriaViewModel)
        {
            //   DateTime _Date = new DateTime (2021, 5, 29);
            var result = await IPayrollDashBoardSevicesRepository.GetLocationAsync(User, _DashboardCreteriaViewModel);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        // //Total totalInOutAtt Start
        // [HttpPost]
        // [Route ("TotalInOut")]
        // public async Task<IActionResult> TotalInOut (DashboardCreteriaViewModel _DashboardCreteriaViewModel) {
        //     //   DateTime _Date = new DateTime (2021, 5, 29);
        //     var result = await IPayrollDashBoardSevicesRepository.GetInOutAsync (User, _DashboardCreteriaViewModel.date);
        //     if (result == null) {
        //         return NotFound ();
        //     }
        //     return Ok (result);
        // }

        // //Total Inout Map Start
        // [HttpGet]
        // [Route ("~/DashBoard/TotalInOutMap/{_key?}/{_Date?}")]
        // public string TotalInOutMap (string _key, string _Date) {
        //     System.Data.DataTable dttblgrn_sel = new System.Data.DataTable ();
        //     dttblgrn_sel.Columns.Add ("Remarks", typeof (string));
        //     dttblgrn_sel.Columns.Add ("status", typeof (int));
        //     dttblgrn_sel.Columns.Add ("Result", typeof (object));
        //     dynamic dataResult = new { Token = _key };

        //     try {
        //         string[] strusr = func.checkuser (dataResult);
        //         if (Convert.ToBoolean (strusr[0])) {
        //             string _com_id = strusr[2], _br_id = strusr[3], _m_yr_id = strusr[4];
        //             bool _usrckbr = Convert.ToBoolean (strusr[17]);

        //             cmdtxt = " select v_emppro.emppro_id, emppro_nam_rpt,dpt_id,dpt_nam, memp_sub_nam,checkdate " +
        //                 " from v_emppro" +
        //                 " inner join checkinout on v_emppro.emppro_macid = checkinout.userid" +
        //                 " where v_emppro.com_id = '" + _com_id + "' and emppro_attexp = 0 and latitude<>0" +
        //                 " and checkdate='" + Convert.ToDateTime (_Date).ToShortDateString () + "'" +
        //                 " group by v_emppro.emppro_id, emppro_nam_rpt,dpt_id,dpt_nam, memp_sub_nam,checkdate" +
        //                 " order by emppro_nam_rpt asc,checkdate desc";

        //             DataSet dsgrn = func.dsfunc (cmdtxt);
        //             if (dsgrn.Tables[0].Rows.Count > 0) {
        //                 dttblgrn_sel.Rows.Add ("OK", dsgrn.Tables[0].Rows.Count.ToString (), dsgrn.Tables[0]);
        //             } else {
        //                 dttblgrn_sel.Rows.Add ("Record not found", 0, null);
        //             }
        //         } else {
        //             dttblgrn_sel.Rows.Add ("Invalid User", 2, null);
        //         }

        //     } catch (Exception e) {
        //         string innerexp = "";
        //         if (e.InnerException != null) {
        //             innerexp = " Inner Error : " + e.InnerException.ToString ();
        //         }
        //         dttblgrn_sel.Rows.Add ("Error : " + e.Message + innerexp, 0, null);
        //     }
        //     // JSONResult = JsonConvert.SerializeObject(dttblgrn_sel);
        //     // return JsonConvert.DeserializeObject<object>(JSONResult);
        //     return JsonConvert.SerializeObject (dttblgrn_sel);
        // }
        // //Total InOut Map End

        // private void insrec (string empproid) {
        //     cmd.Parameters.Clear ();
        //     cmd.Connection = func.con ();
        //     cmd.CommandType = CommandType.StoredProcedure;
        //     cmd.CommandText = "sp_night_shift";

        //     param = cmd.Parameters.AddWithValue ("emppro_id", empproid);
        //     param = cmd.Parameters.AddWithValue ("dt1", date.ToShortDateString ());
        //     param = cmd.Parameters.AddWithValue ("dt2", date.ToShortDateString ());

        //     func.cn.Open ();
        //     func.trans = func.cn.BeginTransaction ();
        //     cmd.Transaction = func.trans;
        //     cmd.ExecuteNonQuery ();
        //     func.trans.Commit ();
        //     func.cn.Close ();

        // }
        // //Get Location Start
        // [HttpGet]
        // [Route ("~/DashBoard/EmployeeGetLocation/{_emppro_id?}/{_date?}/{_key?}")]
        // public string EmployeeGetLocation (int _emppro_id, DateTime _date, string _key) {
        //     //////var JSONResult = "";
        //     System.Data.DataTable dttblgrn_sel = new System.Data.DataTable ();
        //     dttblgrn_sel.Columns.Add ("Remarks", typeof (string));
        //     dttblgrn_sel.Columns.Add ("status", typeof (int));
        //     dttblgrn_sel.Columns.Add ("Result", typeof (object));
        //     dynamic dataResult = new { Token = _key };

        //     try {
        //         string[] strusr = func.checkuser (dataResult);
        //         if (Convert.ToBoolean (strusr[0])) {
        //             string _com_id = strusr[2], _br_id = strusr[3], _m_yr_id = strusr[4];

        //             cmdtxt = "select emppro_nam as [Name],CHECKTIME,latitude,longitude,address from checkinout inner join m_emppro on checkinout.userid=m_emppro.emppro_macid where latitude<>0 and emppro_id=" + _emppro_id + " and checkdate='" + _date + "'";
        //             DataSet dsgrn = func.dsfunc (cmdtxt);
        //             if (dsgrn.Tables[0].Rows.Count > 0) {

        //                 dttblgrn_sel.Rows.Add ("OK", dsgrn.Tables[0].Rows.Count.ToString (), dsgrn.Tables[0]);
        //             } else {
        //                 dttblgrn_sel.Rows.Add ("Record not found", 0, null);
        //             }
        //         } else {
        //             dttblgrn_sel.Rows.Add ("Invalid User", 2, null);
        //         }

        //     } catch (Exception e) {
        //         string innerexp = "";
        //         if (e.InnerException != null) {
        //             innerexp = " Inner Error : " + e.InnerException.ToString ();
        //         }
        //         dttblgrn_sel.Rows.Add ("Error : " + e.Message + innerexp, 0, null);
        //     }
        //     return JsonConvert.SerializeObject (dttblgrn_sel);

        // }
        // //Total Location End

        // //Total UpdateApproval Start
        // [HttpGet]
        // [Route ("~/DashBoard/UpdateApproval/{_key?}/{Details?}/{Unapprovedbtn?}/{approvedbtn?}")]
        // public string UpdateApproval (string _key, string Details, string Unapprovedbtn, string approvedbtn) {
        //     System.Data.DataTable dttblgrn_sel = new System.Data.DataTable ();
        //     dttblgrn_sel.Columns.Add ("Remarks", typeof (string));
        //     dttblgrn_sel.Columns.Add ("status", typeof (int));
        //     dttblgrn_sel.Columns.Add ("Result", typeof (object));
        //     dynamic dataResult = new { Token = _key };

        //     string result = string.Empty;
        //     try {
        //         string[] strusr = func.checkuser (dataResult);
        //         if (Unapprovedbtn == "Unapprovedbtn") {
        //             if (Convert.ToBoolean (strusr[0])) {
        //                 string _com_id = strusr[2], _br_id = strusr[3], _m_yr_id = strusr[4];
        //                 bool _usrckbr = Convert.ToBoolean (strusr[17]);

        //                 string[] recArray = Details.Split (",");
        //                 foreach (var rec in recArray) {
        //                     string[] spArray = rec.Split ("|");
        //                     int id = Convert.ToInt32 (spArray[0]);
        //                     string getdate = spArray[1];
        //                     string gettime = spArray[2];
        //                     cmd.Connection = func.con ();
        //                     cmd.CommandText = "update CHECKINOUT set check_app='0' where CHECKTIME ='" + gettime + "' and checkdate='" + getdate + "'  and USERID='" + id + "' and check_typ='U'  ";
        //                     func.cn.Open ();
        //                     func.trans = func.cn.BeginTransaction ();
        //                     cmd.Transaction = func.trans;
        //                     cmd.ExecuteNonQuery ();
        //                     func.trans.Commit ();
        //                     func.cn.Close ();
        //                     result = getdate;
        //                 }
        //             }
        //         } else {
        //             string _com_id = strusr[2], _br_id = strusr[3], _m_yr_id = strusr[4];
        //             bool _usrckbr = Convert.ToBoolean (strusr[17]);

        //             string[] recArray = Details.Split (",");
        //             foreach (var rec in recArray) {
        //                 string[] spArray = rec.Split ("|");
        //                 int id = Convert.ToInt32 (spArray[0]);
        //                 string getdate = spArray[1];
        //                 string gettime = spArray[2];
        //                 cmd.Connection = func.con ();
        //                 cmd.CommandText = "update CHECKINOUT set check_app='1' where CHECKTIME ='" + gettime + "' and checkdate='" + getdate + "'  and USERID='" + id + "' and check_typ='U'  ";
        //                 func.cn.Open ();
        //                 func.trans = func.cn.BeginTransaction ();
        //                 cmd.Transaction = func.trans;
        //                 cmd.ExecuteNonQuery ();
        //                 func.trans.Commit ();
        //                 func.cn.Close ();
        //                 result = getdate;
        //             }
        //         }

        //     } catch (Exception e) {
        //         result = "Error" + e;
        //         func.trans.Rollback ();
        //     }
        //     // JSONResult = JsonConvert.SerializeObject(dttblgrn_sel);
        //     // return JsonConvert.DeserializeObject<object>(JSONResult);
        //     return JsonConvert.SerializeObject (result);
        // }

        // //Total UpdateApproval End

    }

}