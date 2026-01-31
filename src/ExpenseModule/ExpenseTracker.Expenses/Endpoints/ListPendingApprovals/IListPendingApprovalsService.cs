using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public interface IListPendingApprovalsService
{
  Task<ServiceResult<List<ListPendingApprovalsResponse>>> ListPendingApprovalsAsync(
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
