using MediatR;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetPendingOrgInvitationById;
public record GetPendingOrgInvitationByIdQuery(int InvitationId)
    : IRequest<GeneralResult<OrgInvitationDetailsDto>>;