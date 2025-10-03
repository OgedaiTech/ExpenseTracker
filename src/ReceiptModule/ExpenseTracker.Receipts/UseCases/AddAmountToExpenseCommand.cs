using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Receipts.UseCases;

public record AddAmountToExpenseCommand(Guid ExpenseId, decimal Amount) : IRequest<ServiceResult<Guid>>;
