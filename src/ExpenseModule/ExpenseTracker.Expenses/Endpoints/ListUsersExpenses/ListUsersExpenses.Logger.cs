using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

internal partial class ListUsersExpenses
{
  public static readonly Action<ILogger, string, string, int, Exception?> LogSuccessfullyListedUsersExpenses =
    LoggerMessage.Define<string, string, int>(
      LogLevel.Information,
      new EventId(0, "name: SuccessfullyListedUsersExpenses"),
      "Successfully listed user's expenses. UserId: {UserId}, TenantId: {TenantId}, ExpenseCount: {ExpenseCount}");

  public static readonly Action<ILogger, string, Exception?> LogFailedToListUsersExpenses =
    LoggerMessage.Define<string>(
      LogLevel.Warning,
      new EventId(1, "name: FailedToListUsersExpenses"),
      "Failed to list user's expenses. Reason: {Reason}");
}
