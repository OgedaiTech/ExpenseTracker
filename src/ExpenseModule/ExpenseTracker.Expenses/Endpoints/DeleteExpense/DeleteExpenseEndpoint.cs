using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

internal class DeleteExpenseEndpoint(IDeleteExpenseService deleteExpenseService)
  : EndpointWithoutRequest
{
  public override void Configure()
  {
    Delete("/expenses/{expenseId:guid}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await deleteExpenseService.DeleteExpenseAsync(
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

    await Send.NoContentAsync(ct);
  }
}
