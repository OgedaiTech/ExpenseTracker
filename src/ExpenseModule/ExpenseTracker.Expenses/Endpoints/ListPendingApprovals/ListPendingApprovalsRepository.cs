using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public class ListPendingApprovalsRepository(ExpenseDbContext dbContext) : IListPendingApprovalsRepository
{
  public async Task<List<Expense>> GetPendingApprovalsAsync(Guid approverId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await dbContext.Expenses
      .Where(e => e.SubmittedToApproverId == approverId
        && e.TenantId == tenantId
        && e.Status == ExpenseStatus.Submitted)
      .OrderByDescending(e => e.SubmittedAt)
      .ToListAsync(cancellationToken);
  }
}
