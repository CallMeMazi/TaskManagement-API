using TaskManagement.Application.DTOs.ApplicationDTOs.Project;
using TaskManagement.Application.DTOs.SharedDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Main;
public interface IProjectService
{
    Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken ct);
    Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectStatusAsync(ChangeProjectStatusAppDto command, CancellationToken ct);
    Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken ct);
}
