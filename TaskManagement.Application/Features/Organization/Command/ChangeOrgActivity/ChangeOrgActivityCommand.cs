using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public record ChangeOrgActivityCommand(
    long OrgId,
    long OwnerId,
    string OwnerPassword
) : IRequest<GeneralResult>;