using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.AssignUserToTask;
public class AssignUserToTaskHandler
    : IRequestHandler<AssignUserToTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public AssignUserToTaskHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(AssignUserToTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserTaskAppDto>(request);

        return _taskService.AssignUserToTaskAsync(dto, ct);
    }
}
