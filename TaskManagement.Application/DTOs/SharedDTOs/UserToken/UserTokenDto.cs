namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
public class UserTokenDto
{
    public required string AccessTokenHash { get; set; }
    public required string RefreshTokenHash { get; set; }
}
