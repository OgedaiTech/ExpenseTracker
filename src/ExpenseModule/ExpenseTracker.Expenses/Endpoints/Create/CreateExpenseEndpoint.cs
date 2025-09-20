using FastEndpoints;

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
    await createExpenseService.CreateExpenseAsync(request.Name, ct);
    await Send.CreatedAtAsync("expenses", cancellation: ct);
  }
}

