using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IProjectRepository : IBaseRepository<Project>
{
    Task<Project?> GetProjectByIdWithOrgAsync(int projId, bool isTracking = false, CancellationToken ct = default);
    Task<int> SoftDeleteProjectSpAsync(int projectId, CancellationToken ct);
    Task<Project?> GetProjectByIdWithMembersAsync(int projId, bool isTracking = false, CancellationToken ct = default);
}

