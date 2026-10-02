using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public record CreateOrgCommand(
    string OrgName,
    string SecondOrgName,
    string OrgDescription,
    long OwnerId,
    byte MaxUser
) : IRequest;