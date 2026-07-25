using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Application.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetOrgInvitationById;
public class GetOrgInvitationByIdHandler
    : IRequestHandler<GetOrgInvitationByIdQuery, GeneralResult<OrgInvitationDetailsDto>>
{
    private readonly InvitationService _invitationService;

    public GetOrgInvitationByIdHandler(InvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<GeneralResult<OrgInvitationDetailsDto>> Handle(GetOrgInvitationByIdQuery request, CancellationToken ct)
        => _invitationService.GetOrgInvitationByIdAsync(request.InvitationId, ct);
}
