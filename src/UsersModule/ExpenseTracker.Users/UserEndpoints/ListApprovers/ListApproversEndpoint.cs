using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Users.UserEndpoints.ListApprovers;

internal class ListApproversEndpoint(IListApproversService listApproversService)
  : EndpointWithoutRequest<List<ListApproversResponse>>
{
  public override void Configure()
  {
    Get("/users/approvers");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await listApproversService.ListApproversAsync(tenantId, ct);

    if (!serviceResult.Success)
    {
      var problem = Results.Problem(
        title: "Invalid request",
        detail: serviceResult.Message,
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }

    await Send.OkAsync(serviceResult.Data!, cancellation: ct);
  }
}
