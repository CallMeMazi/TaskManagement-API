namespace TaskManagement.Domin.Interface.Services;
public interface IOrganizationDomainService
{
    Task EnsureCanCreateOrgAsync(string secondOrgName, int ownerId, CancellationToken ct);
    Task EnsureCanDeactiveOrgAsync(int orgId, CancellationToken ct);
    Task EnsureCanRemoveUserFromOrgAsync(int orgId, int userId, CancellationToken ct);
    Task EnsureCanUpdateOrgAsync(string secondOrgName, int orgId, CancellationToken ct);
    Task EnsureCanUserAddToOrgAsync(int orgId, int userId, CancellationToken ct);
    Task EnsureCanChangeRoleToMemberAsync(int userId, int orgId, CancellationToken ct);
}
