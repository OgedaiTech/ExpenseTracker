using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.Create;

internal partial class CreateExpenseEndpoint
{
  internal static readonly Action<ILogger, string, string, Exception?> LogSuccessfullyCreatedExpense =
    LoggerMessage.Define<string, string>(
      LogLevel.Information,
      new EventId(0, "name: SuccessfullyCreatedExpense"),
      "Successfully created an expense. ExpenseName: {ExpenseName}, UserId: {UserId}");

  internal static readonly Action<ILogger, string, string, Exception?> LogExpenseNameCannotBeEmpty =
    LoggerMessage.Define<string, string>(
      LogLevel.Warning,
      new EventId(0, "name: ExpenseNameCannotBeEmpty"),
      "Expense name cannot be empty. UserId: {UserId}, TenantId: {TenantId}");

  internal static readonly Action<ILogger, string, string, Exception?> LogExpenseNameCannotExceed128Characters =
    LoggerMessage.Define<string, string>(
      LogLevel.Warning,
      new EventId(0, "name: ExpenseNameCannotExceed128Characters"),
      "Expense name cannot exceed 128 characters. UserId: {UserId}, TenantId: {TenantId}");

  internal static readonly Action<ILogger, string, string, string, Exception?> LogFailedToCreateExpense =
    LoggerMessage.Define<string, string, string>(
      LogLevel.Error,
      new EventId(0, "name: FailedToCreateExpense"),
      "Failed to create an expense. UserId: {UserId}, TenantId: {TenantId}, Error: {Error}");
}
