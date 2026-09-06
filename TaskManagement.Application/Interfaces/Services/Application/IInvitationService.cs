using TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface IInvitationService
{
    Task<GeneralResult<long>> AcceptInvitationAsync(AcceptOrgInvitationAppDto command, CancellationToken ct);
    Task<GeneralResult<string>> GenerateInviteLinkByUserIdAsync(CreateOrgInvitatoinAppDto command, CancellationToken ct);
    Task<GeneralResult<List<OrgInvitationDetailsDto>>> GetAllOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct);
    Task<GeneralResult<List<OrgInvitationDetailsDto>>> GetAllPendingOrgInvitationByOrgIdAsync(long orgId, CancellationToken ct);
    Task<GeneralResult<OrgInvitationDetailsDto>> GetOrgInvitationByIdAsync(long id, CancellationToken ct);
    Task<GeneralResult<OrgInvitationDetailsDto>> GetPendingOrgInvitationByIdAsync(long id, CancellationToken ct);
    Task<GeneralResult> RevokeInvitationAsync(RevokeOrgInvitationAppDto command, CancellationToken ct);
}
