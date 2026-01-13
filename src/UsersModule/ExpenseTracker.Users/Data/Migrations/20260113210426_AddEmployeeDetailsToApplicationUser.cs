using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Users.Data.Migrations;

/// <inheritdoc />
public partial class AddEmployeeDetailsToApplicationUser : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.AddColumn<string>(
        name: "EmployeeId",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "FirstName",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "LastName",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "NationalIdentityNo",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "TaxIdNo",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);

    migrationBuilder.AddColumn<string>(
        name: "Title",
        schema: "Users",
        table: "AspNetUsers",
        type: "text",
        nullable: true);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropColumn(
        name: "EmployeeId",
        schema: "Users",
        table: "AspNetUsers");

    migrationBuilder.DropColumn(
        name: "FirstName",
        schema: "Users",
        table: "AspNetUsers");

    migrationBuilder.DropColumn(
        name: "LastName",
        schema: "Users",
        table: "AspNetUsers");

    migrationBuilder.DropColumn(
        name: "NationalIdentityNo",
        schema: "Users",
        table: "AspNetUsers");

    migrationBuilder.DropColumn(
        name: "TaxIdNo",
        schema: "Users",
        table: "AspNetUsers");

    migrationBuilder.DropColumn(
        name: "Title",
        schema: "Users",
        table: "AspNetUsers");
  }
}
