using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Receipts.Data.Migrations;

/// <inheritdoc />
public partial class InitialReceipt : Migration
{
  /// <inheritdoc />
  protected override void Up(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.EnsureSchema(
        name: "Receipt");

    migrationBuilder.CreateTable(
        name: "Receipts",
        schema: "Receipt",
        columns: table => new
        {
          Id = table.Column<Guid>(type: "uuid", nullable: false),
          ExpenseId = table.Column<Guid>(type: "uuid", nullable: false),
          ReceiptNo = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
          Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
          Vendor = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
          CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
        },
        constraints: table =>
        {
          table.PrimaryKey("PK_Receipts", x => x.Id);
        });
  }

  /// <inheritdoc />
  protected override void Down(MigrationBuilder migrationBuilder)
  {
    migrationBuilder.DropTable(
        name: "Receipts",
        schema: "Receipt");
  }
}
