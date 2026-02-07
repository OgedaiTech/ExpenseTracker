using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.Create;

internal partial class CreateExpenseEndpoint
  (ICreateExpenseService createExpenseService,
  ILogger<CreateExpenseEndpoint> logger) : Endpoint<CreateExpenseRequest>
{
  public override void Configure()
  {
    Post("/expenses");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CreateExpenseRequest request, CancellationToken ct)
  {
    var userId = User.Claims.First(x => x.Type == "UserId").Value;
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;
    var serviceResult = await createExpenseService.CreateExpenseAsync(request.Name, userId, tenantId, ct);
    if (!serviceResult.Success)
    {
      if (serviceResult.Message == CreateExpenseConstants.ExpenseNameCannotBeEmpty)
      {
        LogExpenseNameCannotBeEmpty(logger, userId, tenantId, null);
      }
      else if (serviceResult.Message == CreateExpenseConstants.ExpenseNameCannotExceed128Characters)
      {
        LogExpenseNameCannotExceed128Characters(logger, userId, tenantId, null);
      }
      else
      {
        LogFailedToCreateExpense(logger, userId, tenantId, serviceResult.Message ?? "Unknown error", null);
      }
      var problem = Results.Problem(
        detail: serviceResult.Message,
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }

    LogSuccessfullyCreatedExpense(logger, request.Name, userId, null);
    await Send.CreatedAtAsync("expenses", cancellation: ct);
  }
}

