namespace ExpenseTracker.Expenses.Endpoints.Create;

public class CreateExpenseService(ICreateExpenseRepository createExpenseRepository) : ICreateExpenseService
{
  public Task CreateExpenseAsync(string name, CancellationToken cancellationToken)
  {
    return createExpenseRepository.CreateExpenseAsync(name, cancellationToken);
  }
}
