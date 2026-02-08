using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

internal partial class ListExpenseReceiptsEndpoint
  (IListExpenseReceiptsService service,
  ILogger<ListExpenseReceiptsEndpoint> logger)
  : Endpoint<ListExpenseReceiptsRequest, ListExpenseReceiptsResponse>
{
  public override void Configure()
  {
    Get("/expenses/{ExpenseId:guid}/receipts");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(
    ListExpenseReceiptsRequest request,
    CancellationToken ct)
  {
    try
    {
      var serviceResult = await service.ListExpenseReceiptsAsync(request.ExpenseId, ct);
      if (serviceResult.Success)
      {
        LogSuccessfullyRetrievedExpenseReceipts(logger, serviceResult.Data!.Count, request.ExpenseId, null);
        var response = new ListExpenseReceiptsResponse
        {
          Items = serviceResult.Data,
          TotalCount = serviceResult.Data!.Count
        };
        await Send.OkAsync(response, ct);
        return;
      }
    }
    catch (Exception)
    {
      LogErrorWhenTryingToListExpenseReceipts(logger, request.ExpenseId, null);
      var problem = Results.Problem(
        detail: "An unexpected error occurred while trying to retrieve the receipts for the specified expense. Please try again later.",
        statusCode: StatusCodes.Status500InternalServerError,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }
  }
}
