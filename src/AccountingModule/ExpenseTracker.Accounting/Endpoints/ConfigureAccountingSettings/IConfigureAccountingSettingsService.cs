using ExpenseTracker.Core;

namespace ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;

public interface IConfigureAccountingSettingsService
{
  Task<ServiceResult<ConfigureAccountingSettingsResponse>> ConfigureAsync(
      ConfigureAccountingSettingsRequest request,
      Guid tenantId,
      CancellationToken cancellationToken);
}
