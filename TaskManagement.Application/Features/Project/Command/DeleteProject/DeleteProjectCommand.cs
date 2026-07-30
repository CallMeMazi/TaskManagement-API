using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.DeleteProject;
public record DeleteProjectCommand(
    int OwnerId,
    string UserPassword,
    int ProjId
) : IRequest<GeneralResult>;