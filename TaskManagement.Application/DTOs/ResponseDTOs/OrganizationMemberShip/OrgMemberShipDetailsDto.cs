using TaskManagement.Domain.Enums.Roles;

namespace TaskManagement.Application.DTOs.SharedDTOs.OrganizationMemberShip;
public record OrgMemberShipDetailsDto(
    int OrgId,
    int UserId,
    OrganizationRoles Role
);