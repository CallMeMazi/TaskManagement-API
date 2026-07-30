using MediatR;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public record CreateProjectCommand(
    string ProjName,
    string ProjDescription,
    int OrgId,
    int CreatorId,
    byte MaxUser,
    byte MaxTask,
    List<int>? UserIds
) : IRequest<GeneralResult>;