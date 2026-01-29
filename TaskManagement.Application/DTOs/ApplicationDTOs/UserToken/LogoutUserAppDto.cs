namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class LogoutUserAppDto
{
    public int UserId { get; set; }
    public required string AccessToken { get; set; }
    public required string DeviceId { get; set; }
}
