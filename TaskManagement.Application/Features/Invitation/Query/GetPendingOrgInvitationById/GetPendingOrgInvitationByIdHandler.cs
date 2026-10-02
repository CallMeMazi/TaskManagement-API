using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.Invitation.Query.GetPendingOrgInvitationById;
public class GetPendingOrgInvitationByIdHandler
    : IRequestHandler<GetPendingOrgInvitationByIdQuery, OrgInvitationDetailsDto>
{
    private readonly IInvitationService _invitationService;

    public GetPendingOrgInvitationByIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<OrgInvitationDetailsDto> Handle(GetPendingOrgInvitationByIdQuery request, CancellationToken ct)
        => _invitationService.GetPendingOrgInvitationByIdAsync(request.InvitationId, ct);
}
