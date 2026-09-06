using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.DeleteOrg;
public record DeleteOrgCommand(
    long OrgId,
    long OwnerId,
    string OwnerPassword
) : IRequest<GeneralResult>;