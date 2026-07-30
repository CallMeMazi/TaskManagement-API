using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.ChangeTaskType;
public class ChangeTaskTypeHandler
    : IRequestHandler<ChangeTaskTypeCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public ChangeTaskTypeHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(ChangeTaskTypeCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<UserTaskAppDto>(request);

        return _taskService.ChangeTaskTypeAsync(dto, ct);
    }
}
