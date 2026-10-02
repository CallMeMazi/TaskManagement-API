using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public class ChangeProjectActivityHandler
    : IRequestHandler<ChangeProjectActivityCommand, GeneralResult>
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

    public async Task<GeneralResult> Handle(ChangeProjectActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeProjectActivityAppDto>(request);

        var chagneActivityProjectRes = await _projectService.ChangeProjectActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return chagneActivityProjectRes;
    }
}
