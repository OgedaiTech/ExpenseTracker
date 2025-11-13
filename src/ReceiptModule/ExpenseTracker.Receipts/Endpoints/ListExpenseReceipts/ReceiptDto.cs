namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public record ReceiptDto
{
  public required Guid Id { get; init; }
  public string ReceiptNo { get; set; } = string.Empty;
  public DateTime Date { get; set; }
  public decimal Amount { get; set; }
  public string Vendor { get; set; } = string.Empty;
  public DateTime CreatedAt { get; set; }
}
