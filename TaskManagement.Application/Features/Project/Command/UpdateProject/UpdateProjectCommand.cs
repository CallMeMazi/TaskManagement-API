using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.UpdateProject;
public record UpdateProjectCommand(
    long ProjId,
    long OwnerId,
    string ProjName,
    string ProjDescription
) : IRequest<GeneralResult>;