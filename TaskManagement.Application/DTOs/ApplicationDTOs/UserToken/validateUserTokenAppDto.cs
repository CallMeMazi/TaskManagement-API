namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class validateUserTokenAppDto
{
    public required string AccessToken { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
