namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record UpdateOrgAppDto(
    int UserId,
    int OrgId,
    string OrgName,
    string SecondOrgName,
    string OrgDescription
);