namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record RemoveUserOrgAppDto(
    int OrgOwnerId,
    int UserId,
    int OrgId
);