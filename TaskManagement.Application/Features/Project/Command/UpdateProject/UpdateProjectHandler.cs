using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.UpdateProject;
internal class UpdateProjectHandler
    : IRequestHandler<UpdateProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public UpdateProjectHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(UpdateProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UpdateProjectAppDto>(request);

        return _projectService.UpdateProjectAsync(dto, ct);
    }
}
