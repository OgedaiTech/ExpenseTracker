namespace ExpenseTrackerUI.Services.Receipt;

public record ReceiptDto(
  Guid Id,
  string ReceiptNo,
  DateTime Date,
  decimal Amount,
  string Vendor,
  DateTime CreatedAt
);
