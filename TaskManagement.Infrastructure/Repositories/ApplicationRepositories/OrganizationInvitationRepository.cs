using TaskManagement.Application.Interfaces.Services.Halper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Domain.Entities.BaseEntities;
using TaskManagement.Domain.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories.ApplicationRepositories;
public class OrganizationInvitationRepository
    : BaseRepository<OrganizationInvitation>, IOrganizationInvitationRepository
{
    public OrganizationInvitationRepository(ApplicationDbContext dbContext, ICommonService commonService)
        : base(dbContext, commonService) { }


    public Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(oi => oi.Org).FirstOrDefaultAsync(filter, ct);
    }
}
