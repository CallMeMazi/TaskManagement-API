using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public record LeaveUserFromOrgCommand(
    long UserId,
    long OrgId
) : IRequest<GeneralResult>;