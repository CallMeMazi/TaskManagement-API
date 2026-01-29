using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.QueryCache;
public static class BaseCompliedQuery<TEntity>
    where TEntity : BaseEntity
{
    // Get by ID
    public static readonly Func<ApplicationDbContext, int, Task<TEntity?>> GetByIdQuery =
    EF.CompileAsyncQuery((ApplicationDbContext context, int entityId) =>
        context.Set<TEntity>().FirstOrDefault(e => e.Id == entityId)
    );
    public static readonly Func<ApplicationDbContext, int, Task<TEntity?>> GetByIdAsNoTrackingQueryg =
    EF.CompileAsyncQuery((ApplicationDbContext context, int entityId) =>
        context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id == entityId)
    );

}
