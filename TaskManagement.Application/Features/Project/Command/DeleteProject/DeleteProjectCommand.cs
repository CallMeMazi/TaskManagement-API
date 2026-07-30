using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.DeleteProject;
internal record DeleteProjectCommand(
    int OwnerId,
    string UserPassword,
    int ProjId
) : IRequest<GeneralResult>;