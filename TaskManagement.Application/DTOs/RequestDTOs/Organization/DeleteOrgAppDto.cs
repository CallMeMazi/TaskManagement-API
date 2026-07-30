namespace TaskManagement.Application.DTOs.RequestDTOs.Organization;
public record DeleteOrgAppDto(
    int OrgId,
    int OwnerId,
    string OwnerPassword
);
