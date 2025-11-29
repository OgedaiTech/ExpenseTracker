using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Users.Data.Migrations;

/// <inheritdoc />
public partial class AddedTenantId : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.AddColumn<Guid>(
        name: "TenantId",
        schema: "Users",
        table: "AspNetUsers",
        type: "uuid",
        nullable: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropColumn(
        name: "TenantId",
        schema: "Users",
        table: "AspNetUsers");
  }
}
