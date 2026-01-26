using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Email.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ForgotPassword;

internal partial class ForgotPasswordEndpoint(
    UserManager<ApplicationUser> userManager,
    IMediator mediator,
    ILogger<ForgotPasswordEndpoint> logger)
    : Endpoint<ForgotPasswordRequest, ForgotPasswordResponse>
{
    public override void Configure()
    {
        Post("/users/forgot-password");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
    {
        // Validate email is provided
        if (string.IsNullOrWhiteSpace(req.Email))
        {
            AddError("EMAIL_IS_REQUIRED");
            ThrowIfAnyErrors();
        }

        // Validate email format
        if (!IsValidEmail(req.Email))
        {
            AddError("INVALID_EMAIL_FORMAT");
            ThrowIfAnyErrors();
        }

        var email = req.Email.Trim();

        // Find user by email (case-insensitive)
        var user = await userManager.FindByEmailAsync(email);

        // Security best practice: Always return success, even if user doesn't exist
        // This prevents user enumeration attacks
        if (user == null)
        {
            LogUserNotFound(logger, email);
            await SendSuccessResponseAsync(ct);
            return;
        }

        // Check if user is deactivated
        if (user.IsDeactivated)
        {
            LogUserDeactivated(logger, email);
            await SendSuccessResponseAsync(ct);
            return;
        }

        // Generate password reset token
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        // Send password reset email
        var emailCommand = new SendPasswordResetEmailCommand(email, token);
        var emailResult = await mediator.Send(emailCommand, ct);

        if (emailResult.Success)
        {
            LogPasswordResetEmailSent(logger, email);
        }
        else
        {
            LogPasswordResetEmailFailed(logger, email, emailResult.Message ?? "UNKNOWN_ERROR");
        }

        // Always return success (security best practice)
        await SendSuccessResponseAsync(ct);
    }

    private async Task SendSuccessResponseAsync(CancellationToken ct)
    {
        var response = new ForgotPasswordResponse(
            Success: true,
            Message: "If an account exists with this email, you will receive a password reset link shortly."
        );

        await Send.OkAsync(response, ct);
    }

    private static bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    [LoggerMessage(
        EventId = 300,
        Level = LogLevel.Information,
        Message = "Password reset requested for non-existent email: {Email}")]
    private static partial void LogUserNotFound(ILogger logger, string email);

    [LoggerMessage(
        EventId = 301,
        Level = LogLevel.Information,
        Message = "Password reset requested for deactivated user: {Email}")]
    private static partial void LogUserDeactivated(ILogger logger, string email);

    [LoggerMessage(
        EventId = 302,
        Level = LogLevel.Information,
        Message = "Password reset email sent successfully to {Email}")]
    private static partial void LogPasswordResetEmailSent(ILogger logger, string email);

    [LoggerMessage(
        EventId = 303,
        Level = LogLevel.Warning,
        Message = "Failed to send password reset email to {Email}: {Error}")]
    private static partial void LogPasswordResetEmailFailed(ILogger logger, string email, string error);
}
