namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class LoginUserAppDto
{
    public required string MobileNumber { get; set; }
    public required string Password { get; set; }
    public required string DeviceId { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
