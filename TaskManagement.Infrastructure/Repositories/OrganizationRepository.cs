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


    // Query methods
    public Task<Organization?> GetOrgByIdWithOwnerAsync(int orgId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(o => o.Owner).FirstOrDefaultAsync(o => o.Id == orgId, ct);
    }
    public Task<Organization?> GetOrgByIdWithMembersAsync(int orgId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(o => o.Members).FirstOrDefaultAsync(o => o.Id == orgId, ct);
    }

    // Command methods
    public Task<int> SoftDeleteOrgSpAsync(int orgId, CancellationToken ct = default)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteOrg @OrgId = {0}", orgId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }
}
