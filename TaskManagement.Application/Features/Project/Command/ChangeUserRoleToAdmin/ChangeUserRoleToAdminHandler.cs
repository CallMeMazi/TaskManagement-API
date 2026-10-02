using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToAdmin;
public class ChangeUserRoleToAdminHandler
    : IRequestHandler<ChangeUserRoleToAdminCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public ChangeUserRoleToAdminHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeUserRoleToAdminCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeUserRoleProjectAppDto>(request);

        var chagneRoleProjectRes = await _projectService.ChangeUserRoleToAdminAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return chagneRoleProjectRes;
    }
}
