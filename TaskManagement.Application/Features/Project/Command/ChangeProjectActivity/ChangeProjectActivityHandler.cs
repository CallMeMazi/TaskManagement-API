using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public class ChangeProjectActivityHandler
    : IRequestHandler<ChangeProjectActivityCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public ChangeProjectActivityHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(ChangeProjectActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeProjectActivityAppDto>(request);

        await _projectService.ChangeProjectActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
