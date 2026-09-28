namespace TaskManagement.Application.DTOs.ResponseDTOs.Organization;
public record OrgDetailsDto(
    string OrgName,
    string SecondOrgName,
    string OrgCode,
    string OrgDescription,
    bool IsActive,
    byte MaxUsers,
    DateTime CreatedAt
);