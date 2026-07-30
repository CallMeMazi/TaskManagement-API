using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllPendingOrgInvitationByOrgId;
public class GetAllPendingOrgInvitationByOrgIdHandler
    : IRequestHandler<GetAllPendingOrgInvitationByOrgIdQuery, GeneralResult<List<OrgInvitationDetailsDto>>>
{
    private readonly IInvitationService _invitationService;

    public GetAllPendingOrgInvitationByOrgIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<GeneralResult<List<OrgInvitationDetailsDto>>> Handle(GetAllPendingOrgInvitationByOrgIdQuery request, CancellationToken ct)
        => _invitationService.GetAllPendingOrgInvitationByOrgIdAsync(request.OrgId, ct);
}
