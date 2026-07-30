using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface ITaskInfoService
{
    Task<GeneralResult<TaskInfoDetailsDto>> GetTaskInfoByIdAsync(int taskInfoId, CancellationToken ct);
    Task<GeneralResult> CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct);
}
