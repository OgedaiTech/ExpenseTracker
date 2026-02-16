namespace ExpenseTracker.Accounting.Providers.Parasut;

public class ParasutGlobalSettings
{
  public string BaseUrl { get; set; } = "https://api.parasut.com/v4";
  public string TokenUrl { get; set; } = "https://api.parasut.com/oauth/token";
  public string ClientId { get; set; } = string.Empty;
  public string ClientSecret { get; set; } = string.Empty;
  public string RedirectUri { get; set; } = "urn:ietf:wg:oauth:2.0:oob";
}
