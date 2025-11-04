using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IOrganizationRepository : IBaseRepository<Organization>
{
    System.Threading.Tasks.Task AddOrgAsync(Organization entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddRangeOrgsAsync(IEnumerable<Organization> entities, CancellationToken cancellationToken = default);
    void DeleteOrg(Organization entity);
    void DeleteRangeOrgs(IEnumerable<Organization> entities);
    ValueTask<Organization?> FindOrgsByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<Organization>> GetAllOrgsAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<List<OrgDetailsDto>> GetAllOrgDtosAsync(CancellationToken cancellationToken = default);
    Task<Organization?> GetOrgByFilterAsync(Expression<Func<Organization, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<Organization?> GetOrgByIdAsync(int orgId, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<OrgDetailsDto?> GetOrgDtoByFilterAsync(Expression<Func<Organization, bool>> filter, CancellationToken cancellationToken = default);
    Task<OrgDetailsDto?> GetOrgDtoByIdAsync(int orgId, CancellationToken cancellationToken = default);
    void UpdateOrg(Organization entity);
    void UpdateRangeOrgs(IEnumerable<Organization> entities);
    Task<bool> IsOrgExistByFilterAsync(Expression<Func<Organization, bool>> filter, CancellationToken cancellationToken);
}