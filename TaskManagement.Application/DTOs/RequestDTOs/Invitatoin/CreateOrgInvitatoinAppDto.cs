namespace TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
public record CreateOrgInvitatoinAppDto(
    int OrgId,
    int OrgOwnerId,
    string UserMobileNumber
);
