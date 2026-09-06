using TaskManagement.Domain.Enums.Roles;

namespace TaskManagement.Application.DTOs.ResponseDTOs.OrganizationMemberShip;
public record OrgMemberShipDetailsDto(
    long OrgId,
    long UserId,
    OrganizationRoles Role
);