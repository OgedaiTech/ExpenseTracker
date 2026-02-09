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
      return new ServiceResult<UpdateExpenseResponse>(UpdateExpenseConstants.ExpenseNameEmptyErrorMessage);
    }

    if (name.Length > 128)
    {
      return new ServiceResult<UpdateExpenseResponse>(UpdateExpenseConstants.ExpenseNameTooLongErrorMessage);
    }

    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Get the expense
    var expense = await updateExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<UpdateExpenseResponse>(UpdateExpenseConstants.ExpenseNotFoundErrorMessage);
    }

    // Validate that the expense belongs to the user
    if (expense.CreatedByUserId != userGuid)
    {
      return new ServiceResult<UpdateExpenseResponse>(UpdateExpenseConstants.ExpenseOwnershipErrorMessage);
    }

    // Validate that the expense is in Draft or Rejected status (immutability check)
    if (expense.Status != ExpenseStatus.Draft && expense.Status != ExpenseStatus.Rejected)
    {
      return new ServiceResult<UpdateExpenseResponse>(
        UpdateExpenseConstants.ExpenseStatusImmutableErrorMessage);
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
