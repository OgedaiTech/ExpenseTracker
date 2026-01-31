using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

public class DeleteExpenseService(IDeleteExpenseRepository deleteExpenseRepository) : IDeleteExpenseService
{
  public async Task<ServiceResult> DeleteExpenseAsync(
    Guid expenseId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Get the expense
    var expense = await deleteExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult("Expense not found");
    }

    // Validate that the expense belongs to the user
    if (expense.CreatedByUserId != userGuid)
    {
      return new ServiceResult("You can only delete your own expenses");
    }

    // Validate that the expense is in Draft or Rejected status (immutability check)
    if (expense.Status != ExpenseStatus.Draft && expense.Status != ExpenseStatus.Rejected)
    {
      return new ServiceResult("Cannot delete expense. Only draft or rejected expenses can be deleted.");
    }

    // Soft delete the expense
    await deleteExpenseRepository.DeleteExpenseAsync(expense, cancellationToken);

    return new ServiceResult();
  }
}
