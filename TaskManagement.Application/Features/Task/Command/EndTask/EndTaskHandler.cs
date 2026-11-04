using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public class EndTaskHandler
    : IRequestHandler<EndTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ITaskInfoService _taskInfoService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public EndTaskHandler(
        ITaskService taskService,
        ITaskInfoService taskInfoService,
        ICommonService common,
        IUnitOfWork uow,
        IAuditLogService audit)
    {
        _taskService = taskService;
        _taskInfoService = taskInfoService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(EndTaskCommand request, CancellationToken ct)
    {
        var endTaskDto = _common.Mapper.Map<UserTaskAppDto>(request);

        await _taskService.EndTaskAsync(endTaskDto, ct);

        var createTaskInfoDto = _common.Mapper.Map<CreateTaskInfoAppDto>(request);

        await _taskInfoService.CreateTaskInfoAsync(createTaskInfoDto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.TaskEnded,
            EntityType.Task,
            request.TaskId,
            request.UserId));

        await _uow.SaveAsync(ct);
    }
}
