using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Email.Contracts;
using ExpenseTracker.Users.CsvService;
using ExpenseTracker.Users.EmailService;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public partial class BulkCreateUsersService(
    UserManager<ApplicationUser> userManager,
    ICsvParserService csvParser,
    IMediator mediator,
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

        var userRecords = await csvParser.ParseUserRecordsAsync(csvStream, cancellationToken);

        var uniqueUsers = userRecords
            .GroupBy(u => u.Email.Trim().ToLowerInvariant())
            .Select(g => g.First())
            .Where(u => !string.IsNullOrWhiteSpace(u.Email))
            .ToList();

        if (uniqueUsers.Count > _settings.MaxUsersPerRequest)
        {
            return new BulkCreateUsersResult(
                Success: false,
                ErrorCode: "TOO_MANY_USERS",
                ErrorMessage: $"CSV contains {uniqueUsers.Count} users, which exceeds the maximum of {_settings.MaxUsersPerRequest}");
        }

        LogBulkCreateStarted(logger, tenantId, uniqueUsers.Count);

        foreach (var userRecord in uniqueUsers)
        {
            var result = await ProcessSingleUserRecordAsync(userRecord, tenantId, cancellationToken);
            results.Add(result);
        }

        var response = new BulkCreateUsersResponse(
            TotalProcessed: uniqueUsers.Count,
            SuccessCount: results.Count(r => r.Status == UserCreationStatus.Created),
            AlreadyExistedCount: results.Count(r => r.Status == UserCreationStatus.AlreadyExists),
            FailedCount: results.Count(r => r.Status is UserCreationStatus.Failed or UserCreationStatus.InvalidEmail),
            Results: results
        );

        LogBulkCreateCompleted(logger, response.TotalProcessed, response.SuccessCount,
            response.AlreadyExistedCount, response.FailedCount);

        return new BulkCreateUsersResult(Success: true, Response: response);
    }

    private async Task<UserCreationResult> ProcessSingleUserRecordAsync(
        UserCsvRecord userRecord,
        Guid tenantId,
        CancellationToken cancellationToken)
    {
        var email = userRecord.Email.Trim();

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
            TenantId = tenantId,
            FirstName = userRecord.FirstName,
            LastName = userRecord.LastName,
            NationalIdentityNo = userRecord.NationalIdentityNo,
            TaxIdNo = userRecord.TaxIdNo,
            EmployeeId = userRecord.EmployeeId,
            Title = userRecord.Title,
            IsDeactivated = false
        };

        var createResult = await userManager.CreateAsync(newUser);
        if (!createResult.Succeeded)
        {
            var errorMsg = string.Join(", ", createResult.Errors.Select(e => e.Description));
            LogUserCreationFailed(logger, email, errorMsg);
            return new UserCreationResult(email, UserCreationStatus.Failed, errorMsg);
        }

        var token = await userManager.GeneratePasswordResetTokenAsync(newUser);

        var emailCommand = new SendInvitationEmailCommand(email, token);
        var emailResult = await mediator.Send(emailCommand, cancellationToken);

        if (emailResult.Success)
        {
            return new UserCreationResult(email, UserCreationStatus.Created);
        }
        else
        {
            LogEmailSendFailed(logger, email, emailResult.Message ?? "UNKNOWN_ERROR", null);
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
