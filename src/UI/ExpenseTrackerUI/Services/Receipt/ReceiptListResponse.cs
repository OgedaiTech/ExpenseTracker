namespace ExpenseTrackerUI.Services.Receipt;

public class ReceiptListResponse
{
  public List<ReceiptDto>? Items { get; set; }
  public int TotalCount { get; set; }
}
