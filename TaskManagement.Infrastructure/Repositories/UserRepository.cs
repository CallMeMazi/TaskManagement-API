using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.User;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly IMapper _mapper;

    public UserRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<User>> GetAllUsersAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<UserDetailsDto>> GetAllUserDtosAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<UserDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<User?> GetUserByIdAsync(int userId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    }

    public Task<UserDetailsDto?> GetUserDtoByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == userId)
            .ProjectTo<UserDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public Task<UserDetailsDto?> GetUserDtoByFilterAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<UserDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<User?> FindUsersByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddUserAsync(User entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddUserAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeUsersAsync(IEnumerable<User> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeUsersAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateUser(User entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateUser)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeUsers(IEnumerable<User> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeUsers)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteUser(User entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteUser)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeUsers(IEnumerable<User> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeUsers)} method!");

        Entities.RemoveRange(entities);
    }

    public Task<bool> IsUserExistByFilterAsync(Expression<Func<User, bool>> filter, CancellationToken cancellationToken)
    {
        return Entities.AnyAsync(filter, cancellationToken);
    }

    #endregion
}
