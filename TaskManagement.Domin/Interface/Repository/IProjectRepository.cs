using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> GetProjectByIdWithMembersAsync(int projId, bool isTracking = false, CancellationToken ct = default);
    Task<Project?> GetProjectByIdWithOrgAsync(int projId, bool isTracking = false, CancellationToken ct = default);
    Task<Project?> GetProjectByIdWithOrgAndCreatorAsync(int projId, bool isTracking = false, CancellationToken ct = default);
    Task<int> SoftDeleteProjectSpAsync(int projectId, CancellationToken ct);
    Task<Project?> GetProjectByIdWithOrgAndMembersAsync(int projId, bool isTracking = false, CancellationToken ct = default);
}

