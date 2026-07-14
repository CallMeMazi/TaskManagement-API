namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record ChangeUserRoleOrgAppDto(
    int OrgOwnerId,
    int OrgId,
    int UserId
);