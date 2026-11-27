using CgbSteps.Domain.AuditLog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CgbSteps.Infrastructure.Persistence.Configurations;

internal sealed class AuditLogEntryConfiguration : IEntityTypeConfiguration<AuditLogEntry>
{
    public void Configure(EntityTypeBuilder<AuditLogEntry> builder)
    {
        builder.ToTable("audit_log");
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Action).HasMaxLength(AuditLogEntry.MaxCodeLength).IsRequired();
        builder.Property(e => e.TargetType).HasMaxLength(AuditLogEntry.MaxCodeLength).IsRequired();
        builder.Property(e => e.TargetId).HasMaxLength(AuditLogEntry.MaxCodeLength);

        builder.HasIndex(e => e.OccurredAt);
        builder.HasIndex(e => new { e.TargetType, e.TargetId });
    }
}
