using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class _20211217_AttendanceMachine_GroupId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine",
                type: "uniqueidentifier",
                nullable: true,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Cf_AttendanceMachine",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Cf_AttendanceMachine_AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine",
                column: "AttendanceMachineGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cf_AttendanceMachine_Cf_AttendanceMachineGroup_AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine",
                column: "AttendanceMachineGroupId",
                principalTable: "Cf_AttendanceMachineGroup",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cf_AttendanceMachine_Cf_AttendanceMachineGroup_AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine");

            migrationBuilder.DropIndex(
                name: "IX_Cf_AttendanceMachine_AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine");

            migrationBuilder.DropColumn(
                name: "AttendanceMachineGroupId",
                table: "Cf_AttendanceMachine");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Cf_AttendanceMachine");
        }
    }
}
