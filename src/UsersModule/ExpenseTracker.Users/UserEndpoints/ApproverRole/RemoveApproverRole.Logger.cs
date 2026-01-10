using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ApproverRole;

internal partial class RemoveApproverRole
{
  public static readonly Action<ILogger, Guid, Exception?> LogUserNotFoundWhenTryingToRemoveApproverRole =
    LoggerMessage.Define<Guid>(
      LogLevel.Warning,
      new EventId(0, "name: ApproverRoleUserNotFound"),
      "User {UserId} not found when trying to remove approver role");
  public static readonly Action<ILogger, Guid, Guid, Exception?> LogRemovedApproverRole =
    LoggerMessage.Define<Guid, Guid>(
      LogLevel.Information,
      new EventId(1, "name: RemovedApproverRole"),
      "Removed approver role from user {UserId} by requesting user {RequestingUserId}");

  public static readonly Action<ILogger, Guid, Guid, Exception?> LogCannotRemoveApproverRoleWhenTheyAreNotInSameTenant =
    LoggerMessage.Define<Guid, Guid>(
      LogLevel.Warning,
      new EventId(2, "name: UserTriedToRemoveApproverRoleFromUserButTheyAreNotInSameTenant"),
      "User {RequestingUserId} tried to remove approver role from user {UserId} but they are not in the same tenant");
}
