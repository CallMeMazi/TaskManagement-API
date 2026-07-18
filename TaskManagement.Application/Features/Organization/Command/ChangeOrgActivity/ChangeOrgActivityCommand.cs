using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeOrgActivity;
public record ChangeOrgActivityCommand(
    int OrgId,
    int OwnerId,
    string OwnerPassword
) : IRequest<GeneralResult>;