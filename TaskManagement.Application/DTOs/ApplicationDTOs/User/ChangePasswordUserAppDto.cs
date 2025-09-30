namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public class ChangePasswordUserAppDto
{
    public required string MobileNumber { get; set; }
    public required string OldPassword { get; set; }
    public required string NewPassword { get; set; }
}
