using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class BaseRepository<TEntity, TDto> : IBaseRepository<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : class
{
    protected readonly ApplicationDbContext DbContext;
    protected readonly IMapper _mapper;
    protected readonly DbSet<TEntity> Entities;
    public virtual IQueryable<TEntity> Table => Entities.AsQueryable();
    public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking().AsQueryable();


    public BaseRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        DbContext = dbContext;
        _mapper = mapper;
        Entities = DbContext.Set<TEntity>();
    }


    #region Async Methods

    // Query methods
    public Task<List<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }
    public Task<List<TDto>> GetAllDtoAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
    public Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Where(filter).ToListAsync(cancellationToken);
    }
    public Task<List<TDto>> GetAllDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
    public Task<TEntity?> GetByIdAsync(int entityId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(o => o.Id == entityId, cancellationToken);
    }
    public Task<TDto?> GetDtoByIdAsync(int entityId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == entityId)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }
    public Task<TDto?> GetDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<TDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
    public ValueTask<TEntity?> FindByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    // Command methods
    public async System.Threading.Tasks.Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddAsync)} method, Type = {typeof(TEntity)}!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }
    public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeAsync)} method, Type = {typeof(TEntity)}!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }
    public void Update(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Update)} method, Type = {typeof(TEntity)}!");

        Entities.Update(entity);
    }
    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRange)} method, Type = {typeof(TEntity)}!");

        Entities.UpdateRange(entities);
    }
    public void Delete(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Delete)} method, Type =  {typeof(TEntity)} !");

        Entities.Remove(entity);
    }
    public void DeleteRange(IEnumerable<TEntity> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRange)} method, Type =  {typeof(TEntity)} !");

        Entities.RemoveRange(entities);
    }

    #endregion


    #region Attach & Detach

    public virtual void Detach(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Detach)} method!");

        var entry = DbContext.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }
    public virtual void Attach(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Detach)} method!");

        var entry = DbContext.Entry(entity);
        if (entry.State == EntityState.Detached)
            Entities.Attach(entity);
    }

    #endregion


    #region Explicit Loading

    public virtual async System.Threading.Tasks.Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);

        var collection = DbContext.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    public virtual async System.Threading.Tasks.Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class
    {
        Attach(entity);

        var reference = DbContext.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
    }

    #endregion
}
