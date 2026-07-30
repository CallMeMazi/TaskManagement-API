using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.UpdateProject;
internal record UpdateProjectCommand(
    int ProjId,
    int OwnerId,
    string ProjName,
    string ProjDescription
) : IRequest<GeneralResult>;