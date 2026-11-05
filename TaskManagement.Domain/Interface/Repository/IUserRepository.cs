using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IUserRepository : IBaseRepository<User>
{
    // Query methods
    Task<string?> GetScurityStampAsync(long userId, CancellationToken ct);
    // Command methods
    Task<int> SoftDeleteUserSpAsync(long userId, CancellationToken ct);
}