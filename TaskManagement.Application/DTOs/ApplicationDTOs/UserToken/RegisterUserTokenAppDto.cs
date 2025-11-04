namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public class RegisterUserTokenAppDto
{
    public required Domin.Entities.BaseEntities.User user { get; set; }
    public required string UserIp { get; set; }
    public required string UserAgent { get; set; }
}
