using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public class CreateTaskHandler
    : IRequestHandler<CreateTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public CreateTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow, IAuditLogService audit)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateTaskAppDto>(request);

        var taskId = await _taskService.CreateTaskAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.TaskCreated,
            EntityType.Task,
            taskId,
            request.UserId,
            Metadata: AuditMetadata.Pairs(("TaskType", request.TaskType.ToString()))));

        await _uow.SaveAsync(ct);
    }
}
