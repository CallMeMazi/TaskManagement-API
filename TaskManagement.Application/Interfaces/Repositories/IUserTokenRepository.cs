using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IUserTokenRepository : IBaseRepository<UserToken>
{
    System.Threading.Tasks.Task AddRangeUserTokensAsync(IEnumerable<UserToken> entities, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddUserTokenAsync(UserToken entity, CancellationToken cancellationToken = default);
    void DeleteRangeUserTokens(IEnumerable<UserToken> entities);
    void DeleteUserToken(UserToken entity);
    ValueTask<UserToken?> FindUserTokensByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<UserToken>> GetAllUserTokensAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<UserToken?> GetUserTokenByFilterAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<UserToken?> GetUserTokenByIdAsync(int userTokenId, bool isTracking = false, CancellationToken cancellationToken = default);
    void UpdateRangeUserTokens(IEnumerable<UserToken> entities);
    void UpdateUserToken(UserToken entity);
    Task<int> GetCountByFilterAsync(Expression<Func<UserToken, bool>> filter);
    Task<List<UserTokenDetails>> GetAllUserTokenDtosByFilterAsync(Expression<Func<UserToken, bool>> filter, CancellationToken cancellationToken = default);
    Task<List<UserToken>> GetAllUserTokensByFilterAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
}
