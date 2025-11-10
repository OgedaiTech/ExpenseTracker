namespace ExpenseTracker.Expenses.Endpoints.Create;

public interface ICreateExpenseRepository
{
  Task CreateExpenseAsync(string name, string userId, CancellationToken cancellationToken);
}
