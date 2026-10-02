using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public class ChangeTaskStatusHandler
    : IRequestHandler<ChangeTaskStatusCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public ChangeTaskStatusHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
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

        await _uow.SaveAsync(ct);
    }
}
