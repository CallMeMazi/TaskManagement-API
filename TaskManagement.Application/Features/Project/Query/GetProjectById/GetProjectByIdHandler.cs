using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Query.GetProjectById;
public class GetProjectByIdHandler
    : IRequestHandler<GetProjectByIdQuery, GeneralResult<ProjectDetailsDto>>
{
    private readonly IProjectService _projectService;

    public GetProjectByIdHandler(IProjectService projectService)
    {
        _projectService = projectService;
    }

    public Task<GeneralResult<ProjectDetailsDto>> Handle(GetProjectByIdQuery request, CancellationToken ct)
        => _projectService.GetProjectByIdAsync(request.ProjectId, ct);
}
