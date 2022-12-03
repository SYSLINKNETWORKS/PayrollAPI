using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TWP_API_Payroll.App_Data;
using TWP_API_Payroll.Generic;
using TWP_API_Payroll.Helpers;
using TWP_API_Payroll.Models;
using TWP_API_Payroll.ViewModels;
using TWP_API_Payroll.ViewModels.Payroll;

namespace TWP_API_Payroll.Processor.Process.Payroll
{
    public class EmployeeProfileProcessor : IProcessor<EmployeeProfileBaseModel>
    {
        private DataContext _context;
        private AbsBusiness _AbsBusiness;
        private SecurityHelper _SecurityHelper = new SecurityHelper();

        public EmployeeProfileProcessor(App_Data.DataContext context)
        {
            _context = context;

            _AbsBusiness = Builder.MakeBusinessClass(Enums.ClassName.EmployeeProfile, _context);
        }
        public async Task<ApiResponse> ProcessGet(Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.View_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataAsync(_User);

                if (Convert.ToInt32(response.statusCode) == 200)
                {
                    var _Table = (IEnumerable<Employee>)response.data;
                    var _MachineIdMax = 1;
                    var _MachineIdMaxTable = await _context.Employees.MaxAsync(x => x.MachineId);
                    if (_MachineIdMaxTable > 1)
                    {
                        _MachineIdMax += _MachineIdMaxTable + 1;
                    }
                    List<EmployeeProfileViewModel> _EmployeeProfileViewModel = new List<EmployeeProfileViewModel>();
                    foreach (var ViewTable in _Table)
                    {
                        var _ReportOfficerTable = await _context.Employees.Where(x => x.Id == ViewTable.ReportOfficerId).FirstOrDefaultAsync();
                        var _ReportOfficerName = _ReportOfficerTable != null ? "[" + _ReportOfficerTable.MachineId.ToString() + "]-" + _ReportOfficerTable.Name.Trim() + "" + _ReportOfficerTable.FatherName.Trim() : "";
                        _EmployeeProfileViewModel.Add(new EmployeeProfileViewModel
                        {
                            Id = ViewTable.Id,
                            MachineId = ViewTable.MachineId,
                            MachineIdMax = _MachineIdMax,
                            Name = ViewTable.Name,
                            FatherName = ViewTable.FatherName,
                            CNIC = ViewTable.CNIC,
                            DateofJoin = ViewTable.DateofJoin,
                            Department = ViewTable.department.Name,
                            Designation = ViewTable.department.Name,
                            ReportOfficerName = _ReportOfficerName,
                            Type = ViewTable.Type,
                            Active = ViewTable.Active,
                            ResignationCheck = ViewTable.ResignationCheck,
                            NewPermission = _UserMenuPermissionAsync.Insert_Permission,
                            UpdatePermission = _UserMenuPermissionAsync.Update_Permission,
                            DeletePermission = _UserMenuPermissionAsync.Delete_Permission,
                        });
                    }

                    response.data = _EmployeeProfileViewModel;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessGetById(Guid _Id, Guid _MenuId, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {

                string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();

                apiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _BranchInfo = (List<BranchViewModel>)apiResponse.data;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                var response = await _AbsBusiness.GetDataByIdAsync(_Id, _User);
                if (Convert.ToInt32(response.statusCode) == 200)
                {
                    var _Table = (Employee)response.data;
                    var _TableImage = await _context.EmployeeImages.Where(x => x.EmployeeId == _Table.Id).ToListAsync();

                    var _ReportOfficerTable = await _context.Employees.Where(x => x.Id == _Table.ReportOfficerId).FirstOrDefaultAsync();
                    var _ReportOfficerName = _ReportOfficerTable != null ? "[" + _ReportOfficerTable.MachineId.ToString() + "]-" + _ReportOfficerTable.Name.Trim() + "" + _ReportOfficerTable.FatherName.Trim() : "";

                    var _ViewModel = new EmployeeProfileViewByIdModel
                    {
                        Id = _Table.Id,
                        BranchId = _Table.BranchId,
                        BranchName = _BranchInfo.Where(x => x.BranchId == _Table.BranchId).FirstOrDefault().BranchName,// _Table.branch.Name.Trim(),
                        EmployeeCategoryId = _Table.EmployeeCategoryId,
                        EmployeeCategoryName = _Table.employeeCategory.Name.Trim(),
                        DesignationId = _Table.DesignationId,
                        DesignationName = _Table.designation.Name.Trim(),
                        DepartmentId = _Table.DepartmentId,
                        DepartmentName = _Table.department.Name.Trim(),
                        AnnualLeavesId = _Table.AnnualLeavesId,
                        AnnualLeavesName = _Table.annualLeaves.Name.Trim(),
                        RosterId = _Table.RosterId,
                        RosterName = _Table.roster.Name.Trim(),
                        MachineId = _Table.MachineId,
                        Name = _Table.Name.Trim(),
                        FatherName = _Table.FatherName.Trim(),
                        Address = _Table.Address.Trim(),
                        AddressPermanent = _Table.AddressPermanent.Trim(),
                        DateofJoin = _Table.DateofJoin,
                        Gender = _Table.Gender,
                        Married = _Table.Married,
                        DateofBirth = _Table.DateofBirth,
                        CNIC = _Table.CNIC.Trim(),
                        CNICExpire = _Table.CNICExpire,
                        NTN = _Table.NTN.Trim(),
                        Phone = _Table.Phone.Trim(),
                        Mobile = _Table.Mobile.Trim(),
                        Email = _Table.Email.Trim(),

                        ReportOfficerId = _Table.ReportOfficerId,
                        ReportOfficerName = _ReportOfficerName,


                        CompanyExpirence = _Table.CompanyExpirence.Trim(),
                        CompanyExpirenceDescription = _Table.CompanyExpirenceDescription.Trim(),
                        CompanyExpirenceFrom = _Table.CompanyExpirenceFrom,
                        CompanyExpirenceTo = _Table.CompanyExpirenceTo,
                        CompanyExpirenceRemarks = _Table.CompanyExpirenceRemarks.Trim(),
                        QualificationInstitute = _Table.QualificationInstitute.Trim(),
                        Qualification = _Table.Qualification.Trim(),
                        QualificationYear = _Table.QualificationYear,
                        QualificationRemarks = _Table.QualificationRemarks.Trim(),
                        Gratuity = _Table.Gratuity,
                        SESSIRegistrationNo = _Table.SESSIRegistrationNo.Trim(),
                        SESSIRegistrationDate = _Table.SESSIRegistrationDate,
                        EOBIRegistrationNo = _Table.EOBIRegistrationNo.Trim(),
                        EOBIRegistrationDate = _Table.EOBIRegistrationDate,
                        StopPayment = _Table.StopPayment,
                        ResignationCheck = _Table.ResignationCheck,
                        ResignationDate = _Table.ResignationDate,
                        ResignationRemarks = _Table.ResignationRemarks.Trim(),
                        ModeOfPayment = _Table.ModeOfPayment,
                        SalaryAccount = _Table.SalaryAccount,
                        OverTime = _Table.OverTime,
                        OverTimeHoliday = _Table.OverTimeHoliday,
                        OverTimeRate = _Table.OverTimeRate,
                        OverTimeFactory = _Table.OverTimeFactory,
                        LateDeduction = _Table.LateDeduction,
                        AttendanceAllowance = _Table.AttendanceAllowance,
                        AttendanceExempt = _Table.AttendanceExempt,
                        OverTimeRateCheck = _Table.OverTimeRateCheck,
                        DocumentAuthorize = _Table.DocumentAuthorize,
                        TemporaryPermanent = _Table.TemporaryPermanent,
                        IncomeTax = _Table.IncomeTax,
                        ProvisionPeriod = _Table.ProvisionPeriod,
                        DateofParmanent = _Table.DateofParmanent,
                        EmergencyContactOne = _Table.EmergencyContactOne.Trim(),
                        EmergencyContactTwo = _Table.EmergencyContactTwo.Trim(),
                        Remarks = _Table.Remarks.Trim(),
                        TakafulRate = _Table.TakafulRate,
                        OfficeWorker = _Table.OfficeWorker,
                        ReferenceOne = _Table.ReferenceOne.Trim(),
                        ReferenceCNICOne = _Table.ReferenceCNICOne.Trim(),
                        ReferenceAddressOne = _Table.ReferenceAddressOne.Trim(),
                        ReferenceContactOne = _Table.ReferenceContactOne.Trim(),
                        ReferenceTwo = _Table.ReferenceTwo.Trim(),
                        ReferenceCNICTwo = _Table.ReferenceCNICTwo.Trim(),
                        ReferenceAddressTwo = _Table.ReferenceAddressTwo.Trim(),
                        ReferenceContactTwo = _Table.ReferenceContactTwo.Trim(),
                        Type = _Table.Type,
                        Active = _Table.Active
                    };

                    List<EmployeeProfileDocumentList> _EmployeeProfileDocumentLists = new List<EmployeeProfileDocumentList>();

                    if (_TableImage != null)
                    {
                        foreach (var _Image in _TableImage.OrderByDescending(a => a.ImageProfileCheck))
                        {

                            _EmployeeProfileDocumentLists.Add(new EmployeeProfileDocumentList
                            {
                                EmployeeId = _Table.Id,
                                ImageProfileCheck = _Image.ImageProfileCheck ? "1" : "0",
                                ImageName = _Image.ImageName,
                                ImageBytes = _Image.ImageBytes,
                                ImageExtension = _Image.ImageExtension,

                            });
                        }
                    }

                    _ViewModel.EmployeeProfileDocumentLists = _EmployeeProfileDocumentLists;
                    response.data = _ViewModel;
                }
                return response;
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessPost(object request, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();


                var _request = (EmployeeProfileAddModel)request;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Insert_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }



                apiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _BranchViewModel = (List<BranchViewModel>)apiResponse.data;

                var _Table = new Employee
                {
                    Name = _request.Name,
                    FatherName = _request.FatherName,
                    MachineId = _request.MachineId,
                    TemporaryPermanent = _request.TemporaryPermanent,
                    ReportOfficerId = _request.ReportOfficerId,
                    DateofParmanent = _request.DateofParmanent,
                    ProvisionPeriod = _request.ProvisionPeriod,
                    DesignationId = _request.DesignationId,
                    DepartmentId = _request.DepartmentId,
                    EmployeeCategoryId = _request.EmployeeCategoryId,
                    DateofBirth = _request.DateofBirth,
                    Address = _request.Address,
                    AddressPermanent = _request.AddressPermanent,
                    Gender = _request.Gender,
                    Married = _request.Married,
                    RosterId = _request.RosterId,
                    OfficeWorker = _request.OfficeWorker,
                    BranchId = _request.BranchId,
                    CompanyId = _BranchViewModel.Where(x => x.BranchId == _request.BranchId).FirstOrDefault().CompanyId,
                    EmergencyContactOne = _request.EmergencyContactOne,
                    EmergencyContactTwo = _request.EmergencyContactTwo,
                    CNIC = _request.CNIC,
                    CNICExpire = _request.CNICExpire,
                    Mobile = _request.Mobile,
                    Phone = _request.Phone,
                    NTN = _request.NTN,
                    Email = _request.Email,
                    Remarks = _request.Remarks,
                    ReferenceOne = _request.ReferenceOne,
                    ReferenceTwo = _request.ReferenceTwo,
                    ReferenceCNICOne = _request.ReferenceCNICOne,
                    ReferenceCNICTwo = _request.ReferenceCNICTwo,
                    ReferenceAddressOne = _request.ReferenceAddressOne,
                    ReferenceAddressTwo = _request.ReferenceAddressTwo,
                    ReferenceContactOne = _request.ReferenceContactOne,
                    ReferenceContactTwo = _request.ReferenceContactTwo,
                    DateofJoin = _request.DateofJoin,
                    SalaryAccount = _request.SalaryAccount,
                    AnnualLeavesId = _request.AnnualLeavesId,
                    ModeOfPayment = _request.ModeOfPayment,
                    Gratuity = _request.Gratuity,
                    AttendanceAllowance = _request.AttendanceAllowance,
                    OverTimeHoliday = _request.OverTimeHoliday,
                    OverTime = _request.OverTime,
                    SESSI = _request.SESSIRegistrationNo.Trim() != "" ? true : false,
                    SESSIRegistrationDate = _request.SESSIRegistrationDate,
                    SESSIRegistrationNo = _request.SESSIRegistrationNo.Trim(),
                    IncomeTax = _request.IncomeTax,
                    AttendanceExempt = _request.AttendanceExempt,
                    EOBI = _request.EOBIRegistrationNo.Trim() != "" ? true : false,
                    EOBIRegistrationDate = _request.EOBIRegistrationDate,
                    EOBIRegistrationNo = _request.EOBIRegistrationNo.Trim(),
                    StopPayment = _request.StopPayment,
                    LateDeduction = _request.LateDeduction,
                    ResignationCheck = _request.ResignationCheck,
                    ResignationDate = _request.ResignationDate,
                    ResignationRemarks = _request.ResignationRemarks,
                    QualificationInstitute = _request.QualificationInstitute,
                    Qualification = _request.Qualification,
                    QualificationYear = _request.QualificationYear,
                    QualificationRemarks = _request.QualificationRemarks,
                    CompanyExpirence = _request.CompanyExpirence,
                    CompanyExpirenceDescription = _request.CompanyExpirenceDescription,
                    CompanyExpirenceFrom = _request.CompanyExpirenceFrom,
                    CompanyExpirenceTo = _request.CompanyExpirenceTo,
                    CompanyExpirenceRemarks = _request.CompanyExpirenceRemarks,
                    Type = _request.Type,
                    Active = _request.Active,

                };
                return await _AbsBusiness.AddAsync(_Table, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }
        public async Task<ApiResponse> ProcessPut(object request, ClaimsPrincipal _User)
        {

            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                string _Key = _User.Claims.FirstOrDefault(c => c.Type == Enums.Misc.Key.ToString())?.Value.ToString();

                var _request = (EmployeeProfileUpdateModel)request;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_request.Menu_Id, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Update_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                apiResponse = await _SecurityHelper.BranchInfoAsync(_Key);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _BranchViewModel = (List<BranchViewModel>)apiResponse.data;

                var _Table = new Employee
                {
                    Id = _request.Id,
                    Name = _request.Name,
                    FatherName = _request.FatherName,
                    MachineId = _request.MachineId,
                    DateofParmanent = _request.DateofParmanent,
                    ProvisionPeriod = _request.ProvisionPeriod,
                    DesignationId = _request.DesignationId,
                    DepartmentId = _request.DepartmentId,
                    DateofBirth = _request.DateofBirth,
                    EmployeeCategoryId = _request.EmployeeCategoryId,
                    Address = _request.Address,
                    AddressPermanent = _request.AddressPermanent,
                    Gender = _request.Gender,
                    Married = _request.Married,
                    RosterId = _request.RosterId,
                    OfficeWorker = _request.OfficeWorker,
                    BranchId = _request.BranchId,
                    CompanyId = _BranchViewModel.Where(x => x.BranchId == _request.BranchId).FirstOrDefault().CompanyId,
                    EmergencyContactOne = _request.EmergencyContactOne,
                    EmergencyContactTwo = _request.EmergencyContactTwo,
                    CNIC = _request.CNIC,
                    CNICExpire = _request.CNICExpire,
                    Mobile = _request.Mobile,
                    NTN = _request.NTN,
                    Email = _request.Email,
                    Remarks = _request.Remarks,
                    ReferenceOne = _request.ReferenceOne,
                    ReferenceTwo = _request.ReferenceTwo,
                    ReferenceCNICOne = _request.ReferenceCNICOne,
                    ReferenceCNICTwo = _request.ReferenceCNICTwo,
                    ReferenceAddressOne = _request.ReferenceAddressOne,
                    ReferenceAddressTwo = _request.ReferenceAddressTwo,
                    ReferenceContactOne = _request.ReferenceContactOne,
                    ReferenceContactTwo = _request.ReferenceContactTwo,
                    DateofJoin = _request.DateofJoin,
                    AnnualLeavesId = _request.AnnualLeavesId,
                    ModeOfPayment = _request.ModeOfPayment,
                    SalaryAccount = _request.SalaryAccount,
                    Gratuity = _request.Gratuity,
                    DocumentAuthorize = _request.DocumentAuthorize,
                    AttendanceAllowance = _request.AttendanceAllowance,
                    OverTimeHoliday = _request.OverTimeHoliday,
                    TakafulRate = _request.TakafulRate,
                    OverTime = _request.OverTime,
                    OverTimeRate = _request.OverTimeRate,
                    OverTimeFactory = _request.OverTimeFactory,
                    SESSI = _request.SESSIRegistrationNo.Trim() != "" ? true : false,
                    SESSIRegistrationDate = _request.SESSIRegistrationDate,
                    SESSIRegistrationNo = _request.SESSIRegistrationNo,
                    IncomeTax = _request.IncomeTax,
                    AttendanceExempt = _request.AttendanceExempt,
                    EOBI = _request.EOBIRegistrationNo.Trim() != "" ? true : false,
                    EOBIRegistrationDate = _request.EOBIRegistrationDate,
                    EOBIRegistrationNo = _request.EOBIRegistrationNo,
                    StopPayment = _request.StopPayment,
                    LateDeduction = _request.LateDeduction,
                    ResignationCheck = _request.ResignationCheck,
                    ResignationDate = _request.ResignationDate,
                    ResignationRemarks = _request.ResignationRemarks,
                    QualificationInstitute = _request.QualificationInstitute,
                    Qualification = _request.Qualification,
                    QualificationYear = _request.QualificationYear,
                    QualificationRemarks = _request.QualificationRemarks,
                    CompanyExpirence = _request.CompanyExpirence,
                    CompanyExpirenceDescription = _request.CompanyExpirenceDescription,
                    CompanyExpirenceFrom = _request.CompanyExpirenceFrom,
                    CompanyExpirenceTo = _request.CompanyExpirenceTo,
                    CompanyExpirenceRemarks = _request.CompanyExpirenceRemarks,
                    ReportOfficerId = _request.ReportOfficerId,

                    Type = _request.Type,
                    Active = _request.Active
                };
                return await _AbsBusiness.UpdateAsync(_Table, _User);

            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;

        }
        public async Task<ApiResponse> ProcessDelete(object request, ClaimsPrincipal _User)
        {
            ApiResponse apiResponse = new ApiResponse();
            if (_AbsBusiness != null)
            {
                var _request = (EmployeeProfileDeleteModel)request;

                Guid _Id = _request.Id;
                Guid _MenuId = _request.Menu_Id;

                apiResponse = await _SecurityHelper.UserMenuPermissionAsync(_MenuId, _User);
                if (apiResponse.statusCode.ToString() != StatusCodes.Status200OK.ToString()) { return apiResponse; }
                var _UserMenuPermissionAsync = (GetUserPermissionViewModel)apiResponse.data;

                if (!_UserMenuPermissionAsync.Delete_Permission)
                {
                    apiResponse.statusCode = StatusCodes.Status403Forbidden.ToString();
                    return apiResponse;
                }
                return await _AbsBusiness.DeleteAsync(_Id, _User);
            }
            apiResponse.statusCode = StatusCodes.Status405MethodNotAllowed.ToString();
            apiResponse.message = "Invalid Class";
            return apiResponse;
        }

    }
}