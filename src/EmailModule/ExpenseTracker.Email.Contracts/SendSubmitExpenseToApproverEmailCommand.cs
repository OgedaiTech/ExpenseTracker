using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Email.Contracts;

public record SendSubmitExpenseToApproverEmailCommand(
    string ExpenseName,
    Guid ApproverId,
    string ApproverEmail,
    string SubmitterName) : IRequest<ServiceResult>;
