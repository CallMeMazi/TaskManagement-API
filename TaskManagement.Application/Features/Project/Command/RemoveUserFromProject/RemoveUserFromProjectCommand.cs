using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public record RemoveUserFromProjectCommand(
    long UserId,
    long ProjId,
    long OwnerId
) : IRequest<GeneralResult>;