using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
internal record ChangeUserRoleToAdminCommand(
    int OwnerId,
    int ProjId,
    int UserId
) : IRequest<GeneralResult>;