using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Accounting.Data;

public class TenantAccountingSettingsConfiguration : IEntityTypeConfiguration<TenantAccountingSettings>
{
  public void Configure(EntityTypeBuilder<TenantAccountingSettings> builder)
  {
    builder
      .ToTable("TenantAccountingSettings")
      .HasKey(t => t.TenantId);

    builder.Property(t => t.TenantId).IsRequired();
    builder.Property(t => t.Provider).IsRequired().HasMaxLength(64);
    builder.Property(t => t.CredentialsJson).HasColumnType("text");
  }
}
