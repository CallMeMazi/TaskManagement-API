using System.Linq.Expressions;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    // Query methods
    Task<UserToken?> GetUserTokenByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
