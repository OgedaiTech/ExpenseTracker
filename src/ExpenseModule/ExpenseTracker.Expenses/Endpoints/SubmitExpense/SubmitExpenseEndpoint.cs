using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

internal class SubmitExpenseEndpoint(ISubmitExpenseService submitExpenseService)
  : Endpoint<SubmitExpenseRequest, SubmitExpenseResponse>
{
  public override void Configure()
  {
    Post("/expenses/{expenseId:guid}/submit");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(SubmitExpenseRequest request, CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await submitExpenseService.SubmitExpenseAsync(
      expenseId,
      request.ApproverId,
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
