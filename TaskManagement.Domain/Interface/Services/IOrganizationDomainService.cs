namespace TaskManagement.Domain.Interface.Services;
public interface IOrganizationDomainService
{
    Task EnsureCanCreateOrgAsync(string secondOrgName, long ownerId, CancellationToken ct);
    Task EnsureCanDeactiveOrgAsync(long orgId, CancellationToken ct);
    Task EnsureCanRemoveUserFromOrgAsync(long orgId, long userId, CancellationToken ct);
    Task EnsureCanUpdateOrgAsync(string secondOrgName, long orgId, CancellationToken ct);
    Task EnsureCanUserAddToOrgAsync(long orgId, long userId, CancellationToken ct);
    Task EnsureCanChangeRoleToMemberAsync(long userId, long orgId, CancellationToken ct);
}
