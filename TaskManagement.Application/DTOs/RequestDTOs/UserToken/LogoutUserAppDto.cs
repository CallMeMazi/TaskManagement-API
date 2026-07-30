namespace TaskManagement.Application.DTOs.RequestDTOs.UserToken;
public record LogoutUserAppDto(
    int UserId,
    string AccessToken,
    string DeviceId
);