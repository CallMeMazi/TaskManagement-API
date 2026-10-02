using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IProjectService
{
    Task AddUserToProjectAysnc(AddRemoveUserProjectAppDto command, CancellationToken ct);
    Task CancelProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task ChangeProjectActivityAsync(ChangeProjectActivityAppDto command, CancellationToken ct);
    Task ChangeProjectProgressAsync(ChangeProjectProgressAppDto command, CancellationToken ct);
    Task ChangeProjectStatusToAdjournmentAsync(UserProjectAppDto command, CancellationToken ct);
    Task ChangeProjectStatusToInProgressAsync(UserProjectAppDto command, CancellationToken ct);
    Task ChangeUserRoleToAdminAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct);
    Task ChangeUserRoleToMemberAsync(ChangeUserRoleProjectAppDto command, CancellationToken ct);
    Task CreateProjectAsync(CreateProjectAppDto command, CancellationToken ct);
    Task FinishProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task<ProjectDetailsDto> GetProjectByIdAsync(long projId, CancellationToken ct);
    Task RemoveUserFromProjectAsync(AddRemoveUserProjectAppDto command, CancellationToken ct);
    Task SoftDeleteProjectAsync(UserProjectAppDto command, CancellationToken ct);
    Task UpdateProjectAsync(UpdateProjectAppDto command, CancellationToken ct);
}
