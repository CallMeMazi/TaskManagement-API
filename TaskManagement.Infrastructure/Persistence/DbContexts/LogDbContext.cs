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

        var DomainAssembly = typeof(LogBaseEntity).Assembly;
        modelBuilder.Entity<AuditLog>();

        new AuditLogConfiguration().Configure(modelBuilder.Entity<AuditLog>());
        
        modelBuilder.AddRestrictDeleteBehaviorConvention();
        modelBuilder.AddPluralizingTableNameConvention();
    }
}
