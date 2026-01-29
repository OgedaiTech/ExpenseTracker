using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

internal class ListPendingApprovalsEndpoint(IListPendingApprovalsService listPendingApprovalsService)
  : EndpointWithoutRequest<List<ListPendingApprovalsResponse>>
{
  public override void Configure()
  {
    Get("/expenses/pending-approvals");
    Claims("EmailAddress");
    Roles("Approver", "TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await listPendingApprovalsService.ListPendingApprovalsAsync(
      userId,
      tenantId,
      ct);

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

    await Send.OkAsync(serviceResult.Data!, ct);
  }
}
