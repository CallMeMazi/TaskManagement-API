using TaskManagement.Application.DTOs.ApplicationDTOs.Task;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services;
public interface ITaskService
{
    Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(int taskId, CancellationToken cancellationToken);
    Task<GeneralResult> CreateTaskAsync(CreateTaskAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken cancellationToken);
    Task<GeneralResult> ChangeTaskStatusAsync(ChangeTaskStatusAppDto command, CancellationToken cancellationToken);
}
