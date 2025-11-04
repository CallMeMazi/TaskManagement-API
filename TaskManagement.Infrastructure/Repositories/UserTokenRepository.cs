using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.UserToken;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserTokenRepository : BaseRepository<UserToken>, IUserTokenRepository
{
    private readonly IMapper _mapper;

    public UserTokenRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext) 
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<UserToken>> GetAllUserTokensAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<UserToken>> GetAllUserTokensByFilterAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Where(filter).ToListAsync(cancellationToken);
    }

    public Task<List<UserTokenDetails>> GetAllUserTokenDtosByFilterAsync(Expression<Func<UserToken, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<UserTokenDetails>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<UserToken?> GetUserTokenByIdAsync(int userTokenId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(u => u.Id == userTokenId, cancellationToken);
    }

    public Task<UserToken?> GetUserTokenByFilterAsync(Expression<Func<UserToken, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public ValueTask<UserToken?> FindUserTokensByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddUserTokenAsync(UserToken entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddUserTokenAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeUserTokensAsync(IEnumerable<UserToken> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeUserTokensAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateUserToken(UserToken entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateUserToken)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeUserTokens(IEnumerable<UserToken> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeUserTokens)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteUserToken(UserToken entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteUserToken)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeUserTokens(IEnumerable<UserToken> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeUserTokens)} method!");

        Entities.RemoveRange(entities);
    }

    public Task<int> GetCountByFilterAsync(Expression<Func<UserToken, bool>> filter)
    {
        return Entities.CountAsync(filter);
    }

    #endregion
}
