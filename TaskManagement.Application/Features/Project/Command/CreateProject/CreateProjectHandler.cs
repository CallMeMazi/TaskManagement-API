using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.CreateProject;
public class CreateProjectHandler
    : IRequestHandler<CreateProjectCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public CreateProjectHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(CreateProjectCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateProjectAppDto>(request);

        var createProjectRes = await _projectService.CreateProjectAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return createProjectRes;
    }
}
