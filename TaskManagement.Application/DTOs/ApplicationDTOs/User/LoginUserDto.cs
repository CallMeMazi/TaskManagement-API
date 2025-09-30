namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class LoginUserDto
{
    public string? MobileNumber { get; set; }
    public string? Email { get; set; }
    public required string Password { get; set; }
}
