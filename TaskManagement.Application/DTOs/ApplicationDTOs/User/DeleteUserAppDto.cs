namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class DeleteUserAppDto
{
    public required Guid UserId { get; set; }
    public required string Password { get; set; }
}
