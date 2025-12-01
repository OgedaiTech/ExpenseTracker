using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Tenants.Endpoints.Create;

internal class CreateTenantEndpoint(
  ICreateTenantService createTenantService) : Endpoint<CreateTenantRequest>
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
          "TENANT_CODE_IS_REQUIRED" => StatusCodes.Status400BadRequest,
          "TENANT_CODE_ALREADY_EXISTS" => StatusCodes.Status409Conflict,
          _ => StatusCodes.Status500InternalServerError
        };

        await Send.ResultAsync(Results.Problem(
          detail: serviceResult.Message,
          statusCode: statusCode));
        return;
      }

      await Send.CreatedAtAsync("tenants", cancellation: ct);
    }
    catch (Exception ex)
    {
      AddError(ex.Message);
      ThrowIfAnyErrors();
    }
  }
}
