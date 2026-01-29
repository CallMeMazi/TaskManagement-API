namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class RefreshUserTokenAppDto
{
    public required string RefreshToken { get; set; }
    public required string DeviceId { get; set; }
}
