using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Tenants.Endpoints.Create;

internal partial class CreateTenantEndpoint(
  ICreateTenantService createTenantService,
  ILogger<CreateTenantEndpoint> logger) : Endpoint<CreateTenantRequest>
{
  public override void Configure()
  {
    Post("/tenants");
    Roles("SystemAdmin");
  }

  public override async Task HandleAsync(CreateTenantRequest request, CancellationToken ct)
  {
    try
    {
      var serviceResult = await createTenantService.CreateTenantAsync(request, ct);

      if (!serviceResult.Success)
      {
        var statusCode = serviceResult.Message switch
        {
          CreateTenantConstants.TenantCodeIsRequired => StatusCodes.Status400BadRequest,
          CreateTenantConstants.CantCreateTenantAdminUser => StatusCodes.Status500InternalServerError,
          CreateTenantConstants.TenantCodeAlreadyExists => StatusCodes.Status409Conflict,
          _ => StatusCodes.Status500InternalServerError
        };

        LogErrorWhenTryingToCreateTenant(logger, request.Name, serviceResult.Message ?? "Unknown error", null);

        await Send.ResultAsync(Results.Problem(
          detail: serviceResult.Message,
          statusCode: statusCode));
        return;
      }

      LogSuccessfullyCreatedTenant(logger, request.Name, null);
      await Send.CreatedAtAsync("tenants", cancellation: ct);
    }
    catch (Exception ex)
    {
      AddError(ex.Message);
      ThrowIfAnyErrors();
    }
  }
}
