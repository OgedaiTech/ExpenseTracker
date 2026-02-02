using System;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

internal partial class SubmitExpenseEndpoint
{
  public static readonly Action<ILogger<SubmitExpenseEndpoint>, string, Exception?> LogSubmitExpenseFailed =
    LoggerMessage.Define<string>(
      LogLevel.Warning,
      new EventId(0, "name: SubmitExpenseFailed"),
      "SubmitExpense failed: {Message}");

  public static readonly Action<ILogger<SubmitExpenseEndpoint>, Guid, string, Exception?> LogSubmitExpenseSucceeded =
    LoggerMessage.Define<Guid, string>(
      LogLevel.Information,
      new EventId(0, "name: SubmitExpenseSucceeded"),
      "SubmitExpense succeeded: ExpenseId {ExpenseId} submitted to ApproverId {ApproverId}");
}
