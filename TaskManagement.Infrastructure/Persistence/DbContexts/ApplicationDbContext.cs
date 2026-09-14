using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
using TaskManagement.Infrastructure.Utilities;

namespace TaskManagement.Infrastructure.Persistence.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var DomainAssembly = typeof(BaseEntity).Assembly;
        var infrastructureAssembly = typeof(ApplicationDbContext).Assembly;

        modelBuilder.RegisterAllEntities<IBaseEntity>(DomainAssembly);
        modelBuilder.RegisterEntityTypeConfiguration(typeof(IBaseConfiguration<>), infrastructureAssembly);
        modelBuilder.AddSnowflakeIdConvention();
        modelBuilder.AddRestrictDeleteBehaviorConvention();
        modelBuilder.AddPluralizingTableNameConvention();
    }
}
