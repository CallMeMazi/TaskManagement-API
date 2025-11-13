using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class UserTokenConfiguration : IBaseConfiguration<UserToken>
{
    public void Configure(EntityTypeBuilder<UserToken> builder)
    {
        #region Types

        builder.HasKey(ut => ut.Id);

        builder.Property(ut => ut.AccessTokenHash)
            .IsRequired()
            .HasColumnType("nvarchar(1000)")
            .HasMaxLength(1000);

        builder.Property(ut => ut.RefreshTokenHash)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(ut => ut.SecurityStamp)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(ut => ut.TokenStatus)
            .IsRequired()
            .HasColumnType("tinyint");

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

        builder.Property(ut => ut.UserAgent)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        //base property
        builder.Property(u => u.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(u => u.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(u => u.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        builder.HasQueryFilter(u => !u.IsDelete);

        builder.HasIndex(ut => ut.UserId);

        builder.HasIndex(ut => ut.AccessTokenHash)
            .IsUnique();

        builder.HasIndex(ut => ut.RefreshTokenHash)
            .IsUnique();

        builder.HasIndex(ut => ut.TokenStatus);
    }
}
