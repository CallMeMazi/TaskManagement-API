using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.DeleteOrg;
public record DeleteOrgCommand(
    int OrgId,
    int OwnerId,
    string OwnerPassword
) : IRequest<GeneralResult>;