using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Expenses.Contracts;

public record ExpenseAmountCommand(Guid ExpenseId, decimal Amount) : IRequest<ServiceResult<Guid>>;
