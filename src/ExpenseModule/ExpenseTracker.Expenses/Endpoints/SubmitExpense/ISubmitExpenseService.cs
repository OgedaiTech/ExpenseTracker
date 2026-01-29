using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public interface ISubmitExpenseService
{
  Task<ServiceResult<SubmitExpenseResponse>> SubmitExpenseAsync(
    Guid expenseId,
    Guid approverId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
