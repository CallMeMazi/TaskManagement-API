using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IOrganizationRepository : IBaseRepository<Organization>
{
    // Query methods
    Task<Organization?> GetOrgByIdWithOwnerAsync(long orgId, bool isTracking = false, CancellationToken ct = default);
    Task<Organization?> GetOrgByIdWithMembersAsync(long orgId, bool isTracking = false, CancellationToken ct = default);

    // Command methods
    Task<int> SoftDeleteOrgSpAsync(long orgId, CancellationToken ct = default);
}