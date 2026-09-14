using TaskManagement.Application.Interfaces.Services.Halper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserTokenRepository 
    : BaseRepository<UserToken>, IUserTokenRepository
{
    public UserTokenRepository(ApplicationDbContext dbContext, IIdGenerator idGenerator)
        : base(dbContext, idGenerator) { }


    // Query methods
    public Task<UserToken?> GetUserTokenByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(ut => ut.User).FirstOrDefaultAsync(filter, ct);
    }
}
