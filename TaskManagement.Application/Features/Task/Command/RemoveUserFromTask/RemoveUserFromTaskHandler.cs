using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public class RemoveUserFromTaskHandler
    : IRequestHandler<RemoveUserFromTaskCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public RemoveUserFromTaskHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(RemoveUserFromTaskCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<AddRemoveUserTaskAppDto>(request);

        var removeUserTaskRes = await _taskService.RemoveUserFromTaskAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return removeUserTaskRes;
    }
}
