using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.UpdateOrg;
public record UpdateOrgCommand(
    int UserId,
    int OrgId,
    string OrgName,
    string SecondOrgName,
    string OrgDescription
) : IRequest<GeneralResult>;