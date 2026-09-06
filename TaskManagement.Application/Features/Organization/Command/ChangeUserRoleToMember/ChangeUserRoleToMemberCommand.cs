using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToMember;
public record ChangeUserRoleToMemberCommand(
    long OrgOwnerId,
    long OrgId,
    long UserId
) : IRequest<GeneralResult>;