using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public record ChangeUserRoleToMemberCommand(
    long OwnerId,
    long ProjId,
    long UserId
) : IRequest<GeneralResult>;