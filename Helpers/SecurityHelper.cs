using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;

namespace TWP_API_Payroll.Helpers
{
    public class SecurityHelper
    {
        //Encrypt Password Start
        public SecurityHelper()
        {

        }


        //Menu Permission
        public async Task<ApiResponse> UserMenuPermissionAsync(Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();

            var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", false)
        .Build();

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("MenuId", _MenuId.ToString());
            client.DefaultRequestHeaders.Add("Key", _Key);// _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString());
            var url = "Microservices/MenuPermission";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    GetUserPermissionViewModel _GetUserPermissionViewModel = new GetUserPermissionViewModel();

                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    var entities = JObject.Parse(_JsonData).SelectToken("data");
                    _GetUserPermissionViewModel = JsonConvert.DeserializeObject<GetUserPermissionViewModel>(entities.ToString());
                    _ApiResponse.data = _GetUserPermissionViewModel;
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Invalid Permission";
            return apiResponse;
        }



        public async Task<ApiResponse> KeyValidation(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .Build();
            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/UserKeyVerification";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Record not found Key Validation API";
            return apiResponse;
        }
        //User Info
        public async Task<ApiResponse> UserInfo(string _TokenString)
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("Authorization", _TokenString);
            var url = "Auth/UserLoginInfo";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    UserLoginInfoBaseModel _UserLoginInfoBaseModel = new UserLoginInfoBaseModel();

                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    var entities = JObject.Parse(_JsonData).SelectToken("data");
                    _UserLoginInfoBaseModel = JsonConvert.DeserializeObject<UserLoginInfoBaseModel>(entities.ToString());
                    _ApiResponse.data = _UserLoginInfoBaseModel;
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Invalid Permission";
            return apiResponse;
        }
        public async Task<ApiResponse> UserInfoKey(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/UserLoginInfo";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    UserLoginInfoBaseModel _UserLoginInfoBaseModel = new UserLoginInfoBaseModel();

                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    var entities = JObject.Parse(_JsonData).SelectToken("data");
                    _UserLoginInfoBaseModel = JsonConvert.DeserializeObject<UserLoginInfoBaseModel>(entities.ToString());
                    _ApiResponse.data = _UserLoginInfoBaseModel;
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Invalid Permission";
            return apiResponse;
        }


        public async Task<ApiResponse> BranchInfoAsync(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();

            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/BranchInfo";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    List<BranchViewModel> _BranchViewModel = new List<BranchViewModel>();

                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    var entities = JObject.Parse(_JsonData).SelectToken("data");
                    _BranchViewModel = JsonConvert.DeserializeObject<List<BranchViewModel>>(entities.ToString());
                    _ApiResponse.data = _BranchViewModel;
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Branch not found";
            return apiResponse;
        }

        public async Task<ApiResponse> GetFinancialYearAsync(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Auth/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/FinancialYear";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    _ApiResponse.data = JsonConvert.DeserializeObject<MSFinancialYearServicesViewModel>(_ApiResponse.data.ToString());
                    apiResponse = _ApiResponse;

                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Record not found";
            return apiResponse;

        }
        public ApiResponse CheckPermission(GetUserPermissionViewModel _UserMenuPermission, DateTime _DateToCheck)
        {
            ApiResponse apiResponse = new ApiResponse();

            if (!(_DateToCheck >= _UserMenuPermission.PermissionDateFrom && _DateToCheck <= _UserMenuPermission.PermissionDateTo))
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                apiResponse.message = "Permission date must be between " + _UserMenuPermission.PermissionDateFrom.ToString("dd-MMM-yyyy") + " and " + _UserMenuPermission.PermissionDateTo.ToString("dd-MMM-yyyy");
                return apiResponse;
            }

            if (!(_DateToCheck >= _UserMenuPermission.YearStartDate && _DateToCheck <= _UserMenuPermission.YearEndDate))
            {
                apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                apiResponse.message = "Financial year date must be between " + _UserMenuPermission.YearStartDate.ToString("dd-MMM-yyyy") + " and " + _UserMenuPermission.YearEndDate.ToString("dd-MMM-yyyy");
                return apiResponse;
            }


            apiResponse.statusCode = StatusCodes.Status200OK.ToString();
            return apiResponse;

        }

    }
}
