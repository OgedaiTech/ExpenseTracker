namespace ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;

public class ConfigureAccountingSettingsRequest
{
  public string Provider { get; set; } = "None";
  public Dictionary<string, string>? Credentials { get; set; }
}
