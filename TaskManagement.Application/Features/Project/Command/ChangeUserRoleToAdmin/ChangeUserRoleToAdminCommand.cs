using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
public record ChangeUserRoleToAdminCommand(
    long OwnerId,
    long ProjId,
    long UserId
) : IRequest<GeneralResult>;