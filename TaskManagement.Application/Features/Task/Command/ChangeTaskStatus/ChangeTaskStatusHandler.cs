using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;
using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskStatus;
public class ChangeTaskStatusHandler
    : IRequestHandler<ChangeTaskStatusCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskStatusHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeTaskStatusCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        GeneralResult changeTaskStatusRes;
        switch (request.TaskStatus)
        {
            case TaskStatusType.Cancel:
                changeTaskStatusRes = await _taskService.CancelTaskAsync(dto, ct);
                break;
            case TaskStatusType.Dead:
                changeTaskStatusRes = await _taskService.DeadTaskAsync(dto, ct);
                break;
            case TaskStatusType.Finished:
                changeTaskStatusRes = await _taskService.FinishTaskAsync(dto, ct);
                break;
            default:
                throw new ArgumentException($"Error in {nameof(ChangeTaskStatusHandler)} Handler!");
        }

        await _uow.SaveAsync(ct);

        return changeTaskStatusRes;
    }
}
