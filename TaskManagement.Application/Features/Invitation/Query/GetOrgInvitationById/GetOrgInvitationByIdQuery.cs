using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;

namespace TaskManagement.Application.Features.Invitation.Query.GetOrgInvitationById;
public record GetOrgInvitationByIdQuery(long InvitationId)
    : IRequest<OrgInvitationDetailsDto>;