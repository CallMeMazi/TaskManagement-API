namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record ChangeActivityOrgAppDto(
    int OrgId,
    int OwnerId,
    string OwnerPassword,
    bool Activity
);