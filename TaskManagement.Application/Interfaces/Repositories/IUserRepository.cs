using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IUserRepository : IBaseRepository<User>
{
    System.Threading.Tasks.Task AddRangeUsersAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddUserAsync(User entity, CancellationToken cancellationToken = default);
    void DeleteRangeUsers(IEnumerable<User> entities);
    void DeleteUser(User entity);
    ValueTask<User?> FindUsersByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<UserDetailsDto>> GetAllUserDtosAsync(CancellationToken cancellationToken = default);
    Task<List<User>> GetAllUsersAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<User?> GetUserByIdAsync(int userId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<UserDetailsDto?> GetUserDtoByFilterAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default);
    Task<UserDetailsDto?> GetUserDtoByIdAsync(int userId, CancellationToken cancellationToken = default);
    void UpdateRangeUsers(IEnumerable<User> entities);
    void UpdateUser(User entity);
    Task<bool> IsUserExistByFilterAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken);
}