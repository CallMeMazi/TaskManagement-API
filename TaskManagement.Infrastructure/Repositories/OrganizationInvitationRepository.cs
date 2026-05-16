using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationInvitationRepository
    : BaseRepository<OrganizationInvitation>, IOrganizationInvitationRepository
{
    public OrganizationInvitationRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


    public Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(oi => oi.Org).FirstOrDefaultAsync(filter, ct);
    }
}
