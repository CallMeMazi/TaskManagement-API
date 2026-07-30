using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
internal record RemoveUserFromProjectCommand(
    int UserId,
    int ProjId,
    int OwnerId
) : IRequest<GeneralResult>;