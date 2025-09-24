using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.LogEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;
public class EntityRelationLogConfiguration : ILogConfigyration<EntityRelationLog>
{
    public void Configure(EntityTypeBuilder<EntityRelationLog> builder)
    {
        #region Types

        builder.HasKey(erl => erl.Id);

        builder.Property(erl => erl.PrimaryEntityType)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(erl => erl.SecondaryEntityType)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(erl => erl.Action)
            .IsRequired()
            .HasColumnType("tinyint");

        //base property
        builder.Property(erl => erl.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(erl => erl.LogDescription)
            .IsRequired()
            .HasColumnType("nvarchar(200)")
            .HasMaxLength(200);

        #endregion

        builder.HasIndex(erl => new
        {
            erl.PrimaryEntityId,
            erl.SecondaryEntityId,
            erl.PrimaryEntityType,
            erl.SecondaryEntityType
        });
    }
}
