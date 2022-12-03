using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class systemaddcolumnPayrollAdminOverTimeExpenseAccountPayrollAdminOverTimePayableAccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PayrollAdminOverTimeExpenseAccount",
                table: "Cf_System",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollAdminOverTimePayableAccount",
                table: "Cf_System",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollManufacturingOverTimeExpenseAccount",
                table: "Cf_System",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "PayrollManufacturingOverTimePayableAccount",
                table: "Cf_System",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PayrollAdminOverTimeExpenseAccount",
                table: "Cf_System");

            migrationBuilder.DropColumn(
                name: "PayrollAdminOverTimePayableAccount",
                table: "Cf_System");

            migrationBuilder.DropColumn(
                name: "PayrollManufacturingOverTimeExpenseAccount",
                table: "Cf_System");

            migrationBuilder.DropColumn(
                name: "PayrollManufacturingOverTimePayableAccount",
                table: "Cf_System");
        }
    }
}
