using ExpenseTracker.Users.EmailService;
using FastEndpoints;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

internal partial class BulkCreateUsersEndpoint(
    IBulkCreateUsersService bulkCreateUsersService,
    IOptions<BulkUserCreationSettings> settings,
    ILogger<BulkCreateUsersEndpoint> logger)
    : Endpoint<BulkCreateUsersRequest, BulkCreateUsersResponse>
{
    private readonly BulkUserCreationSettings _settings = settings.Value;

    public override void Configure()
    {
        Post("/users/bulk");
        Roles("SystemAdmin", "TenantAdmin");
        AllowFileUploads();
    }

    public override async Task HandleAsync(BulkCreateUsersRequest req, CancellationToken ct)
    {
        var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

        if (req.CsvFile is null || req.CsvFile.Length == 0)
        {
            AddError("CSV_FILE_REQUIRED", "CSV file is required");
            ThrowIfAnyErrors();
            return;
        }

        if (req.CsvFile.Length > _settings.MaxFileSizeBytes)
        {
            AddError("FILE_TOO_LARGE", $"File size exceeds maximum allowed size of {_settings.MaxFileSizeBytes} bytes");
            ThrowIfAnyErrors();
        }

        var contentType = req.CsvFile.ContentType.ToLowerInvariant();
        if (contentType is not ("text/csv" or "application/octet-stream" or "text/plain"))
        {
            AddError("INVALID_FILE_TYPE", "File must be a CSV file");
            ThrowIfAnyErrors();
        }

        LogBulkCreateRequested(logger, Guid.Parse(tenantId), req.CsvFile.Length);

        await using var stream = req.CsvFile.OpenReadStream();
        var result = await bulkCreateUsersService.CreateUsersFromCsvAsync(
            stream,
            Guid.Parse(tenantId),
            ct);

        if (!result.Success)
        {
            AddError(result.ErrorCode!, result.ErrorMessage!);
            ThrowIfAnyErrors();
        }

        await Send.OkAsync(result.Response!, ct);
    }

    [LoggerMessage(
        EventId = 110,
        Level = LogLevel.Information,
        Message = "Bulk user creation requested. TenantId: {TenantId}, FileSize: {FileSize}")]
    private static partial void LogBulkCreateRequested(ILogger logger, Guid tenantId, long fileSize);
}
