namespace TaskManagement.Application.DTOs.ApplicationDTOs.User;
public record ChangePasswordUserAppDto(
    int UserId,
    string OldPassword,
    string NewPassword,
    string DeviceId
);