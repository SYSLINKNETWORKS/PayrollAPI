using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class SystemTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cf_System",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminAdvanceAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingAdvanceAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminLoanAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingLoanAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminInsuranceAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingInsuranceAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminIncomeTaxAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingIncomeTaxAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminSalaryExpenseAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollAdminSalaryPayableAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingExpenseAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PayrollManufacturingPayableAccount = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserNameInsert = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cf_System", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cf_System");
        }
    }
}
