using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ApproverRole;

internal partial class AssignApproverRole
{
  public static readonly Action<ILogger, Guid, Exception?> LogUserNotFoundWhenTryingToAssignApproverRole =
    LoggerMessage.Define<Guid>(
      LogLevel.Warning,
      new EventId(0, "name: UserNotFoundWhenTryingToAssignApproverRole"),
      "User not found when trying to assign approver role to user {UserId}");
  public static readonly Action<ILogger, Guid, Guid, Exception?> LogAssignedApproverRole =
    LoggerMessage.Define<Guid, Guid>(
      LogLevel.Information,
      new EventId(1, "name: AssignedApproverRole"),
      "Assigned approver role to user {UserId} by requesting user {RequestingUserId}");

  public static readonly Action<ILogger, Guid, Guid, Exception?> LogCannotAssignApproverRoleWhenTheyAreNotInSameTenant =
    LoggerMessage.Define<Guid, Guid>(
      LogLevel.Information,
      new EventId(2, "name: UserTriedToRemoveApproverRoleFromUserButTheyAreNotInSameTenant"),
      "User {RequestingUserId} tried to assign approver role to user {UserId} but they are not in the same tenant");
}
