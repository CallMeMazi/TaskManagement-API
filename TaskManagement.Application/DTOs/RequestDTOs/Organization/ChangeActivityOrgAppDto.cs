namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record ChangeActivityOrgAppDto(
    long OrgId,
    long OwnerId,
    string OwnerPassword,
    bool Activity
);