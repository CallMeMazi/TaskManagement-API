using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class OrganizationRepository 
    : BaseRepository<Organization>, IOrganizationRepository
{
    public OrganizationRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


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
