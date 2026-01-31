namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public interface IListPendingApprovalsRepository
{
  Task<List<Expense>> GetPendingApprovalsAsync(Guid approverId, Guid tenantId, CancellationToken cancellationToken);
}
