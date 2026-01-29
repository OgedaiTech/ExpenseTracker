using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

internal class ApproveExpenseEndpoint(IApproveExpenseService approveExpenseService)
  : EndpointWithoutRequest<ApproveExpenseResponse>
{
  public override void Configure()
  {
    Post("/expenses/{expenseId:guid}/approve");
    Claims("EmailAddress");
    Roles("Approver", "TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await approveExpenseService.ApproveExpenseAsync(
      expenseId,
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
