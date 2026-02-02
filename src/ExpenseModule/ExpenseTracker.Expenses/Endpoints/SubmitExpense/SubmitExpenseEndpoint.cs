using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

internal class SubmitExpenseEndpoint(ISubmitExpenseService submitExpenseService)
  : Endpoint<SubmitExpenseRequest, SubmitExpenseResponse>
{
  public override void Configure()
  {
    Post("/expenses/{expenseId:guid}/submit");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(SubmitExpenseRequest request, CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await submitExpenseService.SubmitExpenseAsync(
      expenseId,
      request.ApproverId,
      userId,
      tenantId,
      ct);

    if (!serviceResult.Success)
    {
      var statusCode = serviceResult.Message switch
      {
        SubmitExpenseConstants.FailedToRetrieveSubmitterEmail => StatusCodes.Status500InternalServerError,
        SubmitExpenseConstants.FailedToRetrieveApproverEmail => StatusCodes.Status500InternalServerError,
        SubmitExpenseConstants.SelectedApproverIsNotInYourOrganization => StatusCodes.Status400BadRequest,
        SubmitExpenseConstants.CannotSubmitExpenseWithoutReceipts => StatusCodes.Status400BadRequest,
        SubmitExpenseConstants.ExpenseNotFound => StatusCodes.Status400BadRequest,
        SubmitExpenseConstants.OnlyDraftOrRejectedExpensesCanBeSubmitted => StatusCodes.Status400BadRequest,
        SubmitExpenseConstants.YouCanNotSubmitExpensesToYourselfForApproval => StatusCodes.Status400BadRequest,
        SubmitExpenseConstants.YouCanOnlySubmitYourOwnExpenses => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status400BadRequest
      };

      var problem = Results.Problem(
          detail: serviceResult.Message,
          statusCode: statusCode,
          instance: HttpContext.Request.Path);

      await Send.ResultAsync(problem);
      return;
    }

    await Send.OkAsync(serviceResult.Data!, ct);
  }
}
