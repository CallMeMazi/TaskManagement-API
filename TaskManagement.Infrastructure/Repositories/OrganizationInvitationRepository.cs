using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationInvitationRepository
    : BaseRepository<OrganizationInvitation, OrgInvitationDetailsDto>, IOrganizationInvitationRepository
{
    public OrganizationInvitationRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }


    public Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(oi => oi.Org).FirstOrDefaultAsync(filter, ct);
    }
}
