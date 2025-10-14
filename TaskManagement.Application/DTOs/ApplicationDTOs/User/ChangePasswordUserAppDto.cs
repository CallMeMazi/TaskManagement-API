namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class ChangePasswordUserAppDto
{
    public int UserId { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
