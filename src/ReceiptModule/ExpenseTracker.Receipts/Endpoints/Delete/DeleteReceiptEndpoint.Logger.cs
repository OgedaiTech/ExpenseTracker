using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal partial class DeleteReceiptEndpoint
{
  internal static readonly Action<ILogger, string, Guid, Exception?> LogDeleteReceiptFailed =
  LoggerMessage.Define<string, Guid>(
    LogLevel.Warning,
    new EventId(0, "name: DeleteReceiptFailed"),
    "{Reason} ReceiptId:{ReceiptId}.");

  internal static readonly Action<ILogger, Guid, Exception?> LogDeleteReceiptSuccess =
  LoggerMessage.Define<Guid>(
    LogLevel.Information,
    new EventId(0, "name: DeleteReceiptSuccess"),
    "Successfully deleted receipt with ReceiptId:{ReceiptId}.");
}
