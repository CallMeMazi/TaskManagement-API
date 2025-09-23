using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class TaskConfiguration : IEntityTypeConfiguration<Domin.Entities.BaseEntities.Task>
{
    public void Configure(EntityTypeBuilder<Domin.Entities.BaseEntities.Task> builder)
    {
        #region Types

        builder.HasKey(t => t.Id);

        builder.Property(t => t.TaskName)
            .IsRequired()
            .HasColumnType("nvarchar(50)")
            .HasMaxLength(50);

        builder.Property(t => t.TaskDescription)
            .IsRequired()
            .HasColumnType("nvarchar(150)")
            .HasMaxLength(150);

        builder.Property(t => t.IsActive)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(true);

        builder.Property(t => t.TaskType)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(t => t.TaskStatus)
            .IsRequired()
            .HasColumnType("tinyint");

        builder.Property(t => t.TaskDeadline)
            .IsRequired()
            .HasColumnType("datetime2(6)");

        builder.Property(t => t.TaskProgress)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(0);

        //base property
        builder.Property(t => t.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(t => t.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(t => t.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        #region Relations

        builder.HasMany(t => t.Members)
            .WithOne(ta => ta.Task)
            .HasForeignKey(ta => ta.TaskId);

        builder.HasMany(t => t.Info)
            .WithOne(ti => ti.Task)
            .HasForeignKey(ti => ti.TaskId);

        #endregion

        builder.HasQueryFilter(t => !t.IsDelete);
    }
}
