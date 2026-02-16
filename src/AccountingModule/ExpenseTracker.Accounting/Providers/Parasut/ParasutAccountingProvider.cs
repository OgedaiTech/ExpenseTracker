using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ExpenseTracker.Accounting.Contracts;
using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Accounting.Providers.Parasut;

internal record ParasutCredentials(string CompanyId, string Username, string Password);

public partial class ParasutAccountingProvider(
    IHttpClientFactory httpClientFactory,
    IOptions<ParasutGlobalSettings> globalSettings,
    ParasutTokenCache tokenCache,
    ILogger<ParasutAccountingProvider> logger) : IAccountingProvider
{
  private readonly ParasutGlobalSettings _globalSettings = globalSettings.Value;

  private static readonly JsonSerializerOptions JsonOptions = new()
  {
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
  };

  public string ProviderName => "Parasut";

  public async Task<ServiceResult> SendExpenseAsync(
      SendExpenseToAccountingCommand command,
      TenantAccountingSettings tenantSettings,
      List<ReceiptSummaryDto> receipts,
      CancellationToken cancellationToken)
  {
    var credentials = DeserializeCredentials(tenantSettings);
    if (credentials is null)
    {
      LogCredentialsMissing(logger, command.ExpenseId, tenantSettings.TenantId);
      return new ServiceResult("Parasut credentials not configured for tenant");
    }

    var token = await tokenCache.GetOrRefreshTokenAsync(
        tenantSettings.TenantId,
        ct => FetchTokenAsync(tenantSettings.TenantId, credentials, ct),
        cancellationToken);

    if (token is null)
    {
      LogTokenFailed(logger, command.ExpenseId);
      return new ServiceResult("Failed to obtain Parasut OAuth2 token");
    }

    var requestBody = BuildPurchaseBillRequest(command, receipts);
    var client = httpClientFactory.CreateClient("Parasut");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

    var url = $"{_globalSettings.BaseUrl}/{credentials.CompanyId}/purchase_bills";

    LogSendingBill(logger, command.ExpenseId, url);

    using var response = await client.PostAsJsonAsync(url, requestBody, JsonOptions, cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
      LogBillCreationFailed(logger, command.ExpenseId, (int)response.StatusCode, errorContent);
      return new ServiceResult($"Parasut purchase bill creation failed: {response.StatusCode}");
    }

    LogBillCreated(logger, command.ExpenseId);
    return new ServiceResult();
  }

  private static ParasutCredentials? DeserializeCredentials(TenantAccountingSettings settings)
  {
    if (string.IsNullOrEmpty(settings.CredentialsJson))
      return null;

    try
    {
      return JsonSerializer.Deserialize<ParasutCredentials>(settings.CredentialsJson,
          new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
    catch (JsonException)
    {
      return null;
    }
  }

  private async Task<(string Token, int ExpiresIn)?> FetchTokenAsync(
      Guid tenantId,
      ParasutCredentials credentials,
      CancellationToken cancellationToken)
  {
    LogRefreshingToken(logger, tenantId);

    var client = httpClientFactory.CreateClient("Parasut");
    var tokenRequest = new Dictionary<string, string>
    {
      ["grant_type"]    = "password",
      ["client_id"]     = _globalSettings.ClientId,
      ["client_secret"] = _globalSettings.ClientSecret,
      ["username"]      = credentials.Username,
      ["password"]      = credentials.Password,
      ["redirect_uri"]  = _globalSettings.RedirectUri
    };

    using var response = await client.PostAsync(
        _globalSettings.TokenUrl,
        new FormUrlEncodedContent(tokenRequest),
        cancellationToken);

    if (!response.IsSuccessStatusCode)
    {
      LogTokenRequestFailed(logger, tenantId, (int)response.StatusCode);
      return null;
    }

    var tokenResponse = await response.Content.ReadFromJsonAsync<ParasutTokenResponse>(
        cancellationToken: cancellationToken);

    if (tokenResponse is null || string.IsNullOrEmpty(tokenResponse.AccessToken))
    {
      LogTokenResponseEmpty(logger, tenantId);
      return null;
    }

    LogTokenRefreshed(logger, tenantId);
    return (tokenResponse.AccessToken, tokenResponse.ExpiresIn);
  }

  private static ParasutPurchaseBillRequest BuildPurchaseBillRequest(
      SendExpenseToAccountingCommand command,
      List<ReceiptSummaryDto> receipts)
  {
    var issueDate = command.ApprovedAt.ToString("yyyy-MM-dd");

    List<ParasutPurchaseBillDetailAttributes> details;

    if (receipts.Count > 0)
    {
      details = receipts.Select(r => new ParasutPurchaseBillDetailAttributes
      {
        Description = string.IsNullOrEmpty(r.Vendor)
            ? $"Receipt {r.ReceiptNo} ({r.Date:yyyy-MM-dd})"
            : $"{r.Vendor} - {r.ReceiptNo} ({r.Date:yyyy-MM-dd})",
        Quantity = 1,
        UnitPrice = r.Amount
      }).ToList();
    }
    else
    {
      details = [new ParasutPurchaseBillDetailAttributes
      {
        Description = command.ExpenseName,
        Quantity = 1,
        UnitPrice = command.ExpenseAmount
      }];
    }

    return new ParasutPurchaseBillRequest
    {
      Data = new ParasutPurchaseBillData
      {
        Attributes = new ParasutPurchaseBillAttributes
        {
          Description = command.ExpenseName,
          IssueDate = issueDate,
          DueDate = issueDate,
          Currency = "TRL",
          DetailsAttributes = details
        }
      }
    };
  }

  [LoggerMessage(EventId = 2000, Level = LogLevel.Warning,
      Message = "Parasut credentials not configured for expense {ExpenseId} / tenant {TenantId}")]
  private static partial void LogCredentialsMissing(ILogger l, Guid expenseId, Guid tenantId);

  [LoggerMessage(EventId = 2001, Level = LogLevel.Information,
      Message = "Sending expense {ExpenseId} to Parasut at {Url}")]
  private static partial void LogSendingBill(ILogger l, Guid expenseId, string url);

  [LoggerMessage(EventId = 2002, Level = LogLevel.Error,
      Message = "Parasut purchase bill creation failed for expense {ExpenseId}: HTTP {StatusCode} - {ErrorContent}")]
  private static partial void LogBillCreationFailed(ILogger l, Guid expenseId, int statusCode, string errorContent);

  [LoggerMessage(EventId = 2003, Level = LogLevel.Information,
      Message = "Parasut purchase bill created successfully for expense {ExpenseId}")]
  private static partial void LogBillCreated(ILogger l, Guid expenseId);

  [LoggerMessage(EventId = 2004, Level = LogLevel.Warning,
      Message = "Failed to obtain Parasut token for expense {ExpenseId}")]
  private static partial void LogTokenFailed(ILogger l, Guid expenseId);

  [LoggerMessage(EventId = 2005, Level = LogLevel.Information,
      Message = "Refreshing Parasut OAuth2 token for tenant {TenantId}")]
  private static partial void LogRefreshingToken(ILogger l, Guid tenantId);

  [LoggerMessage(EventId = 2006, Level = LogLevel.Error,
      Message = "Parasut OAuth2 token request failed for tenant {TenantId}: HTTP {StatusCode}")]
  private static partial void LogTokenRequestFailed(ILogger l, Guid tenantId, int statusCode);

  [LoggerMessage(EventId = 2007, Level = LogLevel.Error,
      Message = "Parasut OAuth2 token response was empty for tenant {TenantId}")]
  private static partial void LogTokenResponseEmpty(ILogger l, Guid tenantId);

  [LoggerMessage(EventId = 2008, Level = LogLevel.Information,
      Message = "Parasut OAuth2 token refreshed successfully for tenant {TenantId}")]
  private static partial void LogTokenRefreshed(ILogger l, Guid tenantId);
}
