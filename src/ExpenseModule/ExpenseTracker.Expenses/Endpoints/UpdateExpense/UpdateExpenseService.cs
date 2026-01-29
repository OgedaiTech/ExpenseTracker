using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

public class UpdateExpenseService(IUpdateExpenseRepository updateExpenseRepository) : IUpdateExpenseService
{
  public async Task<ServiceResult<UpdateExpenseResponse>> UpdateExpenseAsync(
    Guid expenseId,
    string name,
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    // Validate name
    if (string.IsNullOrWhiteSpace(name))
    {
      return new ServiceResult<UpdateExpenseResponse>("Expense name cannot be empty");
    }

    if (name.Length > 128)
    {
      return new ServiceResult<UpdateExpenseResponse>("Expense name cannot exceed 128 characters");
    }

    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Get the expense
    var expense = await updateExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<UpdateExpenseResponse>("Expense not found");
    }

    // Validate that the expense belongs to the user
    if (expense.CreatedByUserId != userGuid)
    {
      return new ServiceResult<UpdateExpenseResponse>("You can only update your own expenses");
    }

    // Validate that the expense is in Draft or Rejected status (immutability check)
    if (expense.Status != ExpenseStatus.Draft && expense.Status != ExpenseStatus.Rejected)
    {
      return new ServiceResult<UpdateExpenseResponse>(
        "Cannot update expense. Only draft or rejected expenses can be modified.");
    }

    // Update the expense
    expense.Name = name;
    expense.UpdatedAt = DateTime.UtcNow;

    await updateExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    return new ServiceResult<UpdateExpenseResponse>(new UpdateExpenseResponse
    {
      Id = expense.Id,
      Name = expense.Name,
      Status = expense.Status,
      UpdatedAt = expense.UpdatedAt.Value
    });
  }
}
