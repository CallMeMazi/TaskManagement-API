using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskActivity;
public class ChangeTaskActivityHandler
    : IRequestHandler<ChangeTaskActivityCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskActivityHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeTaskActivityCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<ChangeTaskActivityAppDto>(request);

        var changeTaskActivityRes = await _taskService.ChangeTaskActivityAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeTaskActivityRes;
    }
}
