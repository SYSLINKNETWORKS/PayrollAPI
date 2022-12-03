using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;

namespace TWP_API_Payroll.Generic
{

    public class ErrorLog
    {


        public async Task<string> LogError(string ErrorId, string ErrorDescription, string ErrorType, string StackTrace, ClaimsPrincipal _User)
        {

            string _CompanyId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.CompanyId.ToString())?.Value.ToString();
            string _UserId = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.UserId.ToString())?.Value.ToString();


            var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", false)
    .Build();
            var _Key = configuration["ConnectionStrings:Key"];



            ErrorLogViewAddModel _ErrorLogViewAddModel = new ErrorLogViewAddModel();
            _ErrorLogViewAddModel.CompanyId = _CompanyId;
            _ErrorLogViewAddModel.BranchId = _CompanyId;
            _ErrorLogViewAddModel.UserId = _UserId;
            _ErrorLogViewAddModel.Description = ErrorDescription;
            _ErrorLogViewAddModel.StackTrace = StackTrace;
            _ErrorLogViewAddModel.Type = ErrorType;
            _ErrorLogViewAddModel.Key = _Key;

            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Log/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            var url = "Log";
            var company = System.Text.Json.JsonSerializer.Serialize(_ErrorLogViewAddModel);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            HttpResponseMessage response = await client.PostAsync(url, requestContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = await response.Content.ReadAsByteArrayAsync();

                ApiResponse _api = System.Text.Json.JsonSerializer.Deserialize<ApiResponse>(content);

                return _api.message.ToString();
            }
            return response.ReasonPhrase;

        }
        public async Task<ApiResponse> ServiceName()
        {
            ApiResponse apiResponse = new ApiResponse();

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", false)
              .Build();
            var _Key = configuration["ConnectionStrings:Key"];


            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Log/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
             client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/MSServiceById";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    LovServicesViewModel _LovServicesViewModel = new LovServicesViewModel();

                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    var entities = JObject.Parse(_JsonData).SelectToken("data");
                    _LovServicesViewModel = JsonConvert.DeserializeObject<LovServicesViewModel>(entities.ToString());
                    _ApiResponse.data = _LovServicesViewModel;
                    apiResponse = _ApiResponse;
                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Invalid Service";
            return apiResponse;

        }
    }
}