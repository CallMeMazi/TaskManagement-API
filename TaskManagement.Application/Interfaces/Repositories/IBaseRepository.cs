using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IBaseRepository<TEntity, TDto>
    where TEntity : BaseEntity
    where TDto : class
{
    IQueryable<TEntity> Table { get; }
    IQueryable<TEntity> TableNoTracking { get; }


    #region Async methods

    // Query methods
    Task<List<TEntity>> GetAllAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<List<TDto>> GetAllDtoAsync(CancellationToken cancellationToken = default);
    Task<List<TEntity>> GetAllByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<List<TDto>> GetAllDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByIdAsync(int entityId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TDto?> GetDtoByIdAsync(int entityId, CancellationToken cancellationToken = default);
    Task<TEntity?> GetByFilterAsync(Expression<Func<TEntity, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<TDto?> GetDtoByFilterAsync(Expression<Func<TEntity, bool>> filter, CancellationToken cancellationToken = default);
    ValueTask<TEntity?> FindByIdsAsync(CancellationToken cancellationToken, params object[] ids);

    // Command methods
    System.Threading.Tasks.Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    void Delete(TEntity entity);
    void DeleteRange(IEnumerable<TEntity> entities);

    #endregion


    #region Attach & Detach

    void Attach(TEntity entity);
    void Detach(TEntity entity);

    #endregion


    #region Explicit Loading

    System.Threading.Tasks.Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken) 
        where TProperty : class;
    System.Threading.Tasks.Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class;

    #endregion
}
