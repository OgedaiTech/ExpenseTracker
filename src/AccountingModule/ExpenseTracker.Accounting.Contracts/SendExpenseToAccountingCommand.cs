using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Accounting.Contracts;

public record SendExpenseToAccountingCommand(
    Guid ExpenseId,
    Guid TenantId,
    string ExpenseName,
    decimal ExpenseAmount,
    DateTime ApprovedAt) : IRequest<ServiceResult>;
