using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllOrgInvitationByOrgId;
public record GetAllOrgInvitationByOrgIdQuery(long OrgId)
    : IRequest<List<OrgInvitationDetailsDto>>;