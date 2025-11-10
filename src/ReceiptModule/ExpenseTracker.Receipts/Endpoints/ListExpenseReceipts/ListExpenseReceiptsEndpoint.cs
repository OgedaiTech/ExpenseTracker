using FastEndpoints;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

internal class ListExpenseReceiptsEndpoint
  (IListExpenseReceiptsService service)
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
    var serviceResult = await service.ListExpenseReceiptsAsync(request.ExpenseId, ct);
    if (serviceResult.Success)
    {
      var response = new ListExpenseReceiptsResponse
      {
        Items = serviceResult.Data,
        TotalCount = serviceResult.Data!.Count
      };
      await Send.OkAsync(response, ct);
      return;
    }
    await Send.ErrorsAsync(statusCode: 400, cancellation: ct);
  }
}
