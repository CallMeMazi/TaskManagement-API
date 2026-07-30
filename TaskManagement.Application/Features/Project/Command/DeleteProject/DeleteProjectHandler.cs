using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.DeleteProject;
public class DeleteProjectHandler
    : IRequestHandler<DeleteProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public DeleteProjectHandler(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(DeleteProjectCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<UserProjectAppDto>(request);

        return _projectService.SoftDeleteProjectAsync(dto, ct);
    }
}
