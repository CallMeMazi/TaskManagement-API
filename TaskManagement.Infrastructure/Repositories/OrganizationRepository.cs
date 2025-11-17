using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationRepository 
    : BaseRepository<Organization, OrgDetailsDto>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext dbContext, IMapper mapper)
        : base(dbContext, mapper) { }


    public Task<Organization?> GetOrgByIdWithOwnerAsync(int orgId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(ut => ut.Owner).FirstOrDefaultAsync(o => o.Id == orgId, ct);
    }
    public Task<Organization?> GetOrgByIdWithMembersAsync(int orgId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(ut => ut.Members).FirstOrDefaultAsync(o => o.Id == orgId, ct);
    }
}
