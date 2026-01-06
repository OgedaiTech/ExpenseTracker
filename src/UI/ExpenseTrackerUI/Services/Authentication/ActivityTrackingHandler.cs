namespace ExpenseTrackerUI.Services.Authentication;

public class ActivityTrackingHandler(TokenRefreshService tokenRefreshService) : DelegatingHandler
{
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    // Record activity on every HTTP request
    tokenRefreshService.RecordActivity();

    // Check if token needs refresh before making the request
    await tokenRefreshService.RefreshTokenIfNeededAsync();

    return await base.SendAsync(request, cancellationToken);
  }
}
