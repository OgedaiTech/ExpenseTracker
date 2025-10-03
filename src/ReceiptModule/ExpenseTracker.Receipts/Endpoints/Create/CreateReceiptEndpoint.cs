using ExpenseTracker.Receipts.UseCases;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Receipts.Endpoints.Create;

internal class CreateReceiptEndpoint(
  ICreateReceiptService createReceiptService,
  IMediator mediator) : Endpoint<CreateReceiptRequest>
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
    var command = new AddAmountToExpenseCommand(request.ExpenseId, request.Amount);
    var increseAmountResult = await mediator.Send(command, ct);
    if (!increseAmountResult.Success)
    {
      var problem = Results.Problem(
        title: "Could not add amount to expense",
        detail: increseAmountResult.Message,
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      return;
    }
    await Send.CreatedAtAsync("receipts", cancellation: ct);
  }
}
