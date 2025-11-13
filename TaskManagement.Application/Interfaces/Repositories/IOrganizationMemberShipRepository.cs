using TaskManagement.Application.DTOs.SharedDTOs.OrganizationMemberShip;
using TaskManagement.Domin.Entities.BaseEntities;

namespace TaskManagement.Application.Interfaces.Repositories;
public interface IOrganizationMemberShipRepository : IBaseRepository<OrganizationMemberShip, OrgMemberShipDetailsDto>
{
}
