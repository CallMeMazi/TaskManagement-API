namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record RemoveUserOrgAppDto(
    int OrgOwnerId,
    int UserId,
    int OrgId
);