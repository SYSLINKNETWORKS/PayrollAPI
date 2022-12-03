using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using TWP_API_Payroll.Processor;
using TWP_API_Payroll.ViewModels.Payroll;
using TWP_API_Payroll.Processor.Process.Payroll;
using TWP_API_Payroll.Repository;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Services;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(config =>
         {
             config.DefaultApiVersion = new ApiVersion(1, 0);
             config.AssumeDefaultVersionWhenUnspecified = true;
             config.ReportApiVersions = true;
         });
// services.AddIdentity<ApplicationUser, IdentityRole>(options =>
// {
//     options.Password.RequireDigit = true;
//     options.Password.RequireUppercase = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequireNonAlphanumeric = true;
//     options.Password.RequireLowercase = true;
//     options.Password.RequiredLength = 8;
//     options.Password.RequiredUniqueChars = 3;
//     options.Lockout.MaxFailedAccessAttempts = 5;

// }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication(auth =>
{
    auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            RequireExpirationTime = true,
            ValidIssuer = builder.Configuration["AuthSettings:Issuer"], //some string, normally web url,
                                                                        //ValidAudience = Configuration["AuthSettings:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["AuthSettings:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                SecurityHelper _SecurityHelper = new SecurityHelper();
                string _TokenString = context.HttpContext.Request.Headers["Authorization"].ToString();
                var _Token = _TokenString.Substring(7, _TokenString.Length - 7);
                var _Handler = new JwtSecurityTokenHandler();
                var _TokenDecode = _Handler.ReadJwtToken(_Token);
                string _Key = _TokenDecode.Audiences.ToList()[0].ToString();
                var _result = _SecurityHelper.KeyValidation(_Key).GetAwaiter().GetResult();
                if (_result.statusCode != StatusCodes.Status200OK.ToString()) { context.Fail("Unauthorized"); }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    context.Response.Headers.Add("Token-Expired", "true");
                }
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TWP API Payroll", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
    });
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
    });
});

//Microservices
builder.Services.AddScoped<IMicroservices, Microservices>();
//Backup
builder.Services.AddScoped<IBackupSevicesRepository, BackupSevicesRepository>();
//Backup
builder.Services.AddScoped<IVersionSevicesRepository, VersionSevicesRepository>();

//Payroll Start
builder.Services.AddScoped<IProcessor<AllowanceBaseModel>, AllowanceProcessor>();
builder.Services.AddScoped<IProcessor<AdvanceBaseModel>, AdvanceProcessor>();
builder.Services.AddScoped<IProcessor<HolidayBaseModel>, HolidayProcessor>();
builder.Services.AddScoped<IProcessor<DesignationBaseModel>, DesignationProcessor>();
builder.Services.AddScoped<IProcessor<DepartmentBaseModel>, DepartmentProcessor>();
builder.Services.AddScoped<IProcessor<RosterBaseModel>, RosterProcessor>();
builder.Services.AddScoped<IProcessor<RosterGroupBaseModel>, RosterGroupProcessor>();
builder.Services.AddScoped<IProcessor<EmployeeCategoryBaseModel>, EmployeeCategoryProcessor>();
builder.Services.AddScoped<IProcessor<MachineCompanyBaseModel>, MachineCompanyProcessor>();
builder.Services.AddScoped<IProcessor<AttendanceMachineBaseModel>, AttendanceMachineProcessor>();
builder.Services.AddScoped<IProcessor<AnnualLeavesBaseModel>, AnnualLeavesProcessor>();
builder.Services.AddScoped<IProcessor<InOutCategoryBaseModel>, InOutCategoryProcessor>();
builder.Services.AddScoped<IProcessor<EmployeeProfileBaseModel>, EmployeeProfileProcessor>();
builder.Services.AddScoped<IProcessor<EmployeeProfileDocumentBaseModel>, EmployeeProfileDocumentProcessor>();
builder.Services.AddScoped<IProcessor<LoanCategoryBaseModel>, LoanCategoryProcessor>();
builder.Services.AddScoped<IProcessor<IncomeTaxSlabEmployeeBaseModel>, IncomeTaxSlabEmployeeProcessor>();
builder.Services.AddScoped<IProcessor<LoanIssueBaseModel>, LoanIssueProcessor>();
builder.Services.AddScoped<IProcessor<LoanReceiveBaseModel>, LoanReceiveProcessor>();
builder.Services.AddScoped<IProcessor<SalaryAdditionDeductionBaseModel>, SalaryAdditionDeductionProcessor>();
builder.Services.AddScoped<IProcessor<SalaryBaseModel>, SalaryProcessor>();

builder.Services.AddScoped<IPayrollInOutEditorSevicesRepository, PayrollInOutEditorSevicesRepository>();
builder.Services.AddScoped<IPayrollInOutEditorApprovalSevicesRepository, PayrollInOutEditorApprovalSevicesRepository>();
builder.Services.AddScoped<IPayrollMachineAttendanceApprovalSevicesRepository, PayrollMachineAttendanceApprovalSevicesRepository>();
builder.Services.AddScoped<IPayrollMultiLoanReceivingSevicesRepository, PayrollMultiLoanReceivingSevicesRepository>();
builder.Services.AddScoped<IPayrollNightOverTimeSevicesRepository, PayrollNightOverTimeSevicesRepository>();

builder.Services.AddScoped<IPayrollDashBoardSevicesRepository, PayrollDashBoardSevicesRepository>();
builder.Services.AddScoped<IPayrollApprovalDashBoardSevicesRepository, PayrollApprovalDashBoardSevicesRepository>();
builder.Services.AddScoped<IPayrollReportSevicesRepository, PayrollReportSevicesRepository>();
builder.Services.AddScoped<IPayrollAbsentApprovalSevicesRepository, PayrollAbsentApprovalSevicesRepository>();
builder.Services.AddScoped<IPayrollHolidaySevicesRepository, PayrollHolidaySevicesRepository>();
builder.Services.AddScoped<IPayrollSalaryProcessSevicesRepository, PayrollSalaryProcessSevicesRepository>();
builder.Services.AddScoped<IPayrollSevicesRepository, PayrollSevicesRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();


// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Builder;
// using Microsoft.AspNetCore.Hosting;
// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.Hosting;
// using Microsoft.Extensions.Logging;
// using TWP_API_Payroll;
// using TWP_API_Production;

// namespace TWP_API_Production
// {
//     public class Program
//     {
//         public static void Main(string[] args)
//         {
//             CreateHostBuilder(args).Build().Run();
//         }

//         public static IHostBuilder CreateHostBuilder(string[] args) =>
//             Host.CreateDefaultBuilder(args)
//                 .ConfigureWebHostDefaults(webBuilder =>
//                 {
//                     webBuilder.UseStartup<Startup>();
//                 });
//     }

// }