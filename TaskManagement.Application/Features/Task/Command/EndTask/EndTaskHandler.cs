using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public class EndTaskHandler
    : IRequestHandler<EndTaskCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ITaskInfoService _taskInfoService;
    private readonly IMapper _mapper;

    public EndTaskHandler(ITaskService taskService, ITaskInfoService taskInfoService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _taskInfoService = taskInfoService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(EndTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        await _taskService.EndTaskAsync(dto, ct);

        // Create taskinfo after ended task (Event)
        var endTaskRes = await _taskInfoService.CreateTaskInfoAsync(new CreateTaskInfoAppDto(
            request.TaskId,
            request.UserId,
            request.TaskInfoDescription)
            , ct
        );

        await _uow.SaveAsync(ct);

        return endTaskRes;
    }
}
