using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetOrgInvitationById;
public record GetOrgInvitationByIdQuery(int InvitationId)
    : IRequest<GeneralResult<OrgInvitationDetailsDto>>;