using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.UpdateTask;
public class UpdateTaskHandler
    : IRequestHandler<UpdateTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public UpdateTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UpdateTaskAppDto>(request);

        await _taskService.UpdateTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
