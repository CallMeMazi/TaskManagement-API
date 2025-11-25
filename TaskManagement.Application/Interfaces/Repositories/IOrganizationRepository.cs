using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IOrganizationRepository : IBaseRepository<Organization, OrgDetailsDto>
{
    // Query methods
    Task<Organization?> GetOrgByIdWithOwnerAsync(int orgId, bool isTracking = false, CancellationToken ct = default);
    Task<Organization?> GetOrgByIdWithMembersAsync(int orgId, bool isTracking = false, CancellationToken ct = default);

    // Command methods
    Task<int> SoftDeleteOrgSpAsync(int orgId, CancellationToken ct = default);
}