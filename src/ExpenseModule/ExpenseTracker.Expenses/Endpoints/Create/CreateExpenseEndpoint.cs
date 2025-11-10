using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.Create;

internal class CreateExpenseEndpoint
  (ICreateExpenseService createExpenseService) : Endpoint<CreateExpenseRequest>
{
  public override void Configure()
  {
    Post("/expenses");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CreateExpenseRequest request, CancellationToken ct)
  {
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var serviceResult = await createExpenseService.CreateExpenseAsync(request.Name, userId, ct);
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

