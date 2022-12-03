using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TWP_API_Payroll.Migrations
{
    public partial class LoanIssueremovecashchqcolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheaqueCash",
                table: "T_LoanIssue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CheaqueCash",
                table: "T_LoanIssue",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: true);
        }
    }
}
