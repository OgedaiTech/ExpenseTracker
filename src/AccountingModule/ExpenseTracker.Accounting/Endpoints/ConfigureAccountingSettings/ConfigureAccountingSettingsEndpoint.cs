using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;

internal class ConfigureAccountingSettingsEndpoint(
    IConfigureAccountingSettingsService configureAccountingSettingsService)
  : Endpoint<ConfigureAccountingSettingsRequest, ConfigureAccountingSettingsResponse>
{
  public override void Configure()
  {
    Put("/accounting/settings");
    Claims("EmailAddress");
    Roles("TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(ConfigureAccountingSettingsRequest request, CancellationToken ct)
  {
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var result = await configureAccountingSettingsService.ConfigureAsync(
        request,
        Guid.Parse(tenantId),
        ct);

    if (!result.Success)
    {
      var problem = Results.Problem(
          title: "Invalid request",
          detail: result.Message,
          statusCode: StatusCodes.Status400BadRequest,
          instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }

    await Send.OkAsync(result.Data!, ct);
  }
}
