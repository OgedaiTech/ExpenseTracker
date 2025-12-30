namespace ExpenseTrackerUI.Services;

public interface ITokenStorageService
{
  Task StoreTokensAsync(string accessToken, string refreshToken);
  Task<string?> GetAccessTokenAsync();
  Task<string?> GetRefreshTokenAsync();
  Task ClearTokensAsync();
}
