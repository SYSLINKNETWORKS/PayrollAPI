using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class SalaryTablePostVoucherNameDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNamePost",
                table: "T_WorkerSalary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoucherPostDate",
                table: "T_WorkerSalary",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<Guid>(
                name: "VoucherNo",
                table: "T_StaffSalary",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "UserNamePost",
                table: "T_StaffSalary",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VoucherPostDate",
                table: "T_StaffSalary",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserNamePost",
                table: "T_WorkerSalary");

            migrationBuilder.DropColumn(
                name: "VoucherPostDate",
                table: "T_WorkerSalary");

            migrationBuilder.DropColumn(
                name: "UserNamePost",
                table: "T_StaffSalary");

            migrationBuilder.DropColumn(
                name: "VoucherPostDate",
                table: "T_StaffSalary");

            migrationBuilder.AlterColumn<int>(
                name: "VoucherNo",
                table: "T_StaffSalary",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");
        }
    }
}
