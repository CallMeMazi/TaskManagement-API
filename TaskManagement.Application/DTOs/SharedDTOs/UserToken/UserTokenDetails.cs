using TaskManagement.Domin.Enums.Statuses;

namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
public class UserTokenDetails
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
