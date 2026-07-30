namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record CreateOrgAppDto(
    string OrgName,
    string SecondOrgName,
    string OrgDescription,
    int OwnerId,
    byte MaxUser
);