using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public class RemoveUserFromProjectHandler
    : IRequestHandler<RemoveUserFromProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public RemoveUserFromProjectHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(RemoveUserFromProjectCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserProjectAppDto>(request);

        return _projectService.RemoveUserFromProjectAsync(dto, ct);
    }
}
