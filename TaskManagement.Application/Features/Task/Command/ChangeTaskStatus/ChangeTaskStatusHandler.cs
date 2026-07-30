using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public class ChangeTaskStatusHandler
    : IRequestHandler<ChangeTaskStatusCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskStatusHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeTaskStatusCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        switch (request.TaskStatus)
        {
            case TaskStatusType.Cancel:
                return _taskService.CancelTaskAsync(dto, ct);
            case TaskStatusType.Dead:
                return _taskService.DeadTaskAsync(dto, ct);
            case TaskStatusType.Finished:
                return _taskService.FinishTaskAsync(dto, ct);
            default:
                throw new ArgumentException($"Error in {nameof(ChangeTaskStatusHandler)} Handler!");
        }
    }
}
