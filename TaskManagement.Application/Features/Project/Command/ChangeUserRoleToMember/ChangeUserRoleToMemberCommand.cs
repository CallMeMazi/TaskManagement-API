using MediatR;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public record ChangeUserRoleToMemberCommand(
    long OwnerId,
    long ProjId,
    long UserId
) : IRequest;