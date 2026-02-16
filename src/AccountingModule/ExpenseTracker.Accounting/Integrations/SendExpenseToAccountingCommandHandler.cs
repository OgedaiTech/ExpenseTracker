using ExpenseTracker.Accounting.Contracts;
using ExpenseTracker.Accounting.Data;
using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Accounting.Integrations;

public partial class SendExpenseToAccountingCommandHandler(
    AccountingDbContext dbContext,
    IEnumerable<IAccountingProvider> providers,
    IMediator mediator,
    ILogger<SendExpenseToAccountingCommandHandler> logger)
  : IRequestHandler<SendExpenseToAccountingCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(
      SendExpenseToAccountingCommand request,
      CancellationToken cancellationToken)
  {
    var tenantSettings = await dbContext.TenantAccountingSettings
        .FirstOrDefaultAsync(s => s.TenantId == request.TenantId, cancellationToken);

    if (tenantSettings is null || tenantSettings.Provider == "None")
    {
      LogSkipped(logger, request.ExpenseId, request.TenantId);
      return new ServiceResult();
    }

    var provider = providers.FirstOrDefault(p =>
        p.ProviderName.Equals(tenantSettings.Provider, StringComparison.OrdinalIgnoreCase));

    if (provider is null)
    {
      LogProviderNotFound(logger, request.ExpenseId, tenantSettings.Provider);
      return new ServiceResult();
    }

    var receiptsResult = await mediator.Send(
        new GetReceiptsByExpenseIdQuery(request.ExpenseId),
        cancellationToken);

    if (!receiptsResult.Success)
    {
      LogReceiptFetchFailed(logger, request.ExpenseId);
      return new ServiceResult();
    }

    var receipts = receiptsResult.Data ?? [];

    LogSendingToAccounting(logger, request.ExpenseId, tenantSettings.Provider, receipts.Count);

    var result = await provider.SendExpenseAsync(request, tenantSettings, receipts, cancellationToken);

    if (!result.Success)
    {
      LogAccountingFailed(logger, request.ExpenseId, tenantSettings.Provider, result.Message);
    }

    return new ServiceResult();
  }

  [LoggerMessage(EventId = 1001, Level = LogLevel.Information,
      Message = "Accounting not configured for tenant {TenantId} - skipping sync for expense {ExpenseId}")]
  private static partial void LogSkipped(ILogger l, Guid expenseId, Guid tenantId);

  [LoggerMessage(EventId = 1002, Level = LogLevel.Warning,
      Message = "Accounting provider '{Provider}' not registered - skipping sync for expense {ExpenseId}")]
  private static partial void LogProviderNotFound(ILogger l, Guid expenseId, string provider);

  [LoggerMessage(EventId = 1003, Level = LogLevel.Warning,
      Message = "Failed to fetch receipts for expense {ExpenseId} before accounting sync")]
  private static partial void LogReceiptFetchFailed(ILogger l, Guid expenseId);

  [LoggerMessage(EventId = 1004, Level = LogLevel.Information,
      Message = "Sending expense {ExpenseId} to {Provider} with {ReceiptCount} receipts")]
  private static partial void LogSendingToAccounting(ILogger l, Guid expenseId, string provider, int receiptCount);

  [LoggerMessage(EventId = 1005, Level = LogLevel.Warning,
      Message = "Accounting sync failed for expense {ExpenseId} via {Provider}: {Message}")]
  private static partial void LogAccountingFailed(ILogger l, Guid expenseId, string provider, string? message);
}
