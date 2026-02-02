using Microsoft.AspNetCore.Http;
using Serilog.Core;
using Serilog.Events;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Enriches log events with tenant and user context from JWT claims.
/// </summary>
public class TenantEnricher : ILogEventEnricher
{
  private readonly IHttpContextAccessor _httpContextAccessor;

  public TenantEnricher(IHttpContextAccessor httpContextAccessor)
  {
    _httpContextAccessor = httpContextAccessor;
  }

  public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
  {
    var httpContext = _httpContextAccessor.HttpContext;
    if (httpContext == null)
      return;

    // Extract TenantId from claims
    var tenantIdClaim = httpContext.User.FindFirst("TenantId");
    if (tenantIdClaim != null)
    {
      var property = propertyFactory.CreateProperty("TenantId", tenantIdClaim.Value);
      logEvent.AddPropertyIfAbsent(property);
    }

    // Extract UserId from claims (check "sub" or "UserId")
    var userIdClaim = httpContext.User.FindFirst("sub")
                     ?? httpContext.User.FindFirst("UserId");
    if (userIdClaim != null)
    {
      var property = propertyFactory.CreateProperty("UserId", userIdClaim.Value);
      logEvent.AddPropertyIfAbsent(property);
    }

    // Add request correlation ID
    if (httpContext.Request.Headers.TryGetValue("X-Correlation-Id", out var correlationId))
    {
      var property = propertyFactory.CreateProperty("CorrelationId", correlationId.ToString());
      logEvent.AddPropertyIfAbsent(property);
    }
  }
}
