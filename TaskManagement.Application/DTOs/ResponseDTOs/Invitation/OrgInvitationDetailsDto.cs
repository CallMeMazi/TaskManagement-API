using TaskManagement.Domain.Enums.Statuses;

namespace TaskManagement.Application.DTOs.ResponseDTOs.Invitation;
public record OrgInvitationDetailsDto(
    int OrgId,
    int UserId,
    string Token,
    OrgInvitationStatus Status,
    DateTime ExpiredAt,
    DateTime CreatedAt
);