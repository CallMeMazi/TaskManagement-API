using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domain.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;

public class OperationalLogConfiguration : IEntityTypeConfiguration<OperationalLog>
{
    public void Configure(EntityTypeBuilder<OperationalLog> builder)
    {
        builder.ToTable("OperationalLogs");
        builder.HasKey(log => log.Id);

        builder.Property(log => log.Id)
            .ValueGeneratedOnAdd();

        builder.Property(log => log.Message)
            .HasColumnType("nvarchar(max)");

        builder.Property(log => log.MessageTemplate)
            .HasColumnType("nvarchar(max)");

        builder.Property(log => log.Level)
            .HasMaxLength(128)
            .HasColumnType("nvarchar(128)");

        builder.Property(log => log.TimeStamp)
            .HasColumnType("datetime2");

        builder.Property(log => log.Exception)
            .HasColumnType("nvarchar(max)");

        builder.Property(log => log.Properties)
            .HasColumnType("nvarchar(max)");

        builder.Property(log => log.TraceId)
            .HasMaxLength(64)
            .HasColumnType("nvarchar(64)");

        builder.Property(log => log.SourceContext)
            .HasMaxLength(256)
            .HasColumnType("nvarchar(256)");
    }
}
