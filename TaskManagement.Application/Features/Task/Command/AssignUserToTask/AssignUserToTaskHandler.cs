using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public class AssignUserToTaskHandler
    : IRequestHandler<AssignUserToTaskCommand, GeneralResult>
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

    public async Task<GeneralResult> Handle(AssignUserToTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserTaskAppDto>(request);

        var assignUserTask = await _taskService.AssignUserToTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return assignUserTask;
    }
}
