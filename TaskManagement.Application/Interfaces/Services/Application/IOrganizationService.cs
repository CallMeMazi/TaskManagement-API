using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IOrganizationService
{
    Task AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct);
    Task ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken ct);
    Task ChangeUserRoleToAdminAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct);
    Task ChangeUserRoleToMemberAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct);
    Task CreateOrgAsync(CreateOrgAppDto command, CancellationToken ct);
    Task<OrgDetailsDto> GetOrgByCodeAsync(string orgCode, CancellationToken ct);
    Task<OrgDetailsDto> GetOrgByIdAsync(long id, CancellationToken ct);
    Task LeaveUserFromOrgAsync(LeaveUserOrgAppDto command, CancellationToken ct);
    Task RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct);
    Task SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken ct);
    Task UpdateOrgAsync(UpdateOrgAppDto command, CancellationToken ct);
}
