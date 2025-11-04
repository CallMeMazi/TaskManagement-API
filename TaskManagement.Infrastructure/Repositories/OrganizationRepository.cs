using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationRepository : BaseRepository<Organization>, IOrganizationRepository
{
    private readonly IMapper _mapper;

    public OrganizationRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext)
    {
        _mapper = mapper;
    }

    #region Async Method

    public Task<List<Organization>> GetAllOrgsAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<List<OrgDetailsDto>> GetAllOrgDtosAsync(CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .ProjectTo<OrgDetailsDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }

    public Task<Organization?> GetOrgByIdAsync(int orgId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(o => o.Id == orgId, cancellationToken);
    }

    public Task<OrgDetailsDto?> GetOrgDtoByIdAsync(int orgId, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(u => u.Id == orgId)
            .ProjectTo<OrgDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<Organization?> GetOrgByFilterAsync(Expression<Func<Organization, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public Task<OrgDetailsDto?> GetOrgDtoByFilterAsync(Expression<Func<Organization, bool>> filter, CancellationToken cancellationToken = default)
    {
        return Entities.AsNoTracking()
            .Where(filter)
            .ProjectTo<OrgDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public ValueTask<Organization?> FindOrgsByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public Task<bool> IsOrgExistByFilterAsync(Expression<Func<Organization, bool>> filter, CancellationToken cancellationToken)
    {
        return Entities.AnyAsync(filter, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddOrgAsync(Organization entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddOrgAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeOrgsAsync(IEnumerable<Organization> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeOrgsAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateOrg(Organization entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateOrg)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeOrgs(IEnumerable<Organization> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeOrgs)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteOrg(Organization entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteOrg)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeOrgs(IEnumerable<Organization> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeOrgs)} method!");

        Entities.RemoveRange(entities);
    }

    #endregion
}
