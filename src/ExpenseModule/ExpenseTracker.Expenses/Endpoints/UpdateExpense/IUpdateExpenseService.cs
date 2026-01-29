using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

public interface IUpdateExpenseService
{
  Task<ServiceResult<UpdateExpenseResponse>> UpdateExpenseAsync(
    Guid expenseId,
    string name,
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
