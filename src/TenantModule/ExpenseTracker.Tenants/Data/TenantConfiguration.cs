using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.Tenants.Data;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
  public void Configure(EntityTypeBuilder<Tenant> builder)
  {
    builder
      .ToTable("Tenants")
      .HasKey(x => x.Id);

    builder.Property(x => x.Name)
        .IsRequired()
        .HasMaxLength(200);

    builder.Property(x => x.Code)
        .HasMaxLength(50);

    builder.HasIndex(x => x.Code)
        .IsUnique();

    builder.Property(x => x.Domain)
        .HasMaxLength(200);

    builder.Property(x => x.IsActive)
        .IsRequired();
  }
}
