using ExpenseTracker.Accounting.Contracts;
using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;

namespace ExpenseTracker.Accounting;

public interface IAccountingProvider
{
  string ProviderName { get; }

  Task<ServiceResult> SendExpenseAsync(
      SendExpenseToAccountingCommand command,
      TenantAccountingSettings tenantSettings,
      List<ReceiptSummaryDto> receipts,
      CancellationToken cancellationToken);
}
