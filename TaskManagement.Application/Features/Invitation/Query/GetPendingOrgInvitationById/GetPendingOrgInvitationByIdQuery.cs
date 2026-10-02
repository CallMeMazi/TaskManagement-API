using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;

namespace TaskManagement.Application.Features.Invitation.Query.GetPendingOrgInvitationById;
public record GetPendingOrgInvitationByIdQuery(long InvitationId)
    : IRequest<OrgInvitationDetailsDto>;