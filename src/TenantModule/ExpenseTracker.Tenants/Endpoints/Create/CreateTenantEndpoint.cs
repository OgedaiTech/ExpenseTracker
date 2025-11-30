using FastEndpoints;

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
    var serviceResult = await createTenantService.CreateTenantAsync(request, ct);

    if (serviceResult.Success)
    {
      await Send.CreatedAtAsync("tenants", cancellation: ct);
    }
  }
}
