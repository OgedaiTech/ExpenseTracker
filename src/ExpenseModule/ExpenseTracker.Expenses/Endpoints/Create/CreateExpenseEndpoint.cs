using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.Create;

internal class CreateExpenseEndpoint
  (ICreateExpenseService createExpenseService) : Endpoint<CreateExpenseRequest>
{
  override public void Configure()
  {
    Post("/expenses");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateExpenseRequest request, CancellationToken ct)
  {
    var serviceResult = await createExpenseService.CreateExpenseAsync(request.Name, ct);
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
    await Send.CreatedAtAsync("expenses", cancellation: ct);
  }
}

