// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Net;
// using System.Net.Http;
// using System.Web.Http;
// using System.Data;
//using System.Data.SqlClient;
// using TWP_API_Payroll.BLogic;
// using Newtonsoft.Json;
// using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
//using ICSharpCode.SharpZipLib.Zip;
//using System.IO.Compression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Repository;

namespace TWP_API_Payroll.Controllers
{

    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [Authorize]
    public class BackupDBController : ControllerBase
    {
        private readonly IBackupSevicesRepository IBackupSevicesRepository = null;
        public BackupDBController(IBackupSevicesRepository _IBackupSevicesRepository)
        {
            IBackupSevicesRepository = _IBackupSevicesRepository;
        }

        ///<summary>
        ///Create Backup
        ///</summary>
        [HttpPost]
        public Task<ApiResponse> CreateBackup()
        {
            return IBackupSevicesRepository.CreateDBBackupAsync(User);
        }
        //Create Backup End

        ///<summary>
        ///Show Backup
        ///</summary>
        [HttpGet]
        public Task<ApiResponse> ShowBackup()
        {
            return IBackupSevicesRepository.DBBackupShowAsync(User);
        }

        ///<summary>
        ///Delete File Respective File Name
        ///</summary>

        [HttpDelete]
        public Task<ApiResponse> DeleteBackup([FromHeader] string FileName)
        {
            return IBackupSevicesRepository.DBBackupDeleteAsync(User, FileName);
        }


        ///<summary>
        ///Download File Respective File Name
        ///</summary>
        [HttpGet]
        [Route("DownloadBackup")]
        public IActionResult DownloadBackup([FromHeader] string FileName)
        {

            var _FileStream = IBackupSevicesRepository.DBBackupDownloadAsync(User, FileName);
            if (_FileStream != null)
            {
                return _FileStream;
            }
            return NotFound("File not found");

        }

    }

}