using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserTokenRepository 
    : BaseRepository<UserToken, UserTokenDetailsDto>, IUserTokenRepository
{
    public UserTokenRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }


    public Task<UserToken?> GetUserByFilterWithUserAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(ut => ut.User).FirstOrDefaultAsync(filter, ct);
    }

}
