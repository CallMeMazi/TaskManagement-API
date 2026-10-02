using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectProgress;
public class ChangeProjectProgressHandler
    : IRequestHandler<ChangeProjectProgressCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public ChangeProjectProgressHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(ChangeProjectProgressCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeProjectProgressAppDto>(request);

        await _projectService.ChangeProjectProgressAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
