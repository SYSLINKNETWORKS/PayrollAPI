using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Smo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;


namespace TWP_API_Payroll.Repository
{
    public interface IVersionSevicesRepository
    {
        Task<ApiResponse> GetVersionAsync(ClaimsPrincipal _User);
    }
    public class VersionSevicesRepository : IVersionSevicesRepository
    {
        private readonly DataContext _context = null;
        private ErrorLog _ErrorLog = new ErrorLog();

        public VersionSevicesRepository(DataContext context)
        {
            _context = context;
        }


        public async Task<ApiResponse> GetVersionAsync(ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            try
            {
                string _version =GetType().Assembly.GetName().Version.ToString();
                if (_version == "")
                {
                    apiResponse.statusCode = StatusCodes.Status404NotFound.ToString();
                    apiResponse.message = "Version Not Found";
                    return apiResponse;
                }

                apiResponse.statusCode = StatusCodes.Status200OK.ToString();
                apiResponse.message = _version;
                return apiResponse;
            }
            catch (Exception e)
            {

                string innerexp = e.InnerException == null ? e.Message : e.Message + " Inner Error : " + e.InnerException.ToString();
                string _ErrorId = await _ErrorLog.LogError("", innerexp, Enums.ErrorType.Error.ToString(), e.StackTrace, _User);
                apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
                apiResponse.message = _ErrorId;
                return apiResponse;

            }
        }
    }
}