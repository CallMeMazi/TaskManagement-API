using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;
using TaskManagement.Infrastructure.QueryCache;

namespace TaskManagement.Infrastructure.Repositories;
public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<TEntity> Entities;


    public BaseRepository(ApplicationDbContext dbContext)
    {
        _db = dbContext;
        Entities = _db.Set<TEntity>();
    }


    #region Async Methods

    // Query methods
    public Task<List<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(ct);
    }
    public Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Where(filter).ToListAsync(ct);
    }
    public async Task<TEntity?> GetByIdAsync(int entityId, bool isTracking = false, CancellationToken ct = default)
    {
        //var query = isTracking ? Entities : Entities.AsNoTracking();
        //return query.FirstOrDefaultAsync(o => o.Id == entityId, ct);

        // With complied query
        return isTracking ? await BaseCompliedQuery<TEntity>.GetByIdQuery(_db, entityId)
            : await BaseCompliedQuery<TEntity>.GetByIdAsNoTrackingQueryg(_db, entityId);
    }
    public Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, ct);
    }
    public ValueTask<TEntity?> FindByIdsAsync(CancellationToken ct, params object[] ids)
    {
        return Entities.FindAsync(ids, ct);
    }

    // Command methods
    public async System.Threading.Tasks.Task AddAsync(TEntity entity, CancellationToken ct = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddAsync)} method, Type = {typeof(TEntity)}!");

        await Entities.AddAsync(entity, ct).ConfigureAwait(false);
    }
    public async System.Threading.Tasks.Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeAsync)} method, Type = {typeof(TEntity)}!");

        await Entities.AddRangeAsync(entities, ct).ConfigureAwait(false);
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

    // Costum methods
    public Task<bool> IsEntityExistByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct)
    {
        return Entities.AnyAsync(filter, ct);
    }
    public Task<int> GetCountByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct)
    {
        return Entities.CountAsync(filter, ct);
    }

    #endregion


    #region Attach & Detach

    public virtual void Detach(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Detach)} method!");

        var entry = _db.Entry(entity);
        if (entry != null)
            entry.State = EntityState.Detached;
    }
    public virtual void Attach(TEntity entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(Detach)} method!");

        var entry = _db.Entry(entity);
        if (entry.State == EntityState.Detached)
            Entities.Attach(entity);
    }

    #endregion


    #region Explicit Loading

    public virtual async System.Threading.Tasks.Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken ct)
        where TProperty : class
    {
        Attach(entity);

        var collection = _db.Entry(entity).Collection(collectionProperty);
        if (!collection.IsLoaded)
            await collection.LoadAsync(ct).ConfigureAwait(false);
    }

    public virtual async System.Threading.Tasks.Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> referenceProperty, CancellationToken ct)
        where TProperty : class
    {
        Attach(entity);

        var reference = _db.Entry(entity).Reference(referenceProperty);
        if (!reference.IsLoaded)
            await reference.LoadAsync(ct).ConfigureAwait(false);
    }

    #endregion
}
