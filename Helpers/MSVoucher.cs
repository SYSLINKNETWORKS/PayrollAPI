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
using TWP_API.ViewModels;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.ViewModels;

namespace TWP_API.Helpers
{
    public class MSVoucher
    {

        string _connectionString = "";
        DataContext _context;
        SecurityHelper _SecurityHelper = new SecurityHelper();


        public MSVoucher()
        {
            var configuration = new ConfigurationBuilder()
                                      .SetBasePath(Directory.GetCurrentDirectory())
                                      .AddJsonFile("appsettings.json", false)
                                      .Build();

            _connectionString = configuration.GetConnectionString("Connection").ToString();

            var options = new DbContextOptionsBuilder<DataContext>().UseSqlServer(_connectionString).Options;
            _context = new DataContext(options);

        }

        public async Task<ApiResponse> GetCurrency(string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();
            var configuration = new ConfigurationBuilder()
           .SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json", false)
           .Build();
            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = "Microservices/MSCurrency";
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    _ApiResponse.data = JsonConvert.DeserializeObject<List<MSCurrencyViewModel>>(_ApiResponse.data.ToString());
                    apiResponse = _ApiResponse;

                }
                return apiResponse;
            }
            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Record not found Currency API";

            return apiResponse;
        }
        // public async Task<ApiResponse> AddAdvanceVoucher(AdvanceVoucherViewModel _Voucher, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();
        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not create";
        //     return apiResponse;


        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // var url = "Microservices/MSAddPayrollAdvanceVoucher";
        //     // var company = System.Text.Json.JsonSerializer.Serialize(_Voucher);
        //     // var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

        //     // HttpResponseMessage response = await client.PostAsync(url, requestContent);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         _ApiResponse.data = JsonConvert.DeserializeObject<VoucherResponse>(_ApiResponse.data.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }

        // public async Task<ApiResponse> UpdateAdvanceVoucher(AdvanceVoucherViewModel _Voucher, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();

        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not update";
        //     return apiResponse;


        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // var url = "Microservices/MSUpdatePayrollAdvanceVoucher";
        //     // var company = System.Text.Json.JsonSerializer.Serialize(_Voucher);
        //     // var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

        //     // HttpResponseMessage response = await client.PutAsync(url, requestContent);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         _ApiResponse.data = JsonConvert.DeserializeObject<VoucherResponse>(_ApiResponse.data.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }
        // public async Task<ApiResponse> DeleteAdvanceVoucher(Guid _VoucherNo, string _CompanyId, string _UserId, string _UserName, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();
        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not delete";
        //     return apiResponse;



        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // client.DefaultRequestHeaders.Add("CompanyId", _CompanyId);
        //     // client.DefaultRequestHeaders.Add("UserId", _UserId);
        //     // client.DefaultRequestHeaders.Add("UserName", _UserName);
        //     // client.DefaultRequestHeaders.Add("VoucherNo", _VoucherNo.ToString());
        //     // var url = "Microservices/MSDeletePayrollAdvanceVoucher";

        //     // HttpResponseMessage response = await client.DeleteAsync(url);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }
        // public async Task<ApiResponse> AddLoanVoucher(AdvanceVoucherViewModel _Voucher, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();
        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not create";
        //     return apiResponse;


        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // var url = "Microservices/MSAddPayrollLoanVoucher";
        //     // var company = System.Text.Json.JsonSerializer.Serialize(_Voucher);
        //     // var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

        //     // HttpResponseMessage response = await client.PostAsync(url, requestContent);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         _ApiResponse.data = JsonConvert.DeserializeObject<VoucherResponse>(_ApiResponse.data.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }

        // public async Task<ApiResponse> UpdateLoanVoucher(AdvanceVoucherViewModel _Voucher, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();

        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not update";
        //     return apiResponse;


        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // var url = "Microservices/MSUpdatePayrollLoanVoucher";
        //     // var company = System.Text.Json.JsonSerializer.Serialize(_Voucher);
        //     // var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

        //     // HttpResponseMessage response = await client.PutAsync(url, requestContent);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         _ApiResponse.data = JsonConvert.DeserializeObject<VoucherResponse>(_ApiResponse.data.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }
        // public async Task<ApiResponse> DeleteLoanVoucher(Guid _VoucherNo, string _CompanyId, string _UserId, string _UserName, string _Key)
        // {
        //     ApiResponse apiResponse = new ApiResponse();
        //     apiResponse.statusCode = StatusCodes.Status200OK.ToString();
        //     apiResponse.message = "voucher will not delete";
        //     return apiResponse;



        //     // var configuration = new ConfigurationBuilder()
        //     //             .SetBasePath(Directory.GetCurrentDirectory())
        //     //             .AddJsonFile("appsettings.json", false)
        //     //             .Build();

        //     // HttpClient client = new HttpClient();
        //     // var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
        //     // client.BaseAddress = new Uri(_BaseUri);
        //     // client.DefaultRequestHeaders.Accept.Clear();
        //     // client.DefaultRequestHeaders.Add("Key", _Key);
        //     // client.DefaultRequestHeaders.Add("CompanyId", _CompanyId);
        //     // client.DefaultRequestHeaders.Add("UserId", _UserId);
        //     // client.DefaultRequestHeaders.Add("UserName", _UserName);
        //     // client.DefaultRequestHeaders.Add("VoucherNo", _VoucherNo.ToString());
        //     // var url = "Microservices/MSDeletePayrollLoanVoucher";

        //     // HttpResponseMessage response = await client.DeleteAsync(url);
        //     // if (response.StatusCode == HttpStatusCode.OK)
        //     // {
        //     //     var _JsonData = response.Content.ReadAsStringAsync().Result;
        //     //     if (_JsonData != null)
        //     //     {
        //     //         ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
        //     //         apiResponse = _ApiResponse;

        //     //     }
        //     //     return apiResponse;
        //     // }
        //     // apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
        //     // apiResponse.message = "Record not found";
        //     // return apiResponse;

        // }

        public async Task<ApiResponse> AddSalaryVoucher(SalaryVoucherViewModel _Voucher, string _Key)
        {

            ApiResponse apiResponse = new ApiResponse();


            apiResponse = await _SecurityHelper.KeyValidation(_Key);
            if (apiResponse.statusCode != StatusCodes.Status200OK.ToString())
            {
                return apiResponse;
            }

            var configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", false)
                   .Build();

            var apiResponseCurrency = await GetCurrency(_Key);
            var _MSCurrencyViewModel = (List<MSCurrencyViewModel>)apiResponseCurrency.data;


            var _CurrencyTable = _MSCurrencyViewModel.Where(x => x.Type != Enums.ColumnType.S.ToString()).FirstOrDefault();

            VoucherMasterAddModel _VoucherMasterAddModel = new VoucherMasterAddModel();
            _VoucherMasterAddModel.VoucherDetailViewModel = new List<VoucherDetailViewModel>();

            _VoucherMasterAddModel.No = string.IsNullOrEmpty(_Voucher.VoucherNo) ? Guid.Empty : new Guid(_Voucher.VoucherNo);
            _VoucherMasterAddModel.CurrencyId = _CurrencyTable.Id;
            _VoucherMasterAddModel.Date = _Voucher.Date;
            _VoucherMasterAddModel.ExchangeRate = 1;
            _VoucherMasterAddModel.TransactionDate = _Voucher.Date;
            _VoucherMasterAddModel.VoucherTypeNo = "05";
            _VoucherMasterAddModel.Remarks = "";
            _VoucherMasterAddModel.Type = Enums.ColumnType.S.ToString();


            var _SystemTable = await _context.CfSystems.Where(x => x.CompanyId == new Guid(_Voucher.CompanyId)).FirstOrDefaultAsync();
            List<VoucherDetailViewModel> _VoucherDetailViewModel = new List<VoucherDetailViewModel>();
            string _Narration = "";
            Guid _SalaryExpenseAccount = new Guid();
            Guid _SalaryPayableAccount = new Guid();
            Guid _PayrollAdvanceAccount = new Guid();
            Guid _PayrollLoanAccount = new Guid();
            Guid _PayrollInsuranceAccount = new Guid();
            Guid _PayrollIncomeTaxAccount = new Guid();
            Guid _OverTimeExpenseAccount = new Guid();
            Guid _OverTimePayableAccount = new Guid();

            if (_Voucher.Category == Enums.Payroll.Staff.ToString())
            {
                _SalaryExpenseAccount = _SystemTable.PayrollAdminSalaryExpenseAccount;
                _SalaryPayableAccount = _SystemTable.PayrollAdminSalaryPayableAccount;
                _PayrollAdvanceAccount = _SystemTable.PayrollAdminAdvanceAccount;
                _PayrollLoanAccount = _SystemTable.PayrollAdminLoanAccount;
                _PayrollInsuranceAccount = _SystemTable.PayrollAdminInsuranceAccount;
                _PayrollIncomeTaxAccount = _SystemTable.PayrollAdminIncomeTaxAccount;
                _OverTimeExpenseAccount = _SystemTable.PayrollAdminOverTimeExpenseAccount;
                _OverTimePayableAccount = _SystemTable.PayrollAdminOverTimePayableAccount;

            }
            else if (_Voucher.Category == Enums.Payroll.Worker.ToString())
            {
                _SalaryExpenseAccount = _SystemTable.PayrollManufacturingExpenseAccount;
                _SalaryPayableAccount = _SystemTable.PayrollManufacturingPayableAccount;
                _PayrollAdvanceAccount = _SystemTable.PayrollManufacturingAdvanceAccount;
                _PayrollLoanAccount = _SystemTable.PayrollManufacturingLoanAccount;
                _PayrollInsuranceAccount = _SystemTable.PayrollManufacturingInsuranceAccount;
                _PayrollIncomeTaxAccount = _SystemTable.PayrollManufacturingIncomeTaxAccount;
                _OverTimeExpenseAccount = _SystemTable.PayrollManufacturingOverTimeExpenseAccount;
                _OverTimePayableAccount = _SystemTable.PayrollManufacturingOverTimePayableAccount;
            }

            //Debit

            //Expense
            _Narration = "To record salary expense for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                DebitAmount = _Voucher.Amount,
                AccountNo = _SalaryExpenseAccount,
            });

            //Credit 
            //Advance
            _Narration = "To adjust advance against salary for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.AdvanceAmount,
                AccountNo = _PayrollAdvanceAccount,
            });
            //Loan
            _Narration = "To adjust loan against salary for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.LoanAmount,
                AccountNo = _PayrollLoanAccount,
            });
            //Takaful
            _Narration = "To adjust takaful contribution for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.TakafulAmount,
                AccountNo = _PayrollInsuranceAccount,
            });
            //Income Tax
            _Narration = "To adjust I.Tax from employees salary for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.IncomeTaxAmount,
                AccountNo = _PayrollIncomeTaxAccount,
            });
            _Narration = "To record salary expense for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.NetAmount,
                AccountNo = _SalaryPayableAccount,
            });


            //Debit
            //Over Time Expense
            _Narration = "To record overtime expense for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                DebitAmount = _Voucher.OverTimeAmount,
                AccountNo = _OverTimeExpenseAccount,
            });

            //Credit
            //OverTime Payable
            _Narration = "To record overtime expense for the m/o " + _Voucher.Date.ToString("MMM-yyyy");
            _VoucherDetailViewModel.Add(new VoucherDetailViewModel
            {
                Naration = _Narration,
                CreditAmount = _Voucher.OverTimeAmount,
                AccountNo = _OverTimePayableAccount,
            });

            _VoucherMasterAddModel.VoucherDetailViewModel.AddRange(_VoucherDetailViewModel);


            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Key", _Key);
            var url = string.IsNullOrEmpty(_Voucher.VoucherNo) ? "Microservices/MSAddVoucher" : "Microservices/MSUpdateVoucher";
            var company = System.Text.Json.JsonSerializer.Serialize(_VoucherMasterAddModel);
            var requestContent = new StringContent(company, Encoding.UTF8, "application/json");

            HttpResponseMessage response = string.IsNullOrEmpty(_Voucher.VoucherNo) ? await client.PostAsync(url, requestContent) : await client.PutAsync(url, requestContent);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var _JsonData = response.Content.ReadAsStringAsync().Result;
                if (_JsonData != null)
                {
                    ApiResponse _ApiResponse = JsonConvert.DeserializeObject<ApiResponse>(_JsonData.ToString());
                    _ApiResponse.data = JsonConvert.DeserializeObject<VoucherResponse>(_ApiResponse.data.ToString());
                    apiResponse = _ApiResponse;

                    var _VoucherResponse = (VoucherResponse)apiResponse.data;

                    await CheckApprovedUpdateVoucher(_VoucherResponse.No, true, true, _Key);

                }
                return apiResponse;
            }

            apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
            apiResponse.message = "Voucher not create : " + response.ReasonPhrase;
            return apiResponse;

        }

        public async Task<ApiResponse> CheckApprovedUpdateVoucher(Guid _VoucherNo, bool _Check, bool _Approved, string _Key)
        {
            ApiResponse apiResponse = new ApiResponse();


            var configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false)
                        .Build();


            HttpClient client = new HttpClient();
            var _BaseUri = configuration["ConnectionStrings:GWAPI"] + "/Accounts/v1/";
            client.BaseAddress = new Uri(_BaseUri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Key", _Key);
            client.DefaultRequestHeaders.Add("No", _VoucherNo.ToString());
            client.DefaultRequestHeaders.Add("Check", _Check.ToString());
            client.DefaultRequestHeaders.Add("Approved", _Approved.ToString());
            var url = "Microservices/MSUpdateCheckApproveVoucher";

            HttpResponseMessage response = await client.PutAsync(url, null);
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
            apiResponse.message = "voucher status cannot update";
            return apiResponse;

        }

    }
}
