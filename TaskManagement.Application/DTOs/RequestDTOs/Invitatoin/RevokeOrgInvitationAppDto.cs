namespace TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
public record RevokeOrgInvitationAppDto(
    long OrgOwnerId,
    long InvitationId
);
