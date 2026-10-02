using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;

namespace TaskManagement.Application.Features.TaskInfo.Query.GetTaskInfoById;
public class GetTaskInfoByIdHandler
    : IRequestHandler<GetTaskInfoByIdQuery, TaskInfoDetailsDto>
{
    private readonly ITaskInfoService _taskInfoService;

    public GetTaskInfoByIdHandler(ITaskInfoService taskInfoService)
    {
        _taskInfoService = taskInfoService;
    }

    public Task<TaskInfoDetailsDto> Handle(GetTaskInfoByIdQuery request, CancellationToken ct)
        => _taskInfoService.GetTaskInfoByIdAsync(request.TaskInfoId, ct);
}
