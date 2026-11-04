using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Statuses;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public class ChangeTaskStatusHandler
    : IRequestHandler<ChangeTaskStatusCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeTaskStatusHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow, IAuditLogService audit)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(ChangeTaskStatusCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserTaskAppDto>(request);

        switch (request.TaskStatus)
        {
            case TaskStatusType.Cancel:
                await _taskService.CancelTaskAsync(dto, ct);
                break;
            case TaskStatusType.Dead:
                await _taskService.DeadTaskAsync(dto, ct);
                break;
            case TaskStatusType.Finished:
                await _taskService.FinishTaskAsync(dto, ct);
                break;
            default:
                throw new ArgumentException($"Error in {nameof(ChangeTaskStatusHandler)} Handler!");
        }

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.TaskStatusChanged,
            EntityType.Task,
            request.TaskId,
            request.UserId,
            Metadata: AuditMetadata.Pairs(("Status", request.TaskStatus.ToString()))));

        await _uow.SaveAsync(ct);
    }
}
