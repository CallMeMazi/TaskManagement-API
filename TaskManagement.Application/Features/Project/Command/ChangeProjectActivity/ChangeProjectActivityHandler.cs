using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectActivity;
public class ChangeProjectActivityHandler
    : IRequestHandler<ChangeProjectActivityCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeProjectActivityHandler(
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

    public async System.Threading.Tasks.Task Handle(ChangeProjectActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeProjectActivityAppDto>(request);

        await _projectService.ChangeProjectActivityAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            request.Activity ? AuditActions.ProjectActivated : AuditActions.ProjectDeactivated,
            EntityType.Project,
            request.ProjId,
            request.OwnerId,
            Metadata: AuditMetadata.Pairs(("IsActive", request.Activity ? "true" : "false"))));

        await _uow.SaveAsync(ct);
    }
}
