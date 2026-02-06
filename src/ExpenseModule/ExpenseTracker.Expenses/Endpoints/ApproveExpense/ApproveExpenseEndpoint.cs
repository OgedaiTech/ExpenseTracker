using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

internal class ApproveExpenseEndpoint(IApproveExpenseService approveExpenseService)
  : EndpointWithoutRequest<ApproveExpenseResponse>
{
  public override void Configure()
  {
    Post("/expenses/{expenseId:guid}/approve");
    Claims("EmailAddress");
    Roles("Approver", "TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await approveExpenseService.ApproveExpenseAsync(
      expenseId,
      userId,
      tenantId,
      ct);

    if (!serviceResult.Success)
    {
      var statusCode = serviceResult.Message switch
      {
        ApproveExpenseConstants.ExpenseNotFound => StatusCodes.Status404NotFound,
        ApproveExpenseConstants.OnlySubmittedExpensesCanBeApproved => StatusCodes.Status400BadRequest,
        ApproveExpenseConstants.YouAreNotAuthorizedToApproveThisExpense => StatusCodes.Status403Forbidden,
        ApproveExpenseConstants.FailedToRetrieveApproverEmail => StatusCodes.Status500InternalServerError,
        ApproveExpenseConstants.FailedToRetrieveSubmitterEmail => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status400BadRequest
      };
      var problem = Results.Problem(
        title: "Invalid request",
        detail: serviceResult.Message,
        statusCode: statusCode,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }

    await Send.OkAsync(serviceResult.Data!, ct);
  }
}
