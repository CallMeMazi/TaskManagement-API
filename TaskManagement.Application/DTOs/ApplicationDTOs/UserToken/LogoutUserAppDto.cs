namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class LogoutUserAppDto
{
    public int UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
