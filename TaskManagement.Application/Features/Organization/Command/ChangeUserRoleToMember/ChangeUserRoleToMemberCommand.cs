using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Organization.Command.ChangeUserRoleToMember;
public record ChangeUserRoleToMemberCommand(
    int OrgOwnerId,
    int OrgId,
    int UserId
) : IRequest<GeneralResult>;