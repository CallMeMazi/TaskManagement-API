using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.QueryCache;
public static class BaseCompliedQuery<TEntity>
    where TEntity : BaseEntity
{
    // Get by ID
    public static readonly Func<ApplicationDbContext, long, Task<TEntity?>> GetByIdQuery =
    EF.CompileAsyncQuery((ApplicationDbContext context, long entityId) =>
        context.Set<TEntity>().FirstOrDefault(e => e.Id == entityId)
    );
    public static readonly Func<ApplicationDbContext, long, Task<TEntity?>> GetByIdAsNoTrackingQueryg =
    EF.CompileAsyncQuery((ApplicationDbContext context, long entityId) =>
        context.Set<TEntity>().AsNoTracking().FirstOrDefault(e => e.Id == entityId)
    );

}
