using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

internal partial class ListExpenseReceiptsEndpoint
{
  public static readonly Action<ILogger, Guid, Exception?> LogErrorWhenTryingToListExpenseReceipts =
    LoggerMessage.Define<Guid>(
      LogLevel.Error,
      new EventId(0, "name: ErrorWhenTryingToListExpenseReceipts"),
      "An error occurred when trying to list receipts for expense {ExpenseId}");
  public static readonly Action<ILogger, int, Guid, Exception?> LogSuccessfullyRetrievedExpenseReceipts =
    LoggerMessage.Define<int, Guid>(
      LogLevel.Information,
      new EventId(0, "name: SuccessfullyRetrievedExpenseReceipts"),
      "Successfully retrieved {ReceiptCount} receipts for expense {ExpenseId}");
}
