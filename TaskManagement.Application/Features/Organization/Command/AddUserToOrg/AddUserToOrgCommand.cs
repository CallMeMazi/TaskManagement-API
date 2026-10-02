using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
public record AddUserToOrgCommand(
    long UserId,
    long OrgId
) : IRequest;