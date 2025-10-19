using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IProjectService
{
    Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken cancellationToken);
    Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangeProjectStatusAsync(ChangeProjectStatusAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken cancellationToken);
}
