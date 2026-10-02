using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.Task.Query.GetTaskById;
public class GetTaskByIdHandler
    : IRequestHandler<GetTaskByIdQuery, TaskDetailsDto>
{
    private readonly ITaskService _taskService;

    public GetTaskByIdHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public Task<TaskDetailsDto> Handle(GetTaskByIdQuery request, CancellationToken ct)
        => _taskService.GetTaskByIdAsync(request.TaskId, ct);
}
