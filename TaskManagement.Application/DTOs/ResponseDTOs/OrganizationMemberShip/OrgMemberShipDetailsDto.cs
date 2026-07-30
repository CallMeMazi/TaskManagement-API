using TaskManagement.Domain.Enums.Roles;

namespace TaskManagement.Application.DTOs.ResponseDTOs.OrganizationMemberShip;
public record OrgMemberShipDetailsDto(
    int OrgId,
    int UserId,
    OrganizationRoles Role
);