using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserTokenRepository 
    : BaseRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


    // Query methods
    public Task<UserToken?> GetUserTokenByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(ut => ut.User).FirstOrDefaultAsync(filter, ct);
    }
}
