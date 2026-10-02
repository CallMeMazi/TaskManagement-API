using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Project.Command.RemoveUserFromProject;
public class RemoveUserFromProjectHandler
    : IRequestHandler<RemoveUserFromProjectCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public RemoveUserFromProjectHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(RemoveUserFromProjectCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserProjectAppDto>(request);

        await _projectService.RemoveUserFromProjectAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
