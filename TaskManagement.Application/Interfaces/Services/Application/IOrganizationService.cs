using TaskManagement.Application.DTOs.RequestDTOs.Organization;
using TaskManagement.Application.DTOs.ResponseDTOs.Organization;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IOrganizationService
{
    Task<GeneralResult> AddUserToOrgAsync(AddUserOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeOrgActivityAsync(ChangeActivityOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeUserRoleToAdminAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeUserRoleToMemberAsync(ChangeUserRoleOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> CreateOrgAsync(CreateOrgAppDto command, CancellationToken ct);
    Task<GeneralResult<OrgDetailsDto>> GetOrgByCodeAsync(string orgCode, CancellationToken ct);
    Task<GeneralResult<OrgDetailsDto>> GetOrgByIdAsync(int id, CancellationToken ct);
    Task<GeneralResult> LeaveUserFromOrgAsync(LeaveUserOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> RemoveUserFromOrgAsync(RemoveUserOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteOrgAsync(DeleteOrgAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateOrgAsync(UpdateOrgAppDto command, CancellationToken ct);
}
