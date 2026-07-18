using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.AddUserToOrg;
public record AddUserToOrgCommand(
    int UserId,
    int OrgId
) : IRequest<GeneralResult>;