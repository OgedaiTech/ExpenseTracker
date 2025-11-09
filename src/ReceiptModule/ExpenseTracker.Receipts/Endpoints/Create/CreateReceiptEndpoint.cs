using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Receipts.Endpoints.Create;

internal class CreateReceiptEndpoint(
  ICreateReceiptService createReceiptService) : Endpoint<CreateReceiptRequest>
{
  public override void Configure()
  {
    Post("/receipts");
    Claims("EmailAddress");
  }

  public override async Task HandleAsync(CreateReceiptRequest request, CancellationToken ct)
  {
    var serviceResult = await createReceiptService.CreateReceiptAsync(request, ct);
    if (!serviceResult.Success)
    {
      var result = serviceResult.Message switch
      {
        ServiceConstants.CANT_ADD_AMOUNT_TO_EXPENSE
          => Results.Problem(serviceResult.Message, statusCode: StatusCodes.Status400BadRequest),
        _ => Results.Problem("Invalid Request", statusCode: StatusCodes.Status400BadRequest)
      };

      await Send.ResultAsync(result);
      return;
    }

    await Send.CreatedAtAsync("receipts", cancellation: ct);
  }
}
