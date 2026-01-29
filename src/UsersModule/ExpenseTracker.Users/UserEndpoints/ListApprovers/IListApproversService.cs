using ExpenseTracker.Core;

namespace ExpenseTracker.Users.UserEndpoints.ListApprovers;

public interface IListApproversService
{
  Task<ServiceResult<List<ListApproversResponse>>> ListApproversAsync(string tenantId, CancellationToken cancellationToken);
}
