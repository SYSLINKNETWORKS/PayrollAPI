using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class DBInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cf_Allowance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Fix = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_Allowance", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_AnnualLeaves",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AnnualLeaveAllow = table.Column<bool>(type: "bit", nullable: false),
                    AnnualLeaveDays = table.Column<int>(type: "int", nullable: false),
                    SickLeaveAllow = table.Column<bool>(type: "bit", nullable: false),
                    SickLeaveDays = table.Column<int>(type: "int", nullable: false),
                    CasualLeaveAllow = table.Column<bool>(type: "bit", nullable: false),
                    CasualLeaveDays = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_AnnualLeaves", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_AttendanceMachineCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_AttendanceMachineCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_DailyDate",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HolidayCheck = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_DailyDate", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "Cf_Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_Designation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Director = table.Column<bool>(type: "bit", nullable: false),
                    Salesman = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_Designation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_EmployeeCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    SalaryAccountNo = table.Column<int>(type: "int", nullable: true),
                    BonusAccountNo = table.Column<int>(type: "int", nullable: true),
                    EidAccountNo = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_EmployeeCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_IncomeTaxSlabEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SlabFrom = table.Column<double>(type: "float", nullable: false),
                    SlabTo = table.Column<double>(type: "float", nullable: false),
                    Percentage = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_IncomeTaxSlabEmployee", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_InOutCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_InOutCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_LoanCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_LoanCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_Roster",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_Roster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "T_Holiday",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HolidayCheck = table.Column<bool>(type: "bit", nullable: false),
                    FactoryOverTimeCheck = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Holiday", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cf_AttendanceMachine",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Ip = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Port = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    AttendanceMachineCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_AttendanceMachine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cf_AttendanceMachine_Cf_AttendanceMachineCategory_AttendanceMachineCategoryId",
                        column: x => x.AttendanceMachineCategoryId,
                        principalTable: "Cf_AttendanceMachineCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cf_Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddressPermanent = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    DateofJoin = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Married = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateofBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CNIC = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    CNICExpire = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NTN = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyExpirence = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyExpirenceDescription = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CompanyExpirenceFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyExpirenceTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompanyExpirenceRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    QualificationInstitute = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Qualification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    QualificationYear = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QualificationRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Gratuity = table.Column<bool>(type: "bit", nullable: false),
                    EOBI = table.Column<bool>(type: "bit", nullable: false),
                    EOBIRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EOBIRegistrationNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SESSI = table.Column<bool>(type: "bit", nullable: false),
                    SESSIRegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SESSIRegistrationNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StopPayment = table.Column<bool>(type: "bit", nullable: false),
                    ResignationCheck = table.Column<bool>(type: "bit", nullable: false),
                    ResignationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResignationRemarks = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ModeOfPayment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SalaryAccount = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OverTime = table.Column<bool>(type: "bit", nullable: false),
                    OverTimeHoliday = table.Column<bool>(type: "bit", nullable: false),
                    OverTimeRate = table.Column<double>(type: "float", nullable: false),
                    OvertimeSaturday = table.Column<bool>(type: "bit", nullable: false),
                    OverTimeFactory = table.Column<bool>(type: "bit", nullable: false),
                    LateDeduction = table.Column<bool>(type: "bit", nullable: false),
                    AttendanceAllowance = table.Column<bool>(type: "bit", nullable: false),
                    AttendanceExempt = table.Column<bool>(type: "bit", nullable: false),
                    OverTimeRateCheck = table.Column<bool>(type: "bit", nullable: false),
                    DocumentAuthorize = table.Column<bool>(type: "bit", nullable: false),
                    TemporaryPermanent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverTimeSpecialRate = table.Column<double>(type: "float", nullable: false),
                    IncomeTax = table.Column<bool>(type: "bit", nullable: false),
                    ProvisionPeriod = table.Column<int>(type: "int", nullable: false),
                    DateofParmanent = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmergencyContactOne = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    EmergencyContactTwo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    TakafulRate = table.Column<double>(type: "float", nullable: false),
                    OfficeWorker = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReferenceOne = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReferenceCNICOne = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ReferenceAddressOne = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReferenceContactOne = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReferenceTwo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReferenceCNICTwo = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ReferenceAddressTwo = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    ReferenceContactTwo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DesignationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AnnualLeavesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RosterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cf_Employee_Cf_AnnualLeaves_AnnualLeavesId",
                        column: x => x.AnnualLeavesId,
                        principalTable: "Cf_AnnualLeaves",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cf_Employee_Cf_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Cf_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cf_Employee_Cf_Designation_DesignationId",
                        column: x => x.DesignationId,
                        principalTable: "Cf_Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cf_Employee_Cf_EmployeeCategory_EmployeeCategoryId",
                        column: x => x.EmployeeCategoryId,
                        principalTable: "Cf_EmployeeCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cf_Employee_Cf_Roster_RosterId",
                        column: x => x.RosterId,
                        principalTable: "Cf_Roster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cf_RosterGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OverTime = table.Column<double>(type: "float", nullable: false),
                    WorkingHours = table.Column<double>(type: "float", nullable: false),
                    Late = table.Column<double>(type: "float", nullable: false),
                    EarlyGoing = table.Column<double>(type: "float", nullable: false),
                    EarlyOvertime = table.Column<double>(type: "float", nullable: false),
                    MorningWorkingHours = table.Column<int>(type: "int", nullable: false),
                    EveningWorkingHours = table.Column<int>(type: "int", nullable: false),
                    MondayCheck = table.Column<bool>(type: "bit", nullable: false),
                    MondayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MondayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MondayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    TuesdayCheck = table.Column<bool>(type: "bit", nullable: false),
                    TuesdayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TuesdayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    WednesdayCheck = table.Column<bool>(type: "bit", nullable: false),
                    WednesdayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WednesdayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    ThursdayCheck = table.Column<bool>(type: "bit", nullable: false),
                    ThursdayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThursdayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    FridayCheck = table.Column<bool>(type: "bit", nullable: false),
                    FridayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FridayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    SaturdayCheck = table.Column<bool>(type: "bit", nullable: false),
                    SaturdayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SaturdayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    SundayCheck = table.Column<bool>(type: "bit", nullable: false),
                    SundayInn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayOut = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SundayWorkingHours = table.Column<int>(type: "int", nullable: false),
                    RosterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_RosterGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cf_RosterGroup_Cf_Roster_RosterId",
                        column: x => x.RosterId,
                        principalTable: "Cf_Roster",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_CheckInOut",
                columns: table => new
                {
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    CheckTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    VerifyCode = table.Column<int>(type: "int", nullable: true),
                    SensorId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AttendanceMachineId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InOutCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CheckInOut", x => new { x.MachineId, x.CheckTime });
                    table.ForeignKey(
                        name: "FK_T_CheckInOut_Cf_AttendanceMachine_AttendanceMachineId",
                        column: x => x.AttendanceMachineId,
                        principalTable: "Cf_AttendanceMachine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CheckInOut_Cf_InOutCategory_InOutCategoryId",
                        column: x => x.InOutCategoryId,
                        principalTable: "Cf_InOutCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cf_EmployeeAllowances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AllowanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_EmployeeAllowances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cf_EmployeeAllowances_Cf_Allowance_AllowanceId",
                        column: x => x.AllowanceId,
                        principalTable: "Cf_Allowance",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cf_EmployeeAllowances_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cf_EmployeeImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageProfileCheck = table.Column<bool>(type: "bit", nullable: false),
                    ImageName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageBytes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageExtension = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_EmployeeImage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cf_EmployeeImage_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Absent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedAdjust = table.Column<bool>(type: "bit", nullable: false),
                    ApprovedAdjustType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Absent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Absent_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Advance",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CheaqueCash = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Advance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Advance_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_AnnualLeaveAdjustment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LeaveAdjust = table.Column<int>(type: "int", nullable: false),
                    ApprovedAdjustType = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_AnnualLeaveAdjustment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_AnnualLeaveAdjustment_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_CheckAttendance",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Inn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Out = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Minutes = table.Column<int>(type: "int", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    AttendanceMachineIdInn = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AttendanceMachineIdOut = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_CheckAttendance", x => new { x.EmployeeId, x.Date });
                    table.ForeignKey(
                        name: "FK_T_CheckAttendance_Cf_AttendanceMachine_AttendanceMachineIdInn",
                        column: x => x.AttendanceMachineIdInn,
                        principalTable: "Cf_AttendanceMachine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CheckAttendance_Cf_AttendanceMachine_AttendanceMachineIdOut",
                        column: x => x.AttendanceMachineIdOut,
                        principalTable: "Cf_AttendanceMachine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_CheckAttendance_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_LoanIssue",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    NoOfInstalment = table.Column<double>(type: "float", nullable: false),
                    InstalmentAmount = table.Column<double>(type: "float", nullable: false),
                    LoanCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LoanStatus = table.Column<bool>(type: "bit", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheaqueCash = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LoanIssue", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_LoanIssue_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_T_LoanIssue_Cf_LoanCategory_LoanCategoryId",
                        column: x => x.LoanCategoryId,
                        principalTable: "Cf_LoanCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_Salary",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousAmount = table.Column<double>(type: "float", nullable: false),
                    IncreamentPercentage = table.Column<double>(type: "float", nullable: false),
                    IncreamentAmount = table.Column<double>(type: "float", nullable: false),
                    CurrentAmount = table.Column<double>(type: "float", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_Salary", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_Salary_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_SalaryAdditionDeduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdditionAmount = table.Column<double>(type: "float", nullable: false),
                    DeductionAmount = table.Column<double>(type: "float", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_SalaryAdditionDeduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_SalaryAdditionDeduction_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_StaffSalary",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckIncomeTax = table.Column<bool>(type: "bit", nullable: false),
                    Takaful = table.Column<double>(type: "float", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfResign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceExempted = table.Column<bool>(type: "bit", nullable: false),
                    CheckAttendanceAllowance = table.Column<bool>(type: "bit", nullable: false),
                    CheckOvertime = table.Column<bool>(type: "bit", nullable: false),
                    SalaryAmount = table.Column<double>(type: "float", nullable: false),
                    SalaryAllowanceAmount = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossAmount = table.Column<double>(type: "float", nullable: false),
                    NoOfDaysMonth = table.Column<int>(type: "int", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    ResignDays = table.Column<int>(type: "int", nullable: false),
                    JoinDays = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<double>(type: "float", nullable: false),
                    SalaryPerDay = table.Column<double>(type: "float", nullable: false),
                    SalaryPerHour = table.Column<double>(type: "float", nullable: false),
                    SalaryPerminute = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerDay = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerHour = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerMinute = table.Column<double>(type: "float", nullable: false),
                    PresentDays = table.Column<int>(type: "int", nullable: false),
                    AbsentDays = table.Column<int>(type: "int", nullable: false),
                    AbsentDaysApproval = table.Column<int>(type: "int", nullable: false),
                    TotalAbsentDays = table.Column<int>(type: "int", nullable: false),
                    AdjustedDays = table.Column<int>(type: "int", nullable: false),
                    AdvanceAmount = table.Column<double>(type: "float", nullable: false),
                    LoanAmount = table.Column<double>(type: "float", nullable: false),
                    IncomeTaxAmount = table.Column<double>(type: "float", nullable: false),
                    AdditionAmount = table.Column<double>(type: "float", nullable: false),
                    DeductionAmount = table.Column<double>(type: "float", nullable: false),
                    AttendanceAllowanceAmount = table.Column<double>(type: "float", nullable: false),
                    OvertimeMinutes = table.Column<double>(type: "float", nullable: false),
                    OvertimeHours = table.Column<double>(type: "float", nullable: false),
                    OvertimeDays = table.Column<double>(type: "float", nullable: false),
                    LateMinutes = table.Column<double>(type: "float", nullable: false),
                    LateHours = table.Column<double>(type: "float", nullable: false),
                    LateDays = table.Column<double>(type: "float", nullable: false),
                    LateDaysTotal = table.Column<double>(type: "float", nullable: false),
                    AbsentMinutes = table.Column<double>(type: "float", nullable: false),
                    AbsentHours = table.Column<double>(type: "float", nullable: false),
                    OvertimeRate = table.Column<double>(type: "float", nullable: false),
                    OvertimeActual = table.Column<double>(type: "float", nullable: false),
                    OvertimeActualAmount = table.Column<double>(type: "float", nullable: false),
                    AbsentDaysActual = table.Column<double>(type: "float", nullable: false),
                    AbsentDaysActualAmount = table.Column<double>(type: "float", nullable: false),
                    LateDaysActual = table.Column<double>(type: "float", nullable: false),
                    LateDaysActualAmount = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    GrossAmount = table.Column<double>(type: "float", nullable: false),
                    PayableAmount = table.Column<double>(type: "float", nullable: false),
                    AllowanceAbsent = table.Column<int>(type: "int", nullable: false),
                    VoucherPostCk = table.Column<bool>(type: "bit", nullable: false),
                    VoucherNo = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_StaffSalary", x => new { x.EmployeeId, x.Date });
                    table.ForeignKey(
                        name: "FK_T_StaffSalary_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_WorkerSalary",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CheckIncomeTax = table.Column<bool>(type: "bit", nullable: false),
                    Takaful = table.Column<double>(type: "float", nullable: false),
                    DateOfJoining = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateOfResign = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AttendanceExempted = table.Column<bool>(type: "bit", nullable: false),
                    CheckAttendanceAllowance = table.Column<bool>(type: "bit", nullable: false),
                    CheckOvertime = table.Column<bool>(type: "bit", nullable: false),
                    SalaryAmount = table.Column<double>(type: "float", nullable: false),
                    SalaryAllowanceAmount = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossAmount = table.Column<double>(type: "float", nullable: false),
                    NoOfDaysMonth = table.Column<int>(type: "int", nullable: false),
                    WorkingDays = table.Column<int>(type: "int", nullable: false),
                    ResignDays = table.Column<int>(type: "int", nullable: false),
                    JoinDays = table.Column<int>(type: "int", nullable: false),
                    WorkingHours = table.Column<double>(type: "float", nullable: false),
                    SalaryPerDay = table.Column<double>(type: "float", nullable: false),
                    SalaryPerHour = table.Column<double>(type: "float", nullable: false),
                    SalaryPerminute = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerDay = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerHour = table.Column<double>(type: "float", nullable: false),
                    SalaryGrossPerMinute = table.Column<double>(type: "float", nullable: false),
                    PresentDays = table.Column<int>(type: "int", nullable: false),
                    AbsentDays = table.Column<int>(type: "int", nullable: false),
                    AbsentDaysApproval = table.Column<int>(type: "int", nullable: false),
                    TotalAbsentDays = table.Column<int>(type: "int", nullable: false),
                    AdjustedDays = table.Column<int>(type: "int", nullable: false),
                    AdvanceAmount = table.Column<double>(type: "float", nullable: false),
                    LoanAmount = table.Column<double>(type: "float", nullable: false),
                    IncomeTaxAmount = table.Column<double>(type: "float", nullable: false),
                    AdditionAmount = table.Column<double>(type: "float", nullable: false),
                    DeductionAmount = table.Column<double>(type: "float", nullable: false),
                    AttendanceAllowanceAmount = table.Column<double>(type: "float", nullable: false),
                    OvertimeMinutes = table.Column<double>(type: "float", nullable: false),
                    OvertimeHours = table.Column<double>(type: "float", nullable: false),
                    OvertimeDays = table.Column<double>(type: "float", nullable: false),
                    LateMinutes = table.Column<double>(type: "float", nullable: false),
                    LateHours = table.Column<double>(type: "float", nullable: false),
                    LateDays = table.Column<double>(type: "float", nullable: false),
                    LateDaysTotal = table.Column<double>(type: "float", nullable: false),
                    AbsentMinutes = table.Column<double>(type: "float", nullable: false),
                    AbsentHours = table.Column<double>(type: "float", nullable: false),
                    OvertimeRate = table.Column<double>(type: "float", nullable: false),
                    OvertimeActual = table.Column<double>(type: "float", nullable: false),
                    OvertimeActualAmount = table.Column<double>(type: "float", nullable: false),
                    AbsentDaysActual = table.Column<double>(type: "float", nullable: false),
                    AbsentDaysActualAmount = table.Column<double>(type: "float", nullable: false),
                    LateDaysActual = table.Column<double>(type: "float", nullable: false),
                    LateDaysActualAmount = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    GrossAmount = table.Column<double>(type: "float", nullable: false),
                    PayableAmount = table.Column<double>(type: "float", nullable: false),
                    AllowanceAbsent = table.Column<int>(type: "int", nullable: false),
                    VoucherPostCk = table.Column<bool>(type: "bit", nullable: false),
                    VoucherNo = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_WorkerSalary", x => new { x.EmployeeId, x.Date });
                    table.ForeignKey(
                        name: "FK_T_WorkerSalary_Cf_Employee_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Cf_Employee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "T_LoanReceive",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    LoanIssueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheaqueCash = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(1)", maxLength: 1, nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserNameDelete = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: true),
                    DeleteDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_T_LoanReceive", x => x.Id);
                    table.ForeignKey(
                        name: "FK_T_LoanReceive_T_LoanIssue_LoanIssueId",
                        column: x => x.LoanIssueId,
                        principalTable: "T_LoanIssue",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cf_AttendanceMachine_AttendanceMachineCategoryId",
                table: "Cf_AttendanceMachine",
                column: "AttendanceMachineCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_Employee_AnnualLeavesId",
                table: "Cf_Employee",
                column: "AnnualLeavesId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_Employee_DepartmentId",
                table: "Cf_Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_Employee_DesignationId",
                table: "Cf_Employee",
                column: "DesignationId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_Employee_EmployeeCategoryId",
                table: "Cf_Employee",
                column: "EmployeeCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_Employee_RosterId",
                table: "Cf_Employee",
                column: "RosterId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_EmployeeAllowances_AllowanceId",
                table: "Cf_EmployeeAllowances",
                column: "AllowanceId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_EmployeeAllowances_EmployeeId",
                table: "Cf_EmployeeAllowances",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_EmployeeImage_EmployeeId",
                table: "Cf_EmployeeImage",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Cf_RosterGroup_RosterId",
                table: "Cf_RosterGroup",
                column: "RosterId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Absent_EmployeeId",
                table: "T_Absent",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Advance_EmployeeId",
                table: "T_Advance",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_AnnualLeaveAdjustment_EmployeeId",
                table: "T_AnnualLeaveAdjustment",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CheckAttendance_AttendanceMachineIdInn",
                table: "T_CheckAttendance",
                column: "AttendanceMachineIdInn");

            migrationBuilder.CreateIndex(
                name: "IX_T_CheckAttendance_AttendanceMachineIdOut",
                table: "T_CheckAttendance",
                column: "AttendanceMachineIdOut");

            migrationBuilder.CreateIndex(
                name: "IX_T_CheckInOut_AttendanceMachineId",
                table: "T_CheckInOut",
                column: "AttendanceMachineId");

            migrationBuilder.CreateIndex(
                name: "IX_T_CheckInOut_InOutCategoryId",
                table: "T_CheckInOut",
                column: "InOutCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_T_LoanIssue_EmployeeId",
                table: "T_LoanIssue",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_LoanIssue_LoanCategoryId",
                table: "T_LoanIssue",
                column: "LoanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_T_LoanReceive_LoanIssueId",
                table: "T_LoanReceive",
                column: "LoanIssueId");

            migrationBuilder.CreateIndex(
                name: "IX_T_Salary_EmployeeId",
                table: "T_Salary",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_T_SalaryAdditionDeduction_EmployeeId",
                table: "T_SalaryAdditionDeduction",
                column: "EmployeeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cf_DailyDate");

            migrationBuilder.DropTable(
                name: "Cf_EmployeeAllowances");

            migrationBuilder.DropTable(
                name: "Cf_EmployeeImage");

            migrationBuilder.DropTable(
                name: "Cf_IncomeTaxSlabEmployee");

            migrationBuilder.DropTable(
                name: "Cf_RosterGroup");

            migrationBuilder.DropTable(
                name: "T_Absent");

            migrationBuilder.DropTable(
                name: "T_Advance");

            migrationBuilder.DropTable(
                name: "T_AnnualLeaveAdjustment");

            migrationBuilder.DropTable(
                name: "T_CheckAttendance");

            migrationBuilder.DropTable(
                name: "T_CheckInOut");

            migrationBuilder.DropTable(
                name: "T_Holiday");

            migrationBuilder.DropTable(
                name: "T_LoanReceive");

            migrationBuilder.DropTable(
                name: "T_Salary");

            migrationBuilder.DropTable(
                name: "T_SalaryAdditionDeduction");

            migrationBuilder.DropTable(
                name: "T_StaffSalary");

            migrationBuilder.DropTable(
                name: "T_WorkerSalary");

            migrationBuilder.DropTable(
                name: "Cf_Allowance");

            migrationBuilder.DropTable(
                name: "Cf_AttendanceMachine");

            migrationBuilder.DropTable(
                name: "Cf_InOutCategory");

            migrationBuilder.DropTable(
                name: "T_LoanIssue");

            migrationBuilder.DropTable(
                name: "Cf_AttendanceMachineCategory");

            migrationBuilder.DropTable(
                name: "Cf_Employee");

            migrationBuilder.DropTable(
                name: "Cf_LoanCategory");

            migrationBuilder.DropTable(
                name: "Cf_AnnualLeaves");

            migrationBuilder.DropTable(
                name: "Cf_Department");

            migrationBuilder.DropTable(
                name: "Cf_Designation");

            migrationBuilder.DropTable(
                name: "Cf_EmployeeCategory");

            migrationBuilder.DropTable(
                name: "Cf_Roster");
        }
    }
}
