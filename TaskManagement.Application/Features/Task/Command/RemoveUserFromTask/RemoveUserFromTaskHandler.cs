using AutoMapper;
using MediatR;
using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Command.RemoveUserFromTask;
public class RemoveUserFromTaskHandler
    : IRequestHandler<RemoveUserFromTaskCommand, GeneralResult>
{
    private readonly ITaskService _taskService;
    private readonly IMapper _mapper;

    public RemoveUserFromTaskHandler(ITaskService taskService, IMapper mapper)
    {
        _taskService = taskService;
        _mapper = mapper;
    }

    public Task<GeneralResult> Handle(RemoveUserFromTaskCommand request, CancellationToken ct)
    {
        var dto = _mapper.Map<AddRemoveUserTaskAppDto>(request);

        return _taskService.RemoveUserFromTaskAsync(dto, ct);
    }
}
