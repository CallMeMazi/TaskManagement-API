namespace TaskManagement.Application.DTOs.RequestDTOs.User;
public record ChangePasswordUserAppDto(
    long UserId,
    string OldPassword,
    string NewPassword,
    string DeviceId
);