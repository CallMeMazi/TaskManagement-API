using MediatR;
using TaskManagement.Application.Auditing;
using TaskManagement.Application.DTOs.InternalDTOs.Common;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.Services.Log;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Types.Application;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public class DeleteTaskHandler
    : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;
    private readonly IUnitOfWork _uow;
    private readonly IAuditLogService _audit;

    public DeleteTaskHandler(
        ITaskService taskService,
        ICommonService common,
        IUnitOfWork uow,
        IAuditLogService audit)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
        _audit = audit;
    }

    public async System.Threading.Tasks.Task Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserTaskAppDto>(request);

        await _uow.BeginTransactionAsync(ct);

        try
        {
            await _taskService.SoftDeleteTaskAsync(dto, ct);

            _audit.Stage(new AuditEntryInternalDto(
                AuditActions.TaskDeleted,
                EntityType.Task,
                request.TaskId,
                request.UserId));

            await _uow.SaveAsync(ct);
            await _uow.CommitTransactionAsync(ct);
        }
        catch
        {
            await _uow.RollBackTransactionAsync(ct);
            throw;
        }
    }
}
