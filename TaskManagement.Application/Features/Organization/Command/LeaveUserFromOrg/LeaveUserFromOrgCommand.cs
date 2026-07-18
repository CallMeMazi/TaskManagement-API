using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.LeaveUserFromOrg;
public record LeaveUserFromOrgCommand(
    int UserId,
    int OrgId
) : IRequest<GeneralResult>;