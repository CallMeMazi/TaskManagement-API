namespace TaskManagement.Application.DTOs.SharedDTOs.UserToken;
public class UserTokenDto
{
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}
