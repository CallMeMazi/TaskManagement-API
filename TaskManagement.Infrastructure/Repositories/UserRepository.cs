using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserRepository 
    : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


    // Command methods
    public Task<int> SoftDeleteUserSpAsync(int userId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteUser @UserId = {0}", userId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }

}
