using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.EndTask;
public class EndTaskHandler
    : IRequestHandler<EndTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ITaskInfoService _taskInfoService;
    private readonly ICommonService _common;

    public EndTaskHandler(ITaskService taskService, ITaskInfoService taskInfoService, ICommonService common
        , IUnitOfWork uow)
    {
        _taskService = taskService;
        _taskInfoService = taskInfoService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(EndTaskCommand request, CancellationToken ct)
    {
        var endTaskDto = _common.Mapper.Map<UserTaskAppDto>(request);

        await _taskService.EndTaskAsync(endTaskDto, ct);

        var createTaskInfoDto = _common.Mapper.Map<CreateTaskInfoAppDto>(request);

        // Create taskinfo after ended task
        await _taskInfoService.CreateTaskInfoAsync(createTaskInfoDto, ct);

        await _uow.SaveAsync(ct);
    }
}
