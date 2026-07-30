namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record ChangePasswordUserAppDto(
    int UserId,
    string OldPassword,
    string NewPassword,
    string DeviceId
);