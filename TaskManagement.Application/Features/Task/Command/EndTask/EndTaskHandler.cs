using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public class EndTaskHandler
    : IRequestHandler<EndTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly ITaskInfoService _taskInfoService;
    private readonly IMapper _mapper;

    public EndTaskHandler(ITaskService taskService, ITaskInfoService taskInfoService, IMapper mapper)
    {
        _taskService = taskService;
        _taskInfoService = taskInfoService;
        _mapper = mapper;
    }

    public async Task<GeneralResult> Handle(EndTaskCommand request, CancellationToken ct)
    {
        // TransActional

        var dto = _mapper.Map<UserTaskAppDto>(request);

        var endTaskRes = await _taskService.EndTaskAsync(dto, ct);

        // Create taskinfo after ended task (Event)
        return await _taskInfoService.CreateTaskInfoAsync
            (new CreateTaskInfoAppDto(request.TaskId, request.UserId), ct);
    }
}
