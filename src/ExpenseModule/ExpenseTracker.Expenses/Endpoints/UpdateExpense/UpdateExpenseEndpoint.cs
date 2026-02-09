using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

internal partial class UpdateExpenseEndpoint(
  IUpdateExpenseService updateExpenseService,
  ILogger<UpdateExpenseEndpoint> logger)
  : Endpoint<UpdateExpenseRequest, UpdateExpenseResponse>
{
  public override void Configure()
  {
    Put("/expenses/{expenseId:guid}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(UpdateExpenseRequest request, CancellationToken ct)
  {
    var expenseId = Route<Guid>("expenseId");
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var serviceResult = await updateExpenseService.UpdateExpenseAsync(
      expenseId,
      request.Name,
      userId,
      tenantId,
      ct);

    if (!serviceResult.Success)
    {

      var statusCode = serviceResult.Message switch
      {
        UpdateExpenseConstants.ExpenseNotFoundErrorMessage => StatusCodes.Status404NotFound,
        UpdateExpenseConstants.ExpenseNameEmptyErrorMessage => StatusCodes.Status400BadRequest,
        UpdateExpenseConstants.ExpenseNameTooLongErrorMessage => StatusCodes.Status400BadRequest,
        UpdateExpenseConstants.ExpenseOwnershipErrorMessage => StatusCodes.Status403Forbidden,
        UpdateExpenseConstants.ExpenseStatusImmutableErrorMessage => StatusCodes.Status400BadRequest,
        _ => StatusCodes.Status400BadRequest
      };

      LogFailedToUpdateExpense(logger, serviceResult.Message ?? "Unknown error", expenseId.ToString(), userId, null);

      var problem = Results.Problem(
        detail: serviceResult.Message,
        statusCode: statusCode,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }

    LogSuccessfullyUpdatedExpense(logger, expenseId.ToString(), userId, null);
    await Send.OkAsync(serviceResult.Data!, ct);
  }
}
