using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public record ChangeUserRoleToMemberCommand(
    int OwnerId,
    int ProjId,
    int UserId
) : IRequest<GeneralResult>;