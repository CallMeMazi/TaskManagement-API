using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class OrganizationMemberShipConfiguration : IBaseConfiguration<OrganizationMemberShip>
{
    public void Configure(EntityTypeBuilder<OrganizationMemberShip> builder)
    {
        #region Types

        builder.HasKey(om => om.Id);

        builder.Property(om => om.Role)
            .IsRequired()
            .HasColumnType("tinyint");

        //base property
        builder.Property(om => om.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(om => om.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(om => om.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        builder.HasQueryFilter(om => !om.IsDelete);

        builder.HasIndex(ut => ut.UserId);

        builder.HasIndex(ut => ut.OrgId);
    }
}
