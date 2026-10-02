using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Features.Task.Query.GetTaskById;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public class AssignUserToTaskHandler
    : IRequestHandler<AssignUserToTaskCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public AssignUserToTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(AssignUserToTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserTaskAppDto>(request);

        await _taskService.AssignUserToTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
