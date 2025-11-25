using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class ProjectMemberShipConfiguration : IBaseConfiguration<ProjectMemberShip>
{
    public void Configure(EntityTypeBuilder<ProjectMemberShip> builder)
    {
        #region Types

        builder.HasKey(pm => pm.Id);

        builder.Property(pm => pm.Role)
            .IsRequired()
            .HasColumnType("tinyint");

        //base property
        builder.Property(pm => pm.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(pm => pm.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(pm => pm.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        builder.HasQueryFilter(pm => !pm.IsDelete);

        builder.HasIndex(pm => pm.ProjId);

        builder.HasIndex(pm => pm.UserId);

    }
}
