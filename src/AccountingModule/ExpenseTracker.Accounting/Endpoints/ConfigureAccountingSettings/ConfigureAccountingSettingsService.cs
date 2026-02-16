using System.Text.Json;
using ExpenseTracker.Accounting.Data;
using ExpenseTracker.Core;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;

public class ConfigureAccountingSettingsService(AccountingDbContext dbContext)
  : IConfigureAccountingSettingsService
{
  public async Task<ServiceResult<ConfigureAccountingSettingsResponse>> ConfigureAsync(
      ConfigureAccountingSettingsRequest request,
      Guid tenantId,
      CancellationToken cancellationToken)
  {
    var existing = await dbContext.TenantAccountingSettings
        .FirstOrDefaultAsync(s => s.TenantId == tenantId, cancellationToken);

    if (existing is null)
    {
      existing = new TenantAccountingSettings { TenantId = tenantId };
      dbContext.TenantAccountingSettings.Add(existing);
    }

    existing.Provider = request.Provider;
    existing.CredentialsJson = request.Credentials is not null
        ? JsonSerializer.Serialize(request.Credentials)
        : null;

    await dbContext.SaveChangesAsync(cancellationToken);

    return new ServiceResult<ConfigureAccountingSettingsResponse>(
        new ConfigureAccountingSettingsResponse
        {
          TenantId = tenantId,
          Provider = existing.Provider
        });
  }
}
