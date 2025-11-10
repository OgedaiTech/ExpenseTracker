using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Expenses.Data;

internal class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
  [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S2325:Make 'Configure' a static method", Justification = "Cannot be static due to interface")]
  public void Configure(EntityTypeBuilder<Expense> builder)
  {
    builder
    .ToTable("Expenses")
    .HasKey(e => e.Id);
    builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd();
    builder.Property(e => e.Name).IsRequired().HasMaxLength(128);
    builder.Property(e => e.CreatedByUserId).IsRequired();
    builder.Property(e => e.UpdatedAt).IsRequired(false);
    builder.Property(e => e.DeletedAt).IsRequired(false);
  }
}
