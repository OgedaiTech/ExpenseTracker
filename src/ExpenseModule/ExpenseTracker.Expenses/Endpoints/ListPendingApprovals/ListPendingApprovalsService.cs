using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public class ListPendingApprovalsService(IListPendingApprovalsRepository listPendingApprovalsRepository)
  : IListPendingApprovalsService
{
  public async Task<ServiceResult<List<ListPendingApprovalsResponse>>> ListPendingApprovalsAsync(
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    var expenses = await listPendingApprovalsRepository.GetPendingApprovalsAsync(
      userGuid,
      tenantGuid,
      cancellationToken);

    var response = expenses.Select(e => new ListPendingApprovalsResponse
    {
      Id = e.Id,
      Name = e.Name,
      Amount = e.Amount,
      CreatedByUserId = e.CreatedByUserId,
      SubmittedAt = e.SubmittedAt!.Value,
      CreatedAt = e.CreatedAt
    }).ToList();

    return new ServiceResult<List<ListPendingApprovalsResponse>>(response);
  }
}
