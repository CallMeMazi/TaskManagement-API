using TaskManagement.Domin.Enums.Roles;

namespace TaskManagement.Application.DTOs.SharedDTOs.OrganizationMemberShip;
public class OrgMemberShipDetailsDto
{
    public int OrgId { get; set; }
    public int UserId { get; set; }
    public OrganizationRoles Role { get; set; }
}
