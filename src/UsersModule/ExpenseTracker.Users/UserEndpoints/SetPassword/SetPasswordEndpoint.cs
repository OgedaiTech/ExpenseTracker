using FastEndpoints;
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
            AddError("INVALID_REQUEST", "Email, token, and new password are required");
            ThrowIfAnyErrors();
        }

        if (req.NewPassword != req.ConfirmPassword)
        {
            AddError("PASSWORD_MISMATCH", "Passwords do not match");
            ThrowIfAnyErrors();
        }

        var user = await userManager.FindByEmailAsync(req.Email);
        if (user == null)
        {
            LogUserNotFound(logger, req.Email);
            AddError("INVALID_TOKEN", "Invalid or expired token");
            ThrowIfAnyErrors();
        }

        if (user!.IsDeactivated)
        {
            AddError("USER_DEACTIVATED", "User account is deactivated");
            ThrowIfAnyErrors();
        }

        var result = await userManager.ResetPasswordAsync(user!, req.Token, req.NewPassword);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                LogPasswordResetFailed(logger, req.Email, error.Description);
                AddError(error.Code, error.Description);
            }
            ThrowIfAnyErrors();
        }

        LogPasswordSetSuccessfully(logger, req.Email);
        await Send.OkAsync(ct);
    }

    [LoggerMessage(
        EventId = 120,
        Level = LogLevel.Warning,
        Message = "Set password attempt for non-existent user: {Email}")]
    private static partial void LogUserNotFound(ILogger logger, string email);

    [LoggerMessage(
        EventId = 121,
        Level = LogLevel.Warning,
        Message = "Password reset failed for {Email}: {Error}")]
    private static partial void LogPasswordResetFailed(ILogger logger, string email, string error);

    [LoggerMessage(
        EventId = 122,
        Level = LogLevel.Information,
        Message = "Password set successfully for {Email}")]
    private static partial void LogPasswordSetSuccessfully(ILogger logger, string email);
}
