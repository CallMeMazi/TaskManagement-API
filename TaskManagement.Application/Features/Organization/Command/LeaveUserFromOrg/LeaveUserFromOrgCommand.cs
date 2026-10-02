using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public record LeaveUserFromOrgCommand(
    long UserId,
    long OrgId
) : IRequest;