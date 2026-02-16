using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Accounting.Migrations
{
    /// <inheritdoc />
    public partial class UseGenericCredentialsJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParasutCompanyId",
                schema: "Accounting",
                table: "TenantAccountingSettings");

            migrationBuilder.DropColumn(
                name: "ParasutPassword",
                schema: "Accounting",
                table: "TenantAccountingSettings");

            migrationBuilder.DropColumn(
                name: "ParasutUsername",
                schema: "Accounting",
                table: "TenantAccountingSettings");

            migrationBuilder.AddColumn<string>(
                name: "CredentialsJson",
                schema: "Accounting",
                table: "TenantAccountingSettings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CredentialsJson",
                schema: "Accounting",
                table: "TenantAccountingSettings");

            migrationBuilder.AddColumn<string>(
                name: "ParasutCompanyId",
                schema: "Accounting",
                table: "TenantAccountingSettings",
                type: "character varying(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParasutPassword",
                schema: "Accounting",
                table: "TenantAccountingSettings",
                type: "character varying(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ParasutUsername",
                schema: "Accounting",
                table: "TenantAccountingSettings",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);
        }
    }
}
