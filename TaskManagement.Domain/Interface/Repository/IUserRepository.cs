using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IUserRepository : IBaseRepository<User>
{
    // Command methods
    Task<int> SoftDeleteUserSpAsync(long userId, CancellationToken ct);
}