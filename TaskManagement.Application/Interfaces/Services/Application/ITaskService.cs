using TaskManagement.Application.DTOs.RequestDTOs.Task;
using TaskManagement.Application.DTOs.ResponseDTOs.Task;

namespace TaskManagement.Application.Interfaces.Services.Application;
public interface ITaskService
{
    Task AssignUserToTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct);
    Task CancelTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task ChangeTaskActivityAsync(ChangeTaskActivityAppDto command, CancellationToken ct);
    Task ChangeTaskProgressAsync(ChangeTaskProgressAppDto command, CancellationToken ct);
    Task ChangeTaskTypeAsync(UserTaskAppDto command, CancellationToken ct);
    Task CreateTaskAsync(CreateTaskAppDto command, CancellationToken ct);
    Task DeadTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task EndTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task FinishTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task<TaskDetailsDto> GetTaskByIdAsync(long taskId, CancellationToken ct);
    Task RemoveUserFromTaskAsync(AddRemoveUserTaskAppDto command, CancellationToken ct);
    Task SoftDeleteTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task StartTaskAsync(UserTaskAppDto command, CancellationToken ct);
    Task UpdateTaskAsync(UpdateTaskAppDto command, CancellationToken ct);
}
