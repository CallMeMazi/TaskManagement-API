using MediatR;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Application.Interfaces.Services.Application;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Features.TaskInfo.Query.GetTaskInfoById;
public class GetTaskInfoByIdHandler
    : IRequestHandler<GetTaskInfoByIdQuery, GeneralResult<TaskInfoDetailsDto>>
{
    private readonly ITaskInfoService _taskInfoService;

    public GetTaskInfoByIdHandler(ITaskInfoService taskInfoService)
    {
        _taskInfoService = taskInfoService;
    }

    public Task<GeneralResult<TaskInfoDetailsDto>> Handle(GetTaskInfoByIdQuery request, CancellationToken ct)
        => _taskInfoService.GetTaskInfoByIdAsync(request.TaskInfoId, ct);
}
