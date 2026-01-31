using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

internal class UpdateExpenseEndpoint(IUpdateExpenseService updateExpenseService)
  : Endpoint<UpdateExpenseRequest, UpdateExpenseResponse>
{
  public override void Configure()
  {
    Put("/expenses/{expenseId:guid}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(UpdateExpenseRequest request, CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await updateExpenseService.UpdateExpenseAsync(
      expenseId,
      request.Name,
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
