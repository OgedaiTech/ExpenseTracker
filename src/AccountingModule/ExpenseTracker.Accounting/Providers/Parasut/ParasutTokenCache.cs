using System.Collections.Concurrent;

namespace ExpenseTracker.Accounting.Providers.Parasut;

public class ParasutTokenCache
{
  private readonly ConcurrentDictionary<Guid, CachedToken> _tokens = new();

  public async Task<string?> GetOrRefreshTokenAsync(
      Guid tenantId,
      Func<CancellationToken, Task<(string Token, int ExpiresIn)?>> fetchTokenAsync,
      CancellationToken cancellationToken)
  {
    var cached = _tokens.GetOrAdd(tenantId, _ => new CachedToken());

    if (!cached.IsExpired)
    {
      return cached.AccessToken;
    }

    await cached.Lock.WaitAsync(cancellationToken);
    try
    {
      if (!cached.IsExpired)
      {
        return cached.AccessToken;
      }

      var result = await fetchTokenAsync(cancellationToken);
      if (result is null)
      {
        return null;
      }

      cached.SetToken(result.Value.Token, result.Value.ExpiresIn);
      return cached.AccessToken;
    }
    finally
    {
      cached.Lock.Release();
    }
  }

  private sealed class CachedToken
  {
    private DateTime _expiresAt = DateTime.MinValue;

    public string? AccessToken { get; private set; }
    public bool IsExpired => DateTime.UtcNow >= _expiresAt;
    public SemaphoreSlim Lock { get; } = new(1, 1);

    public void SetToken(string accessToken, int expiresInSeconds)
    {
      AccessToken = accessToken;
      _expiresAt = DateTime.UtcNow.AddSeconds(expiresInSeconds - 60);
    }
  }
}
