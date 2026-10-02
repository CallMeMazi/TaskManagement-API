using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public record UpdateOrgCommand(
    long UserId,
    long OrgId,
    string OrgName,
    string SecondOrgName,
    string OrgDescription
) : IRequest;