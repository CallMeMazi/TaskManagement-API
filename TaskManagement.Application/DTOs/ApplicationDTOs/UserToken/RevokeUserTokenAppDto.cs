namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class RevokeUserTokenAppDto
{
    public int UserId { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
