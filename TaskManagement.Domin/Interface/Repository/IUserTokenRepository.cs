using System.Linq.Expressions;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    // Query methods
    Task<UserToken?> GetUserTokenByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
