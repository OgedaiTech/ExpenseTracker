using FastEndpoints;

namespace ExpenseTracker.Expenses.Endpoints.Create;

internal class CreateExpenseEndpoint
  (ICreateExpenseService createExpenseService) : EndpointWithoutRequest
{
  override public void Configure()
  {
    Post("/expenses");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    await createExpenseService.CreateExpenseAsync();
    await Send.CreatedAtAsync("expenses", cancellation: ct);
  }
}

