using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Invitation.Query.GetAllOrgInvitationByOrgId;
public record GetAllOrgInvitationByOrgIdQuery(int OrgId)
    : IRequest<GeneralResult<List<OrgInvitationDetailsDto>>>;