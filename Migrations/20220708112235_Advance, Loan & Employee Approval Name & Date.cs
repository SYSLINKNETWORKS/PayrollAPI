using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class AdvanceLoanEmployeeApprovalNameDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "T_LoanIssue",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "T_LoanIssue",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameApproved",
                table: "T_LoanIssue",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "T_Advance",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "T_Advance",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameApproved",
                table: "T_Advance",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Cf_Employee",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateApproved",
                table: "Cf_Employee",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserNameApproved",
                table: "Cf_Employee",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "T_LoanIssue");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "T_LoanIssue");

            migrationBuilder.DropColumn(
                name: "UserNameApproved",
                table: "T_LoanIssue");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "T_Advance");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "T_Advance");

            migrationBuilder.DropColumn(
                name: "UserNameApproved",
                table: "T_Advance");

            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Cf_Employee");

            migrationBuilder.DropColumn(
                name: "DateApproved",
                table: "Cf_Employee");

            migrationBuilder.DropColumn(
                name: "UserNameApproved",
                table: "Cf_Employee");
        }
    }
}
