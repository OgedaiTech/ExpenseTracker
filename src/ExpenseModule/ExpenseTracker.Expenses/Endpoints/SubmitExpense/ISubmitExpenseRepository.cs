namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public interface ISubmitExpenseRepository
{
  Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken);
  Task<bool> IsUserApproverAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken);
  Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken);
}
