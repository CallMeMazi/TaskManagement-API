using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllPendingOrgInvitationByOrgId;
public record GetAllPendingOrgInvitationByOrgIdQuery(long OrgId)
    : IRequest<List<OrgInvitationDetailsDto>>;