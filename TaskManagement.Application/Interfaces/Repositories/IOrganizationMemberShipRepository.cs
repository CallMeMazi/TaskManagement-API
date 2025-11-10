using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Organization;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IOrganizationMemberShipRepository : IBaseRepository<OrganizationMemberShip>
{
    System.Threading.Tasks.Task AddOrgMembershipAsync(OrganizationMemberShip entity, CancellationToken cancellationToken = default);
    System.Threading.Tasks.Task AddRangeOrgMembershipsAsync(IEnumerable<OrganizationMemberShip> entities, CancellationToken cancellationToken = default);
    void DeleteOrgMembership(OrganizationMemberShip entity);
    void DeleteRangeOrgMemberships(IEnumerable<OrganizationMemberShip> entities);
    ValueTask<OrganizationMemberShip?> FindOrgMembershipsByIdsAsync(CancellationToken cancellationToken, params object[] ids);
    Task<List<OrganizationMemberShip>> GetAllOrgMembershipsAsync(bool isTracking = false, CancellationToken cancellationToken = default);
    Task<OrganizationMemberShip?> GetOrgMembershipByFilterAsync(Expression<Func<OrganizationMemberShip, bool>> filter, bool isTracking = false, CancellationToken cancellationToken = default);
    Task<OrganizationMemberShip?> GetOrgMembershipByIdAsync(int orgMemberShipId, bool isTracking = false, CancellationToken cancellationToken = default);
    void UpdateOrgMembership(OrganizationMemberShip entity);
    void UpdateRangeOrgMemberships(IEnumerable<OrganizationMemberShip> entities);
    Task<bool> IsOrgMembershipExistByFilterAsync(Expression<Func<OrganizationMemberShip, bool>> filter, CancellationToken cancellationToken);
}
