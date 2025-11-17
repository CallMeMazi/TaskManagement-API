using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IBaseRepository<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : class
{
    #region Async methods

    // Query methods
    Task<List<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken ct = default);
    Task<List<TDto>> GetAllDtoAsync(CancellationToken ct = default);
    Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default);
    Task<List<TDto>> GetAllDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default);
    Task<TEntity?> GetByIdAsync(int entityId, bool isTracking = false, CancellationToken ct = default);
    Task<TDto?> GetDtoByIdAsync(int entityId, CancellationToken ct = default);
    Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken ct = default);
    Task<TDto?> GetDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken ct = default);
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
