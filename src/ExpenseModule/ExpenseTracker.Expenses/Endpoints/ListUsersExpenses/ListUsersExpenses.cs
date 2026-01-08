using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

internal class ListUsersExpenses(
  IListUsersExpensesService service,
  ILogger<ListUsersExpenses> logger) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Get("/expenses/users");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    try
    {
      var userId = User.Claims.First(c => c.Type == "UserId").Value;
      var tenantId = User.Claims.First(c => c.Type == "TenantId").Value;
      var serviceResult = await service.ListUsersExpensesAsync(userId, tenantId, ct);
      if (serviceResult.Success)
      {
        var response = new ListUsersExpensesResponse
        {
          Items = [.. serviceResult.Data!.Select(e => new ExpenseDto(e.Id, e.Name, e.Amount, e.CreatedAt))],
          TotalCount = serviceResult.Data!.Length
        };
        await Send.OkAsync(response, ct);
        return;
      }
      else
      {
        var statusCode = serviceResult.Message switch
        {
          "User does not have access to the requested expenses." => StatusCodes.Status403Forbidden,
          _ => StatusCodes.Status400BadRequest
        };

        if (logger.IsEnabled(LogLevel.Warning))
        {
          logger.LogWarning("Failed to list user's expenses. Reason: {Reason}", serviceResult.Message);
        }

        var problem = Results.Problem(
        title: "Invalid request",
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
