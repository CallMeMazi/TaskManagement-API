using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityHandler
    : IRequestHandler<ChangeTaskActivityCommand>
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

    public async System.Threading.Tasks.Task Handle(ChangeTaskActivityCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<ChangeTaskActivityAppDto>(request);

        await _taskService.ChangeTaskActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
