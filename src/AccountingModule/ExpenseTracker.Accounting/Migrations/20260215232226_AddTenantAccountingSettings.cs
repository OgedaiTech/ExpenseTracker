using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Accounting.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantAccountingSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Accounting");

            migrationBuilder.CreateTable(
                name: "TenantAccountingSettings",
                schema: "Accounting",
                columns: table => new
                {
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false),
                    Provider = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ParasutCompanyId = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    ParasutUsername = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ParasutPassword = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TenantAccountingSettings", x => x.TenantId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TenantAccountingSettings",
                schema: "Accounting");
        }
    }
}
