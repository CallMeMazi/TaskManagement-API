using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
{
    public void Configure(EntityTypeBuilder<Organization> builder)
    {
        #region Types

        builder.HasKey(o => o.Id);

        builder.Property(o => o.OrgName)
            .IsRequired()
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100);

        builder.Property(o => o.OrgCode)
            .IsRequired()
            .HasColumnType("nvarchar(120)")
            .HasMaxLength(120);

        builder.Property(o => o.OrgDescription)
            .IsRequired()
            .HasColumnType("nvarchar(400)")
            .HasMaxLength(400);

        builder.Property(o => o.IsActive)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.Property(o => o.MaxUsers)
            .HasColumnType("tinyint")
            .HasDefaultValue(5);

        //base property
        builder.Property(o => o.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(o => o.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(o => o.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        #region Relations

        builder.HasMany(o => o.Members)
            .WithOne(om => om.Org)
            .HasForeignKey(om => om.OrgId);

        builder.HasMany(o => o.Projects)
            .WithOne(p => p.Org)
            .HasForeignKey(p => p.OrgId);

        #endregion

        builder.HasQueryFilter(o => !o.IsDelete);

        builder.HasIndex(o => o.OrgCode)
            .IsUnique();
    }
}
