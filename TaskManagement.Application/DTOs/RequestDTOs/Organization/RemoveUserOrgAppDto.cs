namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record RemoveUserOrgAppDto(
    long OrgOwnerId,
    long UserId,
    long OrgId
);