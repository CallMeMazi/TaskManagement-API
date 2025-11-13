namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
public class UserTokenDetailsDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public required string AccessTokenHash { get; set; }
    public required string RefreshTokenHash { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastUsedAt { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
