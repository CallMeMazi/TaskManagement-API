namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record ChangeActivityOrgAppDto(
    int OrgId,
    int OwnerId,
    string OwnerPassword,
    bool Activity
);