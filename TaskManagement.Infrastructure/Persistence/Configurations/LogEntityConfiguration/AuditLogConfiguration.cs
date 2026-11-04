using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");
        builder.HasKey(audit => audit.Id);

        // long identity maps to bigint on SQL Server. An explicit bigint store type makes SQLite emit
        // AUTOINCREMENT on a non-INTEGER key, which SQLite rejects.
        builder.Property(audit => audit.Id)
            .ValueGeneratedOnAdd();

        builder.Property(audit => audit.OccurredAtUtc)
            .IsRequired()
            .HasColumnType("datetime2(6)");

        builder.Property(audit => audit.ActorUserId)
            .HasColumnType("bigint");

        builder.Property(audit => audit.Action)
            .IsRequired()
            .HasMaxLength(AuditLog.ActionMaxLength)
            .HasColumnType("nvarchar(100)");

        builder.Property(audit => audit.EntityType)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(audit => audit.EntityId)
            .IsRequired()
            .HasColumnType("bigint");

        builder.Property(audit => audit.RelatedEntityType)
            .HasColumnType("tinyint");

        builder.Property(audit => audit.RelatedEntityId)
            .HasColumnType("bigint");

        builder.Property(audit => audit.Outcome)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(audit => audit.TraceId)
            .HasMaxLength(AuditLog.TraceIdMaxLength)
            .HasColumnType("nvarchar(64)");

        builder.Property(audit => audit.Metadata)
            .HasMaxLength(AuditLog.MetadataMaxLength)
            .HasColumnType("nvarchar(2000)");

        builder.HasIndex(audit => new { audit.EntityType, audit.EntityId, audit.OccurredAtUtc });
        builder.HasIndex(audit => new { audit.ActorUserId, audit.OccurredAtUtc });
        builder.HasIndex(audit => audit.TraceId);
        builder.HasIndex(audit => audit.OccurredAtUtc);
    }
}
