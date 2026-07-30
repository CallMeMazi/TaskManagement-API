using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
internal record ChangeUserRoleToMemberCommand(
    int OwnerId,
    int ProjId,
    int UserId
) : IRequest<GeneralResult>;