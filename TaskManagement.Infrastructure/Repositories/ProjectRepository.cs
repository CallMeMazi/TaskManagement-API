using Microsoft.EntityFrameworkCore;
using TaskManagement.Domin.Entities.BaseEntities;
using TaskManagement.Domin.Interface.Repository;
using TaskManagement.Infrastructure.Persistence.DbContexts;

namespace TaskManagement.Infrastructure.Repositories;
public class ProjectRepository 
    : BaseRepository<Project>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext dbContext)
        : base(dbContext) { }


    // Query methods
    public Task<Project?> GetProjectByIdWithMembersAsync(int projId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(p => p.ProjMember).FirstOrDefaultAsync(p => p.Id == projId, ct);
    }
    public Task<Project?> GetProjectByIdWithOrgAsync(int projId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(p => p.Org).FirstOrDefaultAsync(p => p.Id == projId, ct);
    }
    public Task<Project?> GetProjectByIdWithOrgAndCreatorAsync(int projId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(p => p.Org).Include(p => p.Creator).FirstOrDefaultAsync(p => p.Id == projId, ct);
    }
    public Task<Project?> GetProjectByIdWithOrgAndMembersAsync(int projId, bool isTracking = false, CancellationToken ct = default)
    {
        var query = isTracking ? Entities : Entities.AsNoTracking();
        return query.Include(p => p.Org).Include(p => p.ProjMember).FirstOrDefaultAsync(p => p.Id == projId, ct);
    }

    // Command methods
    public Task<int> SoftDeleteProjectSpAsync(int projId, CancellationToken ct)
    {
        var query = string.Format("EXEC dbo.sp_SoftDeleteProject @ProjectId = {0}", projId);
        return _db.Database.ExecuteSqlRawAsync(query, ct);
    }
}
