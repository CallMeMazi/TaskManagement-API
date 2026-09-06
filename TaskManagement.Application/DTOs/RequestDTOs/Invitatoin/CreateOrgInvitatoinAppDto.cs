namespace TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
public record CreateOrgInvitatoinAppDto(
    long OrgId,
    long OrgOwnerId,
    string UserMobileNumber
);
