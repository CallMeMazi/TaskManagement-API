using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class TaskInfoConfiguration : IBaseConfiguration<TaskInfo>
{
    public void Configure(EntityTypeBuilder<TaskInfo> builder)
    {
        #region Types

        builder.HasKey(ti => ti.Id);

        builder.Property(ti => ti.TaskInfoDescription)
            .HasColumnType("nvarchar(200)")
            .HasMaxLength(200);

        builder.Property(ti => ti.StartedTaskAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(ti => ti.EndedTaskAt)
            .HasColumnType("datetime2(6)");

        builder.Property(ti => ti.TotalHours)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(0);

        //base property
        builder.Property(ti => ti.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(ti => ti.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(ti => ti.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        builder.HasQueryFilter(ti => !ti.IsDelete);

        builder.HasIndex(ti => ti.TaskId);

        builder.HasIndex(ti => ti.UserId);

        builder.HasIndex(ti => ti.TaskAssignmentId);
    }
}
