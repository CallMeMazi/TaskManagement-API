using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public record RemoveUserFromOrgCommand(
    int OrgOwnerId,
    int UserId,
    int OrgId
) : IRequest<GeneralResult>;