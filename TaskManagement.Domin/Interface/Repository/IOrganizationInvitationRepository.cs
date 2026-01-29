using System.Linq.Expressions;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Domin.Interface.Repository;
public interface IOrganizationInvitationRepository : IBaseRepository<OrganizationInvitation>
{
    Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
