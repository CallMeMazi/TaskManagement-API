namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class UpdateUserAppDto
{
    public Guid UserId { get; set; }
    public required string Email { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
}
