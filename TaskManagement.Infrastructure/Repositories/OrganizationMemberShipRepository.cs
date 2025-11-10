using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Common.Helpers;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationMemberShipRepository : BaseRepository<OrganizationMemberShip>, IOrganizationMemberShipRepository
{

    public OrganizationMemberShipRepository(ApplicationDbContext dbContext) : base(dbContext) { }


    #region Async Method

    public Task<List<OrganizationMemberShip>> GetAllOrgMembershipsAsync(bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.ToListAsync(cancellationToken);
    }

    public Task<OrganizationMemberShip?> GetOrgMembershipByIdAsync(int orgId, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(o => o.Id == orgId, cancellationToken);
    }

    public Task<OrganizationMemberShip?> GetOrgMembershipByFilterAsync(Expression<Func<OrganizationMemberShip, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.FirstOrDefaultAsync(filter, cancellationToken);
    }

    public ValueTask<OrganizationMemberShip?> FindOrgMembershipsByIdsAsync(CancellationToken cancellationToken, params object[] ids)
    {
        return Entities.FindAsync(ids, cancellationToken);
    }

    public Task<bool> IsOrgMembershipExistByFilterAsync(Expression<Func<OrganizationMemberShip, bool>> filter, CancellationToken cancellationToken)
    {
        return Entities.AnyAsync(filter, cancellationToken);
    }

    public async System.Threading.Tasks.Task AddOrgMembershipAsync(OrganizationMemberShip entity, CancellationToken cancellationToken = default)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddOrgMembershipAsync)} method!");

        await Entities.AddAsync(entity, cancellationToken).ConfigureAwait(false);
    }

    public async System.Threading.Tasks.Task AddRangeOrgMembershipsAsync(IEnumerable<OrganizationMemberShip> entities, CancellationToken cancellationToken = default)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(AddRangeOrgMembershipsAsync)} method!");

        await Entities.AddRangeAsync(entities, cancellationToken).ConfigureAwait(false);
    }

    public void UpdateOrgMembership(OrganizationMemberShip entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateOrgMembership)} method!");

        Entities.Update(entity);
    }

    public void UpdateRangeOrgMemberships(IEnumerable<OrganizationMemberShip> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(UpdateRangeOrgMemberships)} method!");

        Entities.UpdateRange(entities);
    }

    public void DeleteOrgMembership(OrganizationMemberShip entity)
    {
        if (entity.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteOrgMembership)} method!");

        Entities.Remove(entity);
    }

    public void DeleteRangeOrgMemberships(IEnumerable<OrganizationMemberShip> entities)
    {
        if (entities.IsNullParameter())
            throw new NullReferenceException($"null parameter in {nameof(DeleteRangeOrgMemberships)} method!");

        Entities.RemoveRange(entities);
    }

    #endregion
}
