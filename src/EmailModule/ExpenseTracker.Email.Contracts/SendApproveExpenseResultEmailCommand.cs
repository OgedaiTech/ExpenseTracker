using System;
using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Email.Contracts;

public record SendApproveExpenseResultEmailCommand(
    string ExpenseTitle,
    string ApprovedByEmail,
    string SubmittedByEmail) : IRequest<ServiceResult>;
