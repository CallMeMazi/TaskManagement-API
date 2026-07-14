using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.Invitation;
public record OrgInvitationDetailsDto(
    int OrgId,
    int UserId,
    string Token,
    OrgInvitationStatus Status,
    DateTime ExpiredAt,
    DateTime CreatedAt
);