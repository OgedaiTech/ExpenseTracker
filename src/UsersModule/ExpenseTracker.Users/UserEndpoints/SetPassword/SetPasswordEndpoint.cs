using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.SetPassword;

internal partial class SetPasswordEndpoint(
    UserManager<ApplicationUser> userManager,
    ILogger<SetPasswordEndpoint> logger)
    : Endpoint<SetPasswordRequest, EmptyResponse>
{
  public override void Configure()
  {
    Post("/users/set-password");
    AllowAnonymous();
  }

  public override async Task HandleAsync(SetPasswordRequest req, CancellationToken ct)
  {
    if (string.IsNullOrWhiteSpace(req.Email) ||
        string.IsNullOrWhiteSpace(req.Token) ||
        string.IsNullOrWhiteSpace(req.NewPassword))
    {
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Detail = "Email, token, and new password are required",
      };
      await Send.ResultAsync(problemDetails);
      return;
    }

    if (req.NewPassword != req.ConfirmPassword)
    {
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Detail = "New password and confirm password do not match",
      };
      await Send.ResultAsync(problemDetails);
      return;
    }

    var user = await userManager.FindByEmailAsync(req.Email);
    if (user == null)
    {
      LogUserNotFound(logger, req.Email);
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status404NotFound,
        Detail = "User not found",
      };
      await Send.ResultAsync(problemDetails);
      return;
    }

    if (user!.IsDeactivated)
    {
      LogUserAccountDeactivated(logger, req.Email);
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Detail = "User account is deactivated",
      };
      await Send.ResultAsync(problemDetails);
      return;
    }

    var result = await userManager.ResetPasswordAsync(user!, req.Token, req.NewPassword);

    if (!result.Succeeded)
    {
      var errorMessages = string.Join(" ", result.Errors.Select(e => e.Description));
      var problemDetails = new ProblemDetails
      {
        Status = StatusCodes.Status400BadRequest,
        Detail = errorMessages,
      };
      LogPasswordResetFailed(logger, req.Email, errorMessages);
      await Send.ResultAsync(problemDetails);
      return;
    }

    LogPasswordSetSuccessfully(logger, req.Email);
    await Send.OkAsync(ct);
  }

  [LoggerMessage(
      Level = LogLevel.Warning,
      Message = "Set password attempt for non-existent user: {Email}")]
  private static partial void LogUserNotFound(ILogger logger, string email);

  [LoggerMessage(
      Level = LogLevel.Warning,
      Message = "Password reset failed for {Email}: {Error}")]
  private static partial void LogPasswordResetFailed(ILogger logger, string email, string error);

  [LoggerMessage(
      Level = LogLevel.Information,
      Message = "Password set successfully for {Email}")]
  private static partial void LogPasswordSetSuccessfully(ILogger logger, string email);

  [LoggerMessage(
      Level = LogLevel.Warning,
      Message = "Set password attempt for deactivated user: {Email}")]
  private static partial void LogUserAccountDeactivated(ILogger logger, string email);
}
