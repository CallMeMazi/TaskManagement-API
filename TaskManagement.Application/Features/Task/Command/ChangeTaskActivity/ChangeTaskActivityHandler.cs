using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityHandler
    : IRequestHandler<ChangeTaskActivityCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public ChangeTaskActivityHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeTaskActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeTaskActivityAppDto>(request);

        var changeTaskActivityRes = await _taskService.ChangeTaskActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeTaskActivityRes;
    }
}
