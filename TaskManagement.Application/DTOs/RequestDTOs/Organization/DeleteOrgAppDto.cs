namespace TaskManagement.Application.DTOs.ApplicationDTOs.Organization;
public record DeleteOrgAppDto(
    int OrgId,
    int OwnerId,
    string OwnerPassword
);
