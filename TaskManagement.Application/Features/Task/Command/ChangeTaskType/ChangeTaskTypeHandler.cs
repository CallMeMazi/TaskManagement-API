using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.Services.Halper;
using TaskManagement.Application.Interfaces.UnitOfWorks;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public class ChangeTaskTypeHandler
    : IRequestHandler<ChangeTaskTypeCommand>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly ICommonService _common;

    public ChangeTaskTypeHandler(ITaskService taskService, ICommonService common, IUnitOfWork uow)
    {
        _taskService = taskService;
        _common = common;
        _uow = uow;
    }

    public async System.Threading.Tasks.Task Handle(ChangeTaskTypeCommand request, CancellationToken ct)
    {
        var dto = _common.Mapper.Map<UserTaskAppDto>(request);

        await _taskService.ChangeTaskTypeAsync(dto, ct);

        await _uow.SaveAsync(ct);
    }
}
