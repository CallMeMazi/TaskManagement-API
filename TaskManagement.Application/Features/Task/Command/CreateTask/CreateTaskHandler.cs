using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.CreateTask;
public class CreateTaskHandler
    : IRequestHandler<CreateTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public CreateTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(CreateTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<CreateTaskAppDto>(request);

        await _taskService.CreateTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
