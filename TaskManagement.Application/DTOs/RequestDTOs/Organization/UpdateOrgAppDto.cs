namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record UpdateOrgAppDto(
    long UserId,
    long OrgId,
    string OrgName,
    string SecondOrgName,
    string OrgDescription
);