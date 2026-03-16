using TaskManagement.Application.DTOs.ApplicationDTOs.Task;
using TaskManagement.Application.DTOs.SharedDTOs.Task;
using TaskManagement.Common.Classes;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface ITaskService
{
    Task<GeneralResult> AssignUserToTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> CancelTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeTaskProgressAsync(ChangeTaskProgressAppDto command, CancellationToken ct);
    Task<GeneralResult> ChangeTaskTypeAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> CreateTaskAsync(CreateTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> DeadTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> EndTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> FinishTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult<TaskDetailsDto>> GetTaskByIdAsync(int taskId, CancellationToken ct);
    Task<GeneralResult> RemoveUserFromTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> StartTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<GeneralResult> UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct);
}
