using TaskManagement.Application.DTOs.RequestDTOs.TaskInfo;
using TaskManagement.Application.DTOs.ResponseDTOs.TaskInfo;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface ITaskInfoService
{
    Task<TaskInfoDetailsDto> GetTaskInfoByIdAsync(long taskInfoId, CancellationToken ct);
    Task CreateTaskInfoAsync(CreateTaskInfoAppDto command, CancellationToken ct);
}
