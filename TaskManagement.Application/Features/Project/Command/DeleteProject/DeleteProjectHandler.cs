using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.DeleteProject;
public class DeleteProjectHandler
    : IRequestHandler<DeleteProjectCommand, GeneralResult>
{
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public DeleteProjectHandler(IProjectService projectService, ICommonService common)
    {
        _projectService = projectService;
        _common = common;
    }

    public Task<GeneralResult> Handle(DeleteProjectCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserProjectAppDto>(request);

        return _projectService.SoftDeleteProjectAsync(dto, ct);
    }
}
