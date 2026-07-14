namespace TaskManagement.Application.DTOs.ApplicationDTOs.UserToken;
public record LogoutUserAppDto(
    int UserId,
    string AccessToken,
    string DeviceId
);