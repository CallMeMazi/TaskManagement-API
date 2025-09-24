using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
public class TaskAssignmentConfiguration : IBaseConfiguration<TaskAssignment>
{
    public void Configure(EntityTypeBuilder<TaskAssignment> builder)
    {
        #region Types

        builder.HasKey(ta => ta.Id);

        builder.Property(ta => ta.TotalTimeSpent)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(0);

        builder.Property(ta => ta.StartTaskCount)
            .IsRequired()
            .HasColumnType("tinyint")
            .HasDefaultValue(0);

        builder.Property(ta => ta.IsInProgress)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(ta => ta.LastStartedAt)
            .HasColumnType("datetime2(6)");

        //base property
        builder.Property(ta => ta.IsDelete)
            .IsRequired()
            .HasColumnType("bit")
            .HasDefaultValue(false);

        builder.Property(ta => ta.CreatedAt)
            .IsRequired()
            .HasColumnType("datetime2(6)")
            .HasDefaultValueSql("getdate()");

        builder.Property(ta => ta.UpdatedAt)
            .HasColumnType("datetime2(6)");

        #endregion

        #region Relations

        builder.HasMany(ta => ta.Info)
            .WithOne(ti => ti.TaskAssignment)
            .HasForeignKey(ti => ti.TaskAssignmentId);

        #endregion

        builder.HasQueryFilter(ta => !ta.IsDelete);
    }
}
