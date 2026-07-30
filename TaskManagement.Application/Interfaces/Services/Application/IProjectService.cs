using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IProjectService
{
    Task<GeneralResult> AddUserToProjectAysnc(AddRemoveUserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> CancelProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectProgressAsync(ChangeProjectProgressAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectStatusToAdjournmentAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeProjectStatusToInProgressAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeUserRoleToAdminAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeUserRoleToMemberAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> CreateProjectAsync(CreateProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> FinishProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult<ProjectDetailsDto>> GetProjectByIdAsync(int projId, CancellationToken ct);
    Task<GeneralResult> RemoveUserFromProjectAsync(AddRemoveUserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken ct);
}
