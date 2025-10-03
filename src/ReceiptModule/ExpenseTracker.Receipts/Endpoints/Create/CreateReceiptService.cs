using ExpenseTracker.Core;

namespace ExpenseTracker.Receipts.Endpoints.Create;

public class CreateReceiptService(ICreateReceiptRepository createReceiptRepository) : ICreateReceiptService
{
  public async Task<ServiceResult> CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct)
  {
    await createReceiptRepository.CreateReceiptAsync(createReceiptRequest, ct);
    return new ServiceResult();
  }
}
