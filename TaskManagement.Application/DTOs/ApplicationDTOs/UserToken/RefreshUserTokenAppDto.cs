namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class RefreshUserTokenAppDto
{
    public required string RefreshToken { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
