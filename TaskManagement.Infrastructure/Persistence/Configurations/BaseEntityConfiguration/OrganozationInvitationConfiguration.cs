using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class OrganozationInvitationConfiguration : IBaseConfiguration<OrganizationInvitation>
{
    public void Configure(EntityTypeBuilder<OrganizationInvitation> builder)
    {
        #region Types

        builder.HasKey(oi => oi.Id);

        builder.Property(oi => oi.Token)
            .IsRequired()
            .HasColumnType("nvarchar(16)")
            .HasMaxLength(16);

        builder.Property(oi => oi.Status)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(oi => oi.ExpiredAt)
           .IsRequired()
           .HasColumnType("datetime2(6)");

        builder.Property(oi => oi.RevokedAt)
           .HasColumnType("datetime2(6)");

        //base property
        builder.Property(oi => oi.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(oi => oi.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(oi => oi.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        builder.HasQueryFilter(oi => !oi.IsDelete);

        builder.Property(oi => oi.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

        builder.HasIndex(oi => oi.OrgId);

        builder.HasIndex(oi => oi.UserId);

        builder.HasIndex(oi => oi.Status);

        builder.HasIndex(oi => oi.Token)
            .IsUnique();
    }
}
