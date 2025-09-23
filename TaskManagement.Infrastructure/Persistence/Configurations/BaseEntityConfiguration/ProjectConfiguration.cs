using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        #region Types

        builder.HasKey(p => p.Id);

        builder.Property(p => p.ProjName)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(p => p.ProjDescription)
            .IsRequired()
            .HasColumnType("nvarchar(200)")
            .HasMaxLength(200);

        builder.Property(p => p.ProjProgress)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(0);

        builder.Property(p => p.IsActive)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(p => p.ProjStatus)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(p => p.ProjStartAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(p => p.ProjEndAt)
            .HasColumnType("datetime2(6)");

        builder.Property(p => p.ProjMaxUsers)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(3);

        builder.Property(p => p.ProjMaxTasks)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(3);

        //base property
        builder.Property(p => p.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(p => p.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        #region Relations

        builder.HasMany(p => p.ProjMember)
            .WithOne(pm => pm.Project)
            .HasForeignKey(pm => pm.ProjId);

        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjId);

        #endregion

        builder.HasQueryFilter(p => !p.IsDelete);
    }
}
