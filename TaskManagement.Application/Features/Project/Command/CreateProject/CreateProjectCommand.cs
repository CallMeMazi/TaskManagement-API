using MediatR;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public record CreateProjectCommand(
    string ProjName,
    string ProjDescription,
    long OrgId,
    long CreatorId,
    byte MaxUser,
    byte MaxTask,
    List<long>? UserIds
) : IRequest;