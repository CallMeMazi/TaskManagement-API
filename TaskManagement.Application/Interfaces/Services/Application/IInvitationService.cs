using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IInvitationService
{
    Task<long> AcceptInvitationAsync(AcceptOrgInvitationAppDto command, CancellationToken ct);
    Task<string> GenerateInviteLinkByUserIdAsync(CreateOrgInvitatoinAppDto command, CancellationToken ct);
    Task<List<OrgInvitationDetailsDto>> GetAllOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct);
    Task<List<OrgInvitationDetailsDto>> GetAllPendingOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct);
    Task<OrgInvitationDetailsDto> GetOrgInvitationByIdAsync(long id, CancellationToken ct);
    Task<OrgInvitationDetailsDto> GetPendingOrgInvitationByIdAsync(long id, CancellationToken ct);
    Task RevokeInvitationAsync(RevokeOrgInvitationAppDto command, CancellationToken ct);
}
