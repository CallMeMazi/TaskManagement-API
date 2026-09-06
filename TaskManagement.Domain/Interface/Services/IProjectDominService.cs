using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Services;
public interface IProjectDomainService
{
    System.Threading.Tasks.Task CheakProjectActiveTaskAsync(long projectId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureUserHasProjectAccessAsync(long ownerId, long orgId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanAddUserToProjectAsync(Project project, long userId, long orgId, CancellationToken ct);
    System.Threading.Tasks.Task EnsureCanRemoveUserFromProjectAsync(Project project, long userId, CancellationToken ct);
}
