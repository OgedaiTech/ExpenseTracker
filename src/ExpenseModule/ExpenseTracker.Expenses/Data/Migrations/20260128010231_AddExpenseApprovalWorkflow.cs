using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Expenses.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddExpenseApprovalWorkflow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ApprovedAt",
                schema: "Expense",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedByUserId",
                schema: "Expense",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RejectedAt",
                schema: "Expense",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RejectedByUserId",
                schema: "Expense",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                schema: "Expense",
                table: "Expenses",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "Expense",
                table: "Expenses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmittedAt",
                schema: "Expense",
                table: "Expenses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubmittedToApproverId",
                schema: "Expense",
                table: "Expenses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_Status",
                schema: "Expense",
                table: "Expenses",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_SubmittedToApproverId",
                schema: "Expense",
                table: "Expenses",
                column: "SubmittedToApproverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Expenses_Status",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropIndex(
                name: "IX_Expenses_SubmittedToApproverId",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ApprovedAt",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RejectedAt",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RejectedByUserId",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "RejectionReason",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SubmittedAt",
                schema: "Expense",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "SubmittedToApproverId",
                schema: "Expense",
                table: "Expenses");
        }
    }
}
