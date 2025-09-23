using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;
public class EntityLogConfiguration : IEntityTypeConfiguration<EntityLog>
{
    public void Configure(EntityTypeBuilder<EntityLog> builder)
    {
        #region Types

        builder.HasKey(el => el.Id);

        builder.Property(el => el.EntityType)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(el => el.Action)
            .IsRequired()
            .HasColumnType("tinyint");

        //base property
        builder.Property(el => el.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(el => el.LogDescription)
            .IsRequired()
            .HasColumnType("nvarchar(200)")
            .HasMaxLength(200);

        #endregion

        builder.HasIndex(el => el.EntityId);
    }
}
