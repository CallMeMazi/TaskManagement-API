using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.LogEntities;
using TaskManagement.Infrastructure.Persistence.Configurations.LogEntityConfiguration;
using TaskManagement.Infrastructure.Utilities;

namespace TaskManagement.Infrastructure.Persistence.DbContexts;

public class LogDbContext(DbContextOptions<LogDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OperationalLog>();
        modelBuilder.Entity<AuditLog>();

        new AuditLogConfiguration().Configure(modelBuilder.Entity<AuditLog>());
        new OperationalLogConfiguration().Configure(modelBuilder.Entity<OperationalLog>());
        
        modelBuilder.AddRestrictDeleteBehaviorConvention();
    }
}
