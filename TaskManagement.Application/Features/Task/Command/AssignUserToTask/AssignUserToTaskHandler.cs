using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public class AssignUserToTaskHandler
    : IRequestHandler<AssignUserToTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;
    private readonly IAuditLogService _audit;

    public AssignUserToTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow, IAuditLogService audit)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(AssignUserToTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserTaskAppDto>(request);

        await _taskService.AssignUserToTaskAsync(dto, ct);

        _audit.Stage(new AuditEntryInternalDto(
            AuditActions.TaskAssigned,
            EntityType.Task,
            request.TaskId,
            request.OwnerId,
            EntityType.User,
            request.UserId));

        await _uow.SaveAsync(ct);
    }
}
