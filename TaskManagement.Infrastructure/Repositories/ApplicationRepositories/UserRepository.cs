using TaskManagement.Application.Interfaces.Services.Halper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class UserRepository 
    : BaseRepository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext dbContext, ICommonService commonService)
        : base(dbContext, commonService) { }


    // Command methods
    public Task<int> SoftDeleteUserSpAsync(long userId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteUser @UserId = {0}", userId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }

}
