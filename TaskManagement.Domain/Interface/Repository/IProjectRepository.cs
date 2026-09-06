using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> GetProjectByIdWithOrgAsync(long projId, bool isTracking = false, CancellationToken ct = default);
    Task<int> SoftDeleteProjectSpAsync(long projectId, CancellationToken ct);
    Task<Project?> GetProjectByIdWithMembersAsync(long projId, bool isTracking = false, CancellationToken ct = default);
}

