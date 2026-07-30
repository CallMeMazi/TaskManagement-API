namespace TaskManagement.Application.DTOs.RequestDTOs.Invitatoin;
public record RevokeOrgInvitationAppDto(
    int OrgOwnerId,
    int InvitationId
);
