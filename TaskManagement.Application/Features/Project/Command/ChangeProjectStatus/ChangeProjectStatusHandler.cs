using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Project;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Project.Command.ChangeProjectStatus;
public class ChangeProjectStatusHandler
    : IRequestHandler<ChangeProjectStatusCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly IProjectService _projectService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeProjectStatusHandler(
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

    public async System.Threading.Tasks.Task Handle(ChangeProjectStatusCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserProjectAppDto>(request);

        switch (request.ProjectStatus)
        {
            case ProjectStatusType.InProgress:
                await _projectService.ChangeProjectStatusToInProgressAsync(dto, ct);
                break;
            case ProjectStatusType.Adjournment:
                await _projectService.ChangeProjectStatusToAdjournmentAsync(dto, ct);
                break;
            case ProjectStatusType.Cancel:
                await _projectService.CancelProjectAsync(dto, ct);
                break;
            case ProjectStatusType.Finished:
                await _projectService.FinishProjectAsync(dto, ct);
                break;
            default:
                throw new ArgumentException($"Error in {nameof(ChangeProjectStatusHandler)} Handler!");
        }

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.ProjectStatusChanged,
            EntityType.Project,
            request.ProjId,
            request.OwnerId,
            Metadata: AuditMetadata.Pairs(("Status", request.ProjectStatus.ToString()))));

        await _uow.SaveAsync(ct);
    }
}
