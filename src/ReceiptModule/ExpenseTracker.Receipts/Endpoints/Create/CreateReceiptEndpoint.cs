using FastEndpoints;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Receipts.Endpoints.Create;

internal class CreateReceiptEndpoint(ICreateReceiptService createReceiptService) : Endpoint<CreateReceiptRequest>
{
  override public void Configure()
  {
    Post("/receipts");
    AllowAnonymous();
  }

  public override async Task HandleAsync(CreateReceiptRequest request, CancellationToken ct)
  {
    var serviceResult = await createReceiptService.CreateReceiptAsync(request, ct);
    if (!serviceResult.Success)
    {
      var problem = Results.Problem(
        title: "Invalid request",
        detail: serviceResult.Message,
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }
    await Send.CreatedAtAsync("receipts", cancellation: ct);
  }
}
