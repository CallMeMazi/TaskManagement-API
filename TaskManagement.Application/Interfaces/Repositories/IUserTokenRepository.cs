using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IUserTokenRepository : IBaseRepository<UserToken, UserTokenDetailsDto>
{
    Task<UserToken?> GetUserByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
