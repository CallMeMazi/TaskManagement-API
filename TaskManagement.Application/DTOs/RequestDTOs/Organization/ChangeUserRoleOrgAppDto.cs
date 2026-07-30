namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record ChangeUserRoleOrgAppDto(
    int OrgOwnerId,
    int OrgId,
    int UserId
);