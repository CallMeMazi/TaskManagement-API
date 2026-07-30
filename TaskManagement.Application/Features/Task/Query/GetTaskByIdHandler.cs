using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.Task.Query;
public class GetTaskByIdHandler
    : IRequestHandler<GetTaskByIdQuery, GeneralResult<TaskDetailsDto>>
{
    private readonly ITaskService _taskService;

    public GetTaskByIdHandler(ITaskService taskService)
    {
        _taskService = taskService;
    }

    public Task<GeneralResult<TaskDetailsDto>> Handle(GetTaskByIdQuery request, CancellationToken ct)
        => _taskService.GetTaskByIdAsync(request.TaskId, ct);
}
