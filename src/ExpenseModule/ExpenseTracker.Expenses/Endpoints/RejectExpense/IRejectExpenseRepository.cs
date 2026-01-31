namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

public interface IRejectExpenseRepository
{
  Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken);
  Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken);
}
