
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
    public class VersionController : ControllerBase
    {
        private readonly IVersionSevicesRepository IVersionSevicesRepository = null;
        public VersionController(IVersionSevicesRepository _IVersionSevicesRepository)
        {
            IVersionSevicesRepository = _IVersionSevicesRepository;
        }

        ///<summary>
        ///Get Version
        ///</summary>
        [HttpGet]
        public Task<ApiResponse> GetVersion()
        {
            return IVersionSevicesRepository.GetVersionAsync(User);
        }
        //Get Version End

        

    }

}