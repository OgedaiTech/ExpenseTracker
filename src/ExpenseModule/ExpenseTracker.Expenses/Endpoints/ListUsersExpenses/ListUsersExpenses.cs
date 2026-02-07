using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

internal partial class ListUsersExpenses(
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
        LogSuccessfullyListedUsersExpenses(logger, userId, tenantId, serviceResult.Data!.Length, null);

        var response = new ListUsersExpensesResponse
        {
          Items = [.. serviceResult.Data!.Select(e => new ExpenseDto(e.Id, e.Name, e.Amount, e.CreatedAt, e.Status))],
          TotalCount = serviceResult.Data!.Length
        };
        await Send.OkAsync(response, ct);
        return;
      }
      else
      {
        var statusCode = serviceResult.Message switch
        {
          ListUsersConstants.UserDoesNotHaveAccess => StatusCodes.Status403Forbidden,
          _ => StatusCodes.Status400BadRequest
        };

        LogFailedToListUsersExpenses(logger, serviceResult.Message ?? "Unknown error", null);

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
