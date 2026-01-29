using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IUserRepository : IBaseRepository<User>
{
    // Command methods
    Task<int> SoftDeleteUserSpAsync(int userId, CancellationToken ct);
}