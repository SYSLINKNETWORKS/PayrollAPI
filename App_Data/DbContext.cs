using System;
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TWP_API_Payroll.Models;

// insert into roles(id,Name,NormalizedName) values(NewID(),'Admin','Admin')
// insert into roles(id,Name,NormalizedName) values(NewID(),'Guest','Guest')
// insert into roles(id,Name,NormalizedName) values(NewID(),'Member','Member')
//insert into usergroup(id,Name,Type,Active,Action,Userid,MakerDate) values(NewID(),'Admin','S',1,'A',1,curdate());

// insert into roles(id,Name,NormalizedName) values(UUID(),'Admin','Admin');
// insert into roles(id,Name,NormalizedName) values(UUID(),'Guest','Guest');
// insert into roles(id,Name,NormalizedName) values(UUID(),'Member','Member');
//insert into usergroup(id,Name,Type,Active,Action,Userid,MakerDate) values(UUID(),'Admin','S',1,'A',1,curdate());
//    "Connection": "server=www.syslinknetworkiot.com; port=3306; database=abdulsattar_Auth; user=MSAuth; password=1234$Auth; Persist Security Info=False; Connect Timeout=300"

//    "Connection": "server=demo.syslinknetwork.com; port=3306; database=AuthDB; user=msauth; password=1234$Test; Persist Security Info=False; Connect Timeout=300"
//    "Connection": "server=localhost; port=3306; database=AuthDB; user=root; password=1234$Test; Persist Security Info=False; Connect Timeout=300"
// http://demo.syslinknetwork.com:83/TWP_API_Payroll/swagger/index.html

namespace TWP_API_Payroll.App_Data
{
    public partial class DataContext : DbContext
    {
        //DbContext {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        //System
        public virtual DbSet<CfSystem> CfSystems { get; set; }



        //Payroll
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Designation> Designations { get; set; }
        public virtual DbSet<Roster> Rosters { get; set; }
        public virtual DbSet<EmployeeCategory> EmployeeCategories { get; set; }
        public virtual DbSet<AnnualLeaves> AnnualLeaves { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<EmployeeAllowances> EmployeeAllowances { get; set; }
        public virtual DbSet<EmployeeImage> EmployeeImages { get; set; }
        public virtual DbSet<DailyDate> DailyDates { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<Absent> Absents { get; set; }
        public virtual DbSet<AnnualLeaveAdjustment> AnnualLeaveAdjustments { get; set; }
        public virtual DbSet<AttendanceMachineCategory> AttendanceMachineCategories { get; set; }
        public virtual DbSet<AttendanceMachine> AttendanceMachines { get; set; }
        public virtual DbSet<AttendanceMachineGroup> AttendanceMachineGroups { get; set; }
        public virtual DbSet<InOutCategory> InOutCategories { get; set; }
        public virtual DbSet<IncomeTaxSlabEmployee> IncomeTaxSlabEmployees { get; set; }
        public virtual DbSet<CheckInOut> CheckInOuts { get; set; }
        public virtual DbSet<NightOverTime> NightOverTimes { get; set; }
        public virtual DbSet<CheckAttendance> CheckAttendances { get; set; }
        public virtual DbSet<RosterGroup> RosterGroups { get; set; }
        public virtual DbSet<LoanCategory> LoanCategories { get; set; }
        public virtual DbSet<Allowance> Allowances { get; set; }

        public virtual DbSet<Advance> Advances { get; set; }

        public virtual DbSet<LoanIssue> LoanIssues { get; set; }
        public virtual DbSet<LoanReceive> LoanReceives { get; set; }
        public virtual DbSet<SalaryAdditionDeduction> SalaryAdditionDeductions { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<StaffSalary> StaffSalaries { get; set; }
        public virtual DbSet<WorkerSalary> WorkerSalaries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
                modelBuilder.Entity<CheckInOut>().HasKey(hk => new { hk.MachineId, hk.CheckTime });
                modelBuilder.Entity<CheckAttendance>().HasKey(hk => new { hk.EmployeeId, hk.Date });
                modelBuilder.Entity<StaffSalary>().HasKey(hk => new { hk.EmployeeId, hk.Date });
                modelBuilder.Entity<WorkerSalary>().HasKey(hk => new { hk.EmployeeId, hk.Date });
                modelBuilder.Entity<NightOverTime>().HasKey(hk => new { hk.EmployeeId, hk.Date });

            }

            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

        }
    }
}