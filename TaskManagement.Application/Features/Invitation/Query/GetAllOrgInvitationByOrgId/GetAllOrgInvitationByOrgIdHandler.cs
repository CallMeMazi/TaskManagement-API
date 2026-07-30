using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllOrgInvitationByOrgId;
public class GetAllOrgInvitationByOrgIdHandler
    : IRequestHandler<GetAllOrgInvitationByOrgIdQuery, GeneralResult<List<OrgInvitationDetailsDto>>>
{
    private readonly IInvitationService _invitationService;

    public GetAllOrgInvitationByOrgIdHandler(IInvitationService invitationService)
    {
        _invitationService = invitationService;
    }

    public Task<GeneralResult<List<OrgInvitationDetailsDto>>> Handle(GetAllOrgInvitationByOrgIdQuery request, CancellationToken ct)
        => _invitationService.GetAllOrgInvitationByOrgIdAsync(request.OrgId, ct);
}
