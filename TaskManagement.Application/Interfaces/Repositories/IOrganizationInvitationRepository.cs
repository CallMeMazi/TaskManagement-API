using System.Linq.Expressions;
using TaskManagement.Application.DTOs.SharedDTOs.Invitation;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IOrganizationInvitationRepository : IBaseRepository<OrganizationInvitation, OrgInvitationDetailsDto>
{
    Task<OrganizationInvitation?> GetByFilterWithOrgAsync(Expression<Func<OrganizationInvitation, bool>> filter, bool isTracking = false, CancellationToken ct = default);
}
