namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class RevokeUserTokenAppDto
{
    public int UserId { get; set; }
    public required string Deviceid { get; set; }
}
