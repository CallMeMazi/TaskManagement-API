namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record DeleteOrgAppDto(
    long OrgId,
    long OwnerId,
    string OwnerPassword
);
