using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllOrgInvitationByOrgId;
public class GetAllOrgInvitationByOrgIdHandler
    : IRequestHandler<GetAllOrgInvitationByOrgIdQuery, List<OrgInvitationDetailsDto>>
{
    private readonly IInvitationService _invitationService;

    public GetAllOrgInvitationByOrgIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<List<OrgInvitationDetailsDto>> Handle(GetAllOrgInvitationByOrgIdQuery request, CancellationToken ct)
        => _invitationService.GetAllOrgInvitationByOrgIdAsync(request.OrgId, ct);
}
