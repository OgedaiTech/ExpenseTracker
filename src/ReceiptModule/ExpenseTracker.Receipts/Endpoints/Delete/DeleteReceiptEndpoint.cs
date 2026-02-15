using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal partial class DeleteReceiptEndpoint(
  IDeleteReceiptService deleteReceiptService,
  ILogger<DeleteReceiptEndpoint> logger) : EndpointWithoutRequest
{
  public override void Configure()
  {
    Delete("/receipts/{receiptId:guid}");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var receiptId = Route<Guid>("receiptId");
    var serviceResult = await deleteReceiptService.DeleteReceiptAsync(receiptId, ct);
    if (!serviceResult.Success)
    {
      var result = serviceResult.Message switch
      {
        DeleteReceiptConstants.RECEIPT_NOT_FOUND
          => Results.Problem(serviceResult.Message, statusCode: StatusCodes.Status404NotFound),
        DeleteReceiptConstants.DELETE_FAILED
          => Results.Problem(serviceResult.Message, statusCode: StatusCodes.Status500InternalServerError),
        _ => Results.Problem("Invalid Request", statusCode: StatusCodes.Status400BadRequest)
      };

      LogDeleteReceiptFailed(logger, serviceResult.Message ?? "Unknown error", receiptId, null);
      await Send.ResultAsync(result);
      return;
    }

    LogDeleteReceiptSuccess(logger, receiptId, null);
    await Send.NoContentAsync(cancellation: ct);
  }
}
