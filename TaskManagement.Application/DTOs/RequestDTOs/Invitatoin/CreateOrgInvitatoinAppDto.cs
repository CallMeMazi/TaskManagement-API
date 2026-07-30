namespace TaskManagement.Application.DTOs.ApplicationDTOs.Invitatoin;
public record CreateOrgInvitatoinAppDto(
    int OrgId,
    int OrgOwnerId,
    string UserMobileNumber
);
