namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record CreateOrgAppDto(
    string OrgName,
    string SecondOrgName,
    string OrgDescription,
    int OwnerId,
    byte MaxUser
);