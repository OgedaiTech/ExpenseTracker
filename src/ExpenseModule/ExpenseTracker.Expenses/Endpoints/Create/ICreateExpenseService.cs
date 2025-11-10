using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.Create;

public interface ICreateExpenseService
{
  Task<ServiceResult> CreateExpenseAsync(string name, string userId, CancellationToken cancellationToken);
}
