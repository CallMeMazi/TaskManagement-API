using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IProjectRepository : IBaseRepository<Project>
{
    System.Threading.Tasks.Task AddProjectAsync(Project entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddRangeProjectsAsync(IEnumerable<Project> entities, CancellationToken cancellationToken = default);
    void DeleteProject(Project entity);
    void DeleteRangeProjects(IEnumerable<Project> entities);
    ValueTask<Project?> FindProjectsByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<ProjectDetailsDto>> GetAllProjectDtosAsync(CancellationToken cancellationToken = default);
    Task<List<Project>> GetAllProjectsAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<Project?> GetProjectByFilterAsync(Expression<Func<Project, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<Project?> GetProjectByIdAsync(Guid projectId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<ProjectDetailsDto?> GetProjectDtoByFilterAsync(Expression<Func<Project, bool>> filter, CancellationToken cancellationToken = default);
    Task<ProjectDetailsDto?> GetProjectDtoByIdAsync(Guid projectId, CancellationToken cancellationToken = default);
    void UpdateProject(Project entity);
    void UpdateRangeProjects(IEnumerable<Project> entities);
}

