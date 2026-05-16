using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IOrganizationRepository : IBaseRepository<Organization>
{
    // Query methods
    Task<Organization?> GetOrgByIdWithOwnerAsync(int orgId, bool isTracking = false, CancellationToken ct = default);
    Task<Organization?> GetOrgByIdWithMembersAsync(int orgId, bool isTracking = false, CancellationToken ct = default);

    // Command methods
    Task<int> SoftDeleteOrgSpAsync(int orgId, CancellationToken ct = default);
}