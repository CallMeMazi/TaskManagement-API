using MediatR;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public record ChangeOrgActivityCommand(
    long OrgId,
    long OwnerId,
    string OwnerPassword,
    bool Activity
) : IRequest;