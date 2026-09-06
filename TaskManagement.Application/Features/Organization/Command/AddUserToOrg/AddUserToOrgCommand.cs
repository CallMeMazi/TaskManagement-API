using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
public record AddUserToOrgCommand(
    long UserId,
    long OrgId
) : IRequest<GeneralResult>;