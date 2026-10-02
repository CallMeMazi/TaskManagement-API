using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public class ChangeUserRoleToMemberHandler
    : IRequestHandler<ChangeUserRoleToMemberCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;

    public ChangeUserRoleToMemberHandler(IProjectService projectService, ICommonService common, IUnitOfWork uow)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeUserRoleToMemberCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeUserRoleProjectAppDto>(request);

        var chagneRoleProjectRes = await _projectService.ChangeUserRoleToMemberAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return chagneRoleProjectRes;
    }
}
