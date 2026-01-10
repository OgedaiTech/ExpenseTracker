using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Users.CsvService;
using ExpenseTracker.Users.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public partial class BulkCreateUsersService(
    UserManager<ApplicationUser> userManager,
    ICsvParserService csvParser,
    IEmailService emailService,
    IOptions<BulkUserCreationSettings> settings,
    ILogger<BulkCreateUsersService> logger) : IBulkCreateUsersService
{
    private readonly BulkUserCreationSettings _settings = settings.Value;

    public async Task<BulkCreateUsersResult> CreateUsersFromCsvAsync(
        Stream csvStream,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        var results = new List<UserCreationResult>();

        var emails = await csvParser.ParseEmailsAsync(csvStream, cancellationToken);

        var uniqueEmails = emails
            .Select(e => e.Trim().ToLowerInvariant())
            .Where(e => !string.IsNullOrWhiteSpace(e))
            .Distinct()
            .ToList();

        if (uniqueEmails.Count > _settings.MaxUsersPerRequest)
        {
            return new BulkCreateUsersResult(
                Success: false,
                ErrorCode: "TOO_MANY_USERS",
                ErrorMessage: $"CSV contains {uniqueEmails.Count} users, which exceeds the maximum of {_settings.MaxUsersPerRequest}");
        }

        LogBulkCreateStarted(logger, tenantId, uniqueEmails.Count);

        foreach (var email in uniqueEmails)
        {
            var result = await ProcessSingleEmailAsync(email, tenantId, cancellationToken);
            results.Add(result);
        }

        var response = new BulkCreateUsersResponse(
            TotalProcessed: uniqueEmails.Count,
            SuccessCount: results.Count(r => r.Status == UserCreationStatus.Created),
            AlreadyExistedCount: results.Count(r => r.Status == UserCreationStatus.AlreadyExists),
            FailedCount: results.Count(r => r.Status is UserCreationStatus.Failed or UserCreationStatus.InvalidEmail),
            Results: results
        );

        LogBulkCreateCompleted(logger, response.TotalProcessed, response.SuccessCount,
            response.AlreadyExistedCount, response.FailedCount);

        return new BulkCreateUsersResult(Success: true, Response: response);
    }

    private async Task<UserCreationResult> ProcessSingleEmailAsync(
        string email,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        if (!IsValidEmail(email))
        {
            return new UserCreationResult(email, UserCreationStatus.InvalidEmail, "Invalid email format");
        }

        var existingUser = await userManager.FindByEmailAsync(email);
        if (existingUser != null)
        {
            return new UserCreationResult(email, UserCreationStatus.AlreadyExists);
        }

        var newUser = new ApplicationUser
        {
            UserName = email,
            Email = email,
            TenantId = tenantId
        };

        var createResult = await userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            var errorMsg = string.Join(", ", createResult.Errors.Select(e => e.Description));
            LogUserCreationFailed(logger, email, errorMsg);
            return new UserCreationResult(email, UserCreationStatus.Failed, errorMsg);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(newUser);

        try
        {
            await emailService.SendInvitationEmailAsync(email, token, cancellationToken);
            return new UserCreationResult(email, UserCreationStatus.Created);
        }
        catch (Exception ex)
        {
            LogEmailSendFailed(logger, email, ex.Message, ex);
            return new UserCreationResult(email, UserCreationStatus.Created,
                "User created but invitation email failed to send");
        }
    }

    private static bool IsValidEmail(string email)
    {
        return new EmailAddressAttribute().IsValid(email);
    }

    [LoggerMessage(
        EventId = 100,
        Level = LogLevel.Information,
        Message = "Bulk user creation started. TenantId: {TenantId}, EmailCount: {EmailCount}")]
    private static partial void LogBulkCreateStarted(ILogger logger, Guid tenantId, int emailCount);

    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Information,
        Message = "Bulk user creation completed. Total: {Total}, Created: {Created}, Existed: {Existed}, Failed: {Failed}")]
    private static partial void LogBulkCreateCompleted(
        ILogger logger, int total, int created, int existed, int failed);

    [LoggerMessage(
        EventId = 102,
        Level = LogLevel.Warning,
        Message = "Failed to create user {Email}: {Error}")]
    private static partial void LogUserCreationFailed(ILogger logger, string email, string error);

    [LoggerMessage(
        EventId = 103,
        Level = LogLevel.Warning,
        Message = "Failed to send invitation email to {Email}: {Error}")]
    private static partial void LogEmailSendFailed(ILogger logger, string email, string error, Exception? exception);
}
