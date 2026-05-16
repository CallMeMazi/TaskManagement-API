using System.Linq.Expressions;
using TaskManagement.Domain.Entities.BaseEntities;

namespace TaskManagement.Domain.Interface.Repository;
public interface IOrganizationInvitationRepository : IBaseRepository<OrganizationInvitation>
{
    Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
