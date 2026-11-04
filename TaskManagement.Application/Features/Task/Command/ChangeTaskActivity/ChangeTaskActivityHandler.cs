using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityHandler
    : IRequestHandler<ChangeTaskActivityCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public ChangeTaskActivityHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow, IAuditLogService audit)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(ChangeTaskActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeTaskActivityAppDto>(request);

        await _taskService.ChangeTaskActivityAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            request.Activity ? AuditActions.TaskActivated : AuditActions.TaskDeactivated,
            EntityType.Task,
            request.TaskId,
            request.UserId,
            Metadata: AuditMetadata.Pairs(("IsActive", request.Activity ? "true" : "false"))));

        await _uow.SaveAsync(ct);
    }
}
