using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

internal class RejectExpenseEndpoint(IRejectExpenseService rejectExpenseService)
  : Endpoint<RejectExpenseRequest, RejectExpenseResponse>
{
  public override void Configure()
  {
    Post("/expenses/{expenseId:guid}/reject");
    Claims("EmailAddress");
    Roles("Approver", "TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(RejectExpenseRequest request, CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await rejectExpenseService.RejectExpenseAsync(
      expenseId,
      request.RejectionReason,
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
