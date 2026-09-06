using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
public record OrgInvitationDetailsDto(
    long OrgId,
    long UserId,
    string Token,
    OrgInvitationStatus Status,
    DateTime ExpiredAt,
    DateTime CreatedAt
);