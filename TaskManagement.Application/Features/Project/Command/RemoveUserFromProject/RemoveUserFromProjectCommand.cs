using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public record RemoveUserFromProjectCommand(
    int UserId,
    int ProjId,
    int OwnerId
) : IRequest<GeneralResult>;