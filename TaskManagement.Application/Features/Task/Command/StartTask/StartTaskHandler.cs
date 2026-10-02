using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.StartTask;
public class StartTaskHandler
    : IRequestHandler<StartTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public StartTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(StartTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserTaskAppDto>(request);

        await _taskService.StartTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
