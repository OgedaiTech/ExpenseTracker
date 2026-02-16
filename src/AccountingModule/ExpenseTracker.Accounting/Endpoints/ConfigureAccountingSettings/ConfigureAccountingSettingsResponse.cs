namespace ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;

public class ConfigureAccountingSettingsResponse
{
  public Guid TenantId { get; set; }
  public string Provider { get; set; } = string.Empty;
}
