using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Expenses.Data.Migrations;

/// <inheritdoc />
public partial class AddUserId : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.AddColumn<Guid>(
        name: "CreatedByUserId",
        schema: "Expense",
        table: "Expenses",
        type: "uuid",
        nullable: false,
        defaultValue: Guid.Empty);
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropColumn(
        name: "CreatedByUserId",
        schema: "Expense",
        table: "Expenses");
  }
}
