using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Services;
public interface IProjectDomainService
{
    System.Threading.Tasks.Task CheakProjectActiveTaskAsync(int projectId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureUserHasProjectAccessAsync(int ownerId, int orgId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanAddUserToProjectAsync(Project project, int userId, int orgId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanRemoveUserFromProjectAsync(Project project, int userId, CancellationToken ct);
}
