using System.Linq.Expressions;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IBaseRepository<TEntity>
    where TEntity : BaseEntity
{
    #region Async methods

    // Query methods
    Task<List<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken ct = default);
    Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int entityId, bool isTracking = false, CancellationToken ct = default);
    Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default);
    ValueTask<TEntity?> FindByIdsAsync(CancellationToken ct, params object[] ids);

    // Command methods
    System.Threading.Tasks.Task AddAsync(TEntity entity, CancellationToken ct = default);
    System.Threading.Tasks.Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken ct = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);

    // Costum methods
    Task<bool> IsEntityExistByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct);
    Task<int> GetCountByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct);

    #endregion


    #region Attach & Detach

    void Attach(TEntity entity);
    void Detach(TEntity entity);

    #endregion


    #region Explicit Loading

    System.Threading.Tasks.Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken ct)
        where TProperty : class;
    System.Threading.Tasks.Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> referenceProperty, CancellationToken ct)
        where TProperty : class;

    #endregion
}
