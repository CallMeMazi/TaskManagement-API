using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.Configurations.BaseEntityConfiguration;
using TaskManagement.Infrastructure.Utilities;

namespace TaskManagement.Infrastructure.Persistence.DbContexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var dominAssembly = typeof(BaseEntity).Assembly;
        var infrastructureAssembly = typeof(ApplicationDbContext).Assembly;

        modelBuilder.RegisterAllEntities<IBaseEntity>(dominAssembly);
        modelBuilder.RegisterEntityTypeConfiguration(typeof(IBaseConfiguration<>), infrastructureAssembly);
        modelBuilder.AddRestrictDeleteBehaviorConvention();
        modelBuilder.AddSequentialGuidForIdConvention();
        modelBuilder.AddPluralizingTableNameConvention();
    }

    public override int SaveChanges()
    {
        _cleanString();
        return base.SaveChanges();
    }
    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        _cleanString();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }
    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        _cleanString();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        _cleanString();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void _cleanString()
    {
        var changedEntities = ChangeTracker.Entries()
            .Where(x => x.State == EntityState.Added || x.State == EntityState.Modified);

        foreach (var item in changedEntities)
        {
            if (item.Entity.IsNullParameter())
                continue;

            var strProps = item.Entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead && p.CanWrite && p.PropertyType == typeof(string));

            foreach (var property in strProps)
            {
                var val = property.GetValue(item.Entity, null) as string;

                if (val.IsNullParameter())
                    continue;

                var newVal = val.FixPersianCharsFull();
                if (newVal == val)
                    continue;

                property.SetValue(item.Entity, newVal);
            }
        }
    }
}
