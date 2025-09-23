using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class UserTokenConfiguration : IEntityTypeConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        #region Types

        builder.HasKey(ut => ut.Id);

        builder.Property(ut => ut.Token)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(ut => ut.SecurityStamp)
            .IsRequired()
            .HasColumnType("uniqueidentifier");

        builder.Property(ut => ut.TokenStatus)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(ut => ut.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(ut => ut.ExpiredAt)
            .IsRequired()
            .HasColumnType("datetime2(6)");

        builder.Property(ut => ut.RevokedAt)
            .HasColumnType("datetime2(6)");

        builder.Property(ut => ut.LastUsedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(ut => ut.UserIp)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        #endregion

        builder.HasIndex(ut => ut.Token)
            .IsUnique();
    }
}
