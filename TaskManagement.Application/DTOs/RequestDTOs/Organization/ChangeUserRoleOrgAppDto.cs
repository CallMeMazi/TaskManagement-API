namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record ChangeUserRoleOrgAppDto(
    long OrgOwnerId,
    long OrgId,
    long UserId
);