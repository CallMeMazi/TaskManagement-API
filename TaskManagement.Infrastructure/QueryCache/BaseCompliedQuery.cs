using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.QueryCache; 
public static class BaseCompliedQuery<TEntity, TDto>
    where TDto : class
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
    public static readonly Func<ApplicationDbContext, int, IConfigurationProvider, Task<TDto?>> GetDtoByIdQueryg =
    EF.CompileAsyncQuery((ApplicationDbContext context, int entityId, IConfigurationProvider mapperConfig) =>
        context.Set<TEntity>().AsNoTracking().Where(e => e.Id == entityId).ProjectTo<TDto>(mapperConfig).FirstOrDefault()
    );

}
