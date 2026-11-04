using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Project.Command.ChangeUserRoleToMember;
public class ChangeUserRoleToMemberHandler
    : IRequestHandler<ChangeUserRoleToMemberCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeUserRoleToMemberHandler(
        IProjectService projectService,
        ICommonService common,
        IUnitOfWork uow,
        IAuditLogService audit)
    {
        _projectService = projectService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(ChangeUserRoleToMemberCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeUserRoleProjectAppDto>(request);

        await _projectService.ChangeUserRoleToMemberAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.ProjectRoleChanged,
            EntityType.Project,
            request.ProjId,
            request.OwnerId,
            EntityType.User,
            request.UserId,
            AuditMetadata.Pairs(("Role", "Member"))));

        await _uow.SaveAsync(ct);
    }
}
