using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Receipts.Data;

public class ReceiptConfiguration : IEntityTypeConfiguration<Receipt>
{
  public void Configure(EntityTypeBuilder<Receipt> builder)
  {
    builder
    .ToTable("Receipts")
    .HasKey(r => r.Id);
    builder.Property(r => r.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Property(r => r.ExpenseId).IsRequired();
    builder.Property(r => r.ReceiptNo).IsRequired().HasMaxLength(64);
    builder.Property(r => r.Date).IsRequired();
    builder.Property(r => r.Amount).IsRequired().HasPrecision(18, 2);
    builder.Property(r => r.Vendor).IsRequired().HasMaxLength(128);
  }
}
