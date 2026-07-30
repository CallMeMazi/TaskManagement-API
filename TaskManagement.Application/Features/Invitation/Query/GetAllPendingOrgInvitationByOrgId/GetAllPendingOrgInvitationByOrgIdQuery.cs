using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllPendingOrgInvitationByOrgId;
public record GetAllPendingOrgInvitationByOrgIdQuery(int OrgId)
    : IRequest<GeneralResult<List<OrgInvitationDetailsDto>>>;