using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

internal class GetExpenseByIdEndpoint(
    IGetExpenseByIdService service,
    ILogger<GetExpenseByIdEndpoint> logger) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Get("/expenses/{id}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    try
    {
      var expenseId = Route<string>("id") ?? string.Empty;
      var userId = User.Claims.First(c => c.Type == "UserId").Value;
      var tenantId = User.Claims.First(c => c.Type == "TenantId").Value;

      var serviceResult = await service.GetExpenseByIdAsync(expenseId, userId, tenantId, ct);

      if (serviceResult.Success)
      {
        var response = new GetExpenseByIdResponse(
            new ExpenseDto(
                serviceResult.Data!.Id,
                serviceResult.Data!.Name,
                serviceResult.Data!.Amount,
                serviceResult.Data!.CreatedAt));

        await Send.OkAsync(response, ct);
        return;
      }
      else
      {
        var statusCode = serviceResult.Message switch
        {
          "Expense not found." => StatusCodes.Status404NotFound,
          "Invalid expense ID." => StatusCodes.Status400BadRequest,
          _ => StatusCodes.Status400BadRequest
        };

        if (logger.IsEnabled(LogLevel.Warning))
        {
          logger.LogWarning("Failed to get expense. Reason: {Reason}", serviceResult.Message);
        }

        var problem = Results.Problem(
            title: statusCode == StatusCodes.Status404NotFound ? "Not Found" : "Invalid request",
            detail: serviceResult.Message,
            statusCode: statusCode,
            instance: HttpContext.Request.Path);
        await Send.ResultAsync(problem);
        return;
      }
    }
    catch (Exception)
    {
      var problem = Results.Problem(
          title: "Internal Server error",
          detail: "An unexpected error occurred while processing the request.",
          statusCode: StatusCodes.Status500InternalServerError,
          instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }
  }
}
