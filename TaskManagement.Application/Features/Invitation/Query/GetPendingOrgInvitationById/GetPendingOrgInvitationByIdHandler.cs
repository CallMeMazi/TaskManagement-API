using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetPendingOrgInvitationById;
public class GetPendingOrgInvitationByIdHandler
    : IRequestHandler<GetPendingOrgInvitationByIdQuery, GeneralResult<OrgInvitationDetailsDto>>
{
    private readonly IInvitationService _invitationService;

    public GetPendingOrgInvitationByIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<GeneralResult<OrgInvitationDetailsDto>> Handle(GetPendingOrgInvitationByIdQuery request, CancellationToken ct)
        => _invitationService.GetPendingOrgInvitationByIdAsync(request.InvitationId, ct);
}
