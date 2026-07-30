namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record UpdateOrgAppDto(
    int UserId,
    int OrgId,
    string OrgName,
    string SecondOrgName,
    string OrgDescription
);