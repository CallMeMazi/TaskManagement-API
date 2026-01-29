using TaskManagement.Application.DTOs.ApplicationDTOs.Task;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface ITaskService
{
    Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(int taskId, CancellationToken ct);
    Task<GeneralResult> CreateTaskAsync(CreateTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeTaskStatusAsync(ChangeTaskStatusAppDto command, CancellationToken ct);
}
