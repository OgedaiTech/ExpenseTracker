using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

internal class GetExpenseByIdEndpoint(
    IGetExpenseByIdService service,
    ILogger<GetExpenseByIdEndpoint> logger) : Endpoint<GetExpenseByIdRequest, GetExpenseByIdResponse>
{
  public override void Configure()
  {
    Get("/expenses/{id}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(GetExpenseByIdRequest request, CancellationToken ct)
  {
    try
    {
      var expenseId = Route<string>("id") ?? string.Empty;
      // if approver is getting expense on behalf of user, use createdByUserId query param otherwise use userId from token
      var createdByUserIdQuery = Query<string>("createdByUserId", false);
      var userId = !string.IsNullOrEmpty(createdByUserIdQuery)
        ? createdByUserIdQuery
        : (request.UserId?.ToString() ?? User.Claims.First(c => c.Type == "UserId").Value);
      var tenantId = User.Claims.First(c => c.Type == "TenantId").Value;

      var serviceResult = await service.GetExpenseByIdAsync(expenseId, userId, tenantId, ct);

      if (serviceResult.Success)
      {
        var response = new GetExpenseByIdResponse(
            new ExpenseDto(
                serviceResult.Data!.Id,
                serviceResult.Data!.Name,
                serviceResult.Data!.Amount,
                serviceResult.Data!.CreatedAt,
                serviceResult.Data!.Status));

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

public record GetExpenseByIdRequest(Guid? UserId);
