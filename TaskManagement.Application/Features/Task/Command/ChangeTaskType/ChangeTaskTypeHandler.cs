using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Application.Interfaces.UnitOfWork;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public class ChangeTaskTypeHandler
    : IRequestHandler<ChangeTaskTypeCommand, GeneralResult>
{
    private readonly IUnitOfWork _uow;
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskTypeHandler(ITaskService taskService, IMapper mapper, IUnitOfWork uow)
    {
        _taskService = taskService;
        _mapper = mapper;
        _uow = uow;
    }

    public async Task<GeneralResult> Handle(ChangeTaskTypeCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        var changeTaskTypeRes = await _taskService.ChangeTaskTypeAsync(dto, ct);

        await _uow.SaveAsync(ct);

        return changeTaskTypeRes;
    }
}
