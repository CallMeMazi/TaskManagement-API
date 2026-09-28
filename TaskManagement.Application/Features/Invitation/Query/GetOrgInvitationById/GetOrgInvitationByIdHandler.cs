using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetOrgInvitationById;
public class GetOrgInvitationByIdHandler
    : IRequestHandler<GetOrgInvitationByIdQuery, GeneralResult<OrgInvitationDetailsDto>>
{
    private readonly IInvitationService _invitationService;

    public GetOrgInvitationByIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<GeneralResult<OrgInvitationDetailsDto>> Handle(GetOrgInvitationByIdQuery request, CancellationToken ct)
        => _invitationService.GetOrgInvitationByIdAsync(request.InvitationId, ct);
}
