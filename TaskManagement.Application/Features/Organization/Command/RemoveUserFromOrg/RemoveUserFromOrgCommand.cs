using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.RemoveUserFromOrg;
public record RemoveUserFromOrgCommand(
    long OrgOwnerId,
    long UserId,
    long OrgId
) : IRequest<GeneralResult>;