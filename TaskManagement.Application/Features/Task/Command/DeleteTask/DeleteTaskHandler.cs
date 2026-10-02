using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;

namespace TaskManagement.Application.Features.Task.Command.DeleteTask;
public class DeleteTaskHandler
    : IRequestHandler<DeleteTaskCommand>
{
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public DeleteTaskHandler(ITaskService taskService, ICommonService common)
    {
        _taskService = taskService;
        _common = common;
    }

    public System.Threading.Tasks.Task Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserTaskAppDto>(request);

        return _taskService.SoftDeleteTaskAsync(dto, ct);
    }
}
