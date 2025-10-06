using System.Linq.Expressions;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IBaseRepository<TEntity>
{
    void Attach(TEntity entity);
    void Detach(TEntity entity);
    Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken) 
        where TProperty : class;
    Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty?>> referenceProperty, CancellationToken cancellationToken)
        where TProperty : class;
}
