using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.CreateOrg;
public record CreateOrgCommand(
    string OrgName,
    string SecondOrgName,
    string OrgDescription,
    int OwnerId,
    byte MaxUser
) : IRequest<GeneralResult>;