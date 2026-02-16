namespace ExpenseTracker.Accounting;

public class TenantAccountingSettings
{
  public Guid TenantId { get; set; }
  public string Provider { get; set; } = "None";
  public string? CredentialsJson { get; set; }
}
