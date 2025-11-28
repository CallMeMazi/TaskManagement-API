using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.Invitation;
public class OrgInvitationDetailsDto
{
    public int OrgId { get; set; }
    public int UserId { get; set; }
    public required string Token { get; set; }
    public OrgInvitationStatus Status { get; set; }
    public DateTime ExpiredAt { get; set; }
    public DateTime CreatedAt { get; set; }
}
