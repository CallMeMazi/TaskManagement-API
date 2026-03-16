using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Services;
public interface ITaskDomainService
{
    System.Threading.Tasks.Task EnsureCanCreateTaskAsync(Project project, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanChangeTaskStateAsync(Entities.BaseEntities.Task task, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureUserHasAdminRoleAsync(Entities.BaseEntities.Task task, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanAssignUserToTaskAsync(Entities.BaseEntities.Task task, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanChangeTaskTypeAsync(Entities.BaseEntities.Task task, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanRemoveUserFromTaskAsync(Entities.BaseEntities.Task task, int userId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanUserStartTaskAsync(int taskId, CancellationToken ct);
}
