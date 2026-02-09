using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

internal partial class UpdateExpenseEndpoint
{
  internal static readonly Action<ILogger, string, string, Exception?> LogSuccessfullyUpdatedExpense =
    LoggerMessage.Define<string, string>(
      LogLevel.Information,
      new EventId(0, "name: SuccessfullyUpdatedExpense"),
      "Successfully updated an expense. ExpenseId: {ExpenseId}, UserId: {UserId}");

  internal static readonly Action<ILogger, string, string, string, Exception?> LogFailedToUpdateExpense =
    LoggerMessage.Define<string, string, string>(
      LogLevel.Error,
      new EventId(0, "name: FailedToUpdateExpense"),
      "{Reason} ExpenseId: {ExpenseId}, UserId: {UserId}");
}
