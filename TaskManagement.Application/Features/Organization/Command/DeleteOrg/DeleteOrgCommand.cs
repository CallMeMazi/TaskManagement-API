using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.DeleteOrg;
public record DeleteOrgCommand(
    long OrgId,
    long OwnerId,
    string OwnerPassword
) : IRequest;