using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.DeleteProject;
public record DeleteProjectCommand(
    long OwnerId,
    string UserPassword,
    long ProjId
) : IRequest<GeneralResult>;