using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public class CreateProjectHandler
    : IRequestHandler<CreateProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public CreateProjectHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<CreateProjectAppDto>(request);

        return _projectService.CreateProjectAsync(dto, ct);
    }
}
