using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class UserConfiguration : IBaseConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        #region Types

        builder.HasKey(u => u.Id);

        builder.Property(u => u.MobileNumber)
            .IsRequired()
            .HasColumnType("nvarchar(11)")
            .HasMaxLength(11);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasColumnType("nvarchar(100)")
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired()
            .HasColumnType("nvarchar(256)")
            .HasMaxLength(256);

        builder.Property(u => u.FirstName)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(u => u.LastName)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(u => u.Points)
            .HasColumnType("tinyint");

        builder.Property(u => u.LastLoginDate)
            .HasColumnType("datetime2(6)");

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

        #region Relations

        builder.HasMany(u => u.OrgAsOwner)
            .WithOne(o => o.Owner)
            .HasForeignKey(o => o.OwnerId);

        builder.HasMany(u => u.OrgAsMember)
            .WithOne(om => om.User)
            .HasForeignKey(om => om.UserId);

        builder.HasMany(u => u.ProjAsCreator)
            .WithOne(p => p.Creator)
            .HasForeignKey(p => p.CreatorId);

        builder.HasMany(u => u.ProjAsMember)
            .WithOne(pm => pm.User)
            .HasForeignKey(pm => pm.UserId);

        builder.HasMany(u => u.TaskAsCreator)
            .WithOne(t => t.Creator)
            .HasForeignKey(t => t.CreatorId);

        builder.HasMany(u => u.MyTasks)
            .WithOne(ta => ta.User)
            .HasForeignKey(ta => ta.UserId);

        builder.HasMany(u => u.MyTaskInfo)
            .WithOne(ti => ti.User)
            .HasForeignKey(ti => ti.UserId);

        builder.HasMany(u => u.MyTokens)
           .WithOne(ut => ut.User)
           .HasForeignKey(ut => ut.UserId);

        #endregion

        builder.HasQueryFilter(u => !u.IsDelete);

        builder.HasIndex(u => u.MobileNumber)
            .IsUnique();

        builder.HasIndex(u => u.Email)
            .IsUnique();
    }
}
